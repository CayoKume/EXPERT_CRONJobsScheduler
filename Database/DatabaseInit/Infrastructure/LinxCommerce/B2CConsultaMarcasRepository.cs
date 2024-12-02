﻿using Dapper;
using DatabaseInit.Domain.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repository.LinxCommerce
{
    public class B2CConsultaMarcasRepository : IB2CConsultaMarcasRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaMarcasRepository(ISQLServerConnection? conn) =>
            (_conn) = (conn);

        public Task<bool> CreateDataTableIfNotExists(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME LIKE '%{jobParameter.jobName}%'";

            try
            {
                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.QueryAsync(sql: sql);

                    if (result.Count() == 0)
                    {
                        conn.CreateTable<B2CConsultaVendedores>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaVendedores>(tableName: $"{jobParameter.jobName}_trusted");

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateTableMerge(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAMARCAS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAMARCAS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAMARCAS_TRUSTED] AS TARGET
		                           USING [B2CCONSULTAMARCAS_RAW] AS SOURCE

		                           ON (TARGET.[CODIGO_MARCA] = SOURCE.[CODIGO_MARCA])

		                           WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[CODIGO_MARCA] = SOURCE.[CODIGO_MARCA],
			                           TARGET.[NOME_MARCA] = SOURCE.[NOME_MARCA],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[LINHAS] = SOURCE.[LINHAS],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

		                           WHEN NOT MATCHED BY TARGET AND SOURCE.[CODIGO_MARCA] NOT IN (SELECT [CODIGO_MARCA] FROM [B2CCONSULTAMARCAS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [CODIGO_MARCA], [NOME_MARCA], [TIMESTAMP], [LINHAS], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[CODIGO_MARCA], SOURCE.[NOME_MARCA], SOURCE.[TIMESTAMP], SOURCE.[LINHAS], SOURCE.[PORTAL]);
	                           END'
                           )
                           END";

            try
            {
                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertParametersIfNotExists(LinxMicrovixJobParameter jobParameter)
        {
            try
            {
                var parameter = new {
                    method = jobParameter.jobName,
                    parameters_timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    parameters_dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                <Parameter id=""codigo_marca"">[codigo_marca]</Parameter>",
                    ativo = 1
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{jobParameter.parametersTableName}] WHERE [method] = '{jobParameter.jobName}') " +
                              $"INSERT INTO [{jobParameter.parametersTableName}] ([method], [parameters_timestamp], [parameters_dateinterval], [parameters_individual], [ativo]) " +
                               "VALUES (@method, @parameters_timestamp, @parameters_dateinterval, @parameters_individual, @ativo)";


                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: parameter, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}