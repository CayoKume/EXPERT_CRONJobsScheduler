﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaEspessurasRepository : IB2CConsultaEspessurasRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaEspessurasRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaEspessuras>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaEspessuras>(tableName: $"{jobName}");
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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAESPESSURAS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAESPESSURAS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAESPESSURAS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAESPESSURAS_RAW] AS SOURCE

                                   ON (
			                           TARGET.CODIGO_ESPESSURA = SOURCE.CODIGO_ESPESSURA
		                           )

                                   WHEN MATCHED AND TARGET.[parameters_timestamp] != SOURCE.[parameters_timestamp] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[CODIGO_ESPESSURA] = SOURCE.[CODIGO_ESPESSURA],
			                           TARGET.[NOME_ESPESSURA] = SOURCE.[NOME_ESPESSURA],
			                           TARGET.[parameters_timestamp] = SOURCE.[parameters_timestamp],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND CODIGO_ESPESSURA NOT IN (SELECT [CODIGO_ESPESSURA] FROM [B2CCONSULTAESPESSURAS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [CODIGO_ESPESSURA], [NOME_ESPESSURA], [parameters_timestamp], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[CODIGO_ESPESSURA], SOURCE.[NOME_ESPESSURA], SOURCE.[parameters_timestamp], SOURCE.[PORTAL]);
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
                                                <Parameter id=""codigo_espessura"">[codigo_espessura]</Parameter>"
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