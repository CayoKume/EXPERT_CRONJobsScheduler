﻿using Dapper;
using DatabaseInit.Domain.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repository.LinxCommerce
{
    public class B2CConsultaNFeSituacaoRepository : IB2CConsultaNFeSituacaoRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaNFeSituacaoRepository(ISQLServerConnection? conn) =>
            (_conn) = (conn);

        public async Task<bool> CreateDataTableIfNotExists(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME LIKE '%{jobParameter.jobName}%'";

            try
            {
                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.QueryAsync(sql: sql);

                    if (result.Count() == 0)
                    {
                        conn.CreateTable<B2CConsultaNFeSituacao>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaNFeSituacao>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTANFESITUACAO_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTANFESITUACAO_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTANFESITUACAO_TRUSTED] AS TARGET
                                   USING [B2CCONSULTANFESITUACAO_RAW] AS SOURCE

                                   ON (
			                           TARGET.[ID_NFE_SITUACAO] = SOURCE.[ID_NFE_SITUACAO]
		                           )

                                   WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_NFE_SITUACAO] = SOURCE.[ID_NFE_SITUACAO],
			                           TARGET.[DESCRICAO] = SOURCE.[DESCRICAO],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_NFE_SITUACAO] NOT IN (SELECT [ID_NFE_SITUACAO] FROM [B2CCONSULTANFESITUACAO_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_NFE_SITUACAO], [DESCRICAO], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_NFE_SITUACAO], SOURCE.[DESCRICAO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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
                    parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>",
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
