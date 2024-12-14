﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaMarcasRepository : IB2CConsultaMarcasRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaMarcasRepository(ISQLServerConnection? conn) =>
            _conn = conn;

        public bool CreateDataTableIfNotExists(string databaseName, string jobName, string untreatedDatabaseName)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME = '{jobName}'";

            try
            {
                using (var conn = _conn.GetIDbConnection(databaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaMarcas>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaMarcas>(tableName: $"{jobName}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateTableMerge(string databaseName, string tableName)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAMARCAS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAMARCAS_SYNC] AS
	                           BEGIN
		                           MERGE [LINX_MICROVIX_COMMERCE].[dbo].[B2CCONSULTAMARCAS] AS TARGET
		                           USING [UNTREATED].[dbo].[B2CCONSULTAMARCAS] AS SOURCE

		                           ON (TARGET.[CODIGO_MARCA] = SOURCE.[CODIGO_MARCA])

		                           WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[CODIGO_MARCA] = SOURCE.[CODIGO_MARCA],
			                           TARGET.[NOME_MARCA] = SOURCE.[NOME_MARCA],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[LINHAS] = SOURCE.[LINHAS],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

		                           WHEN NOT MATCHED BY TARGET AND SOURCE.[CODIGO_MARCA] NOT IN (SELECT [CODIGO_MARCA] FROM [LINX_MICROVIX_COMMERCE].[dbo].[B2CCONSULTAMARCAS]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [CODIGO_MARCA], [NOME_MARCA], [TIMESTAMP], [LINHAS], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[CODIGO_MARCA], SOURCE.[NOME_MARCA], SOURCE.[TIMESTAMP], SOURCE.[LINHAS], SOURCE.[PORTAL]);
	                           END'
                           )
                           END";

            try
            {
                using (var conn = _conn.GetIDbConnection(databaseName))
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

        public async Task<bool> InsertParametersIfNotExists(string jobName, string parametersTableName, string databaseName)
        {
            try
            {
                var parameter = new
                {
                    method = jobName,
                    timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                <Parameter id=""codigo_marca"">[codigo_marca]</Parameter>"
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{parametersTableName}] WHERE [method] = '{jobName}') " +
                              $"INSERT INTO [{parametersTableName}] ([method], [parameters_timestamp], [parameters_dateinterval], [parameters_individual]) " +
                               "VALUES (@method, @timestamp, @dateinterval, @individual)";


                using (var conn = _conn.GetIDbConnection(databaseName))
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
