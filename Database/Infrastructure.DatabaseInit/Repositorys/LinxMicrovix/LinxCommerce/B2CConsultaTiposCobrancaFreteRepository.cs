﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaTiposCobrancaFreteRepository : IB2CConsultaTiposCobrancaFreteRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaTiposCobrancaFreteRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaTiposCobrancaFrete>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaTiposCobrancaFrete>(tableName: $"{jobName}");
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
            string? sql = @$"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTATIPOFRETE_SYNC')
                           BEGIN
                           EXECUTE (
                               'CREATE PROCEDURE [P_B2CCONSULTATIPOFRETE_SYNC] AS
                               BEGIN
									MERGE [LINX_MICROVIX_COMMERCE].[dbo].[B2CCONSULTATIPOFRETE] AS TARGET
									USING [UNTREATED].[dbo].[B2CCONSULTATIPOFRETE] AS SOURCE
									ON (TARGET.CODIG_TIPO_COBRANCA_FRETE = SOURCE.CODIG_TIPO_COBRANCA_FRETE)
									WHEN MATCHED THEN UPDATE SET
									TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
									TARGET.[CODIG_TIPO_COBRANCA_FRETE] = SOURCE.[CODIG_TIPO_COBRANCA_FRETE],
									TARGET.[NOME_TIPO_COBRANCA_FRETE] = SOURCE.[NOME_TIPO_COBRANCA_FRETE],
									TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
									TARGET.[PORTAL] = SOURCE.[PORTAL]
									WHEN NOT MATCHED BY TARGET THEN
									INSERT
									([LASTUPDATEON], [CODIG_TIPO_COBRANCA_FRETE], [NOME_TIPO_COBRANCA_FRETE], [TIMESTAMP], [PORTAL])
									VALUES
									(SOURCE.[LASTUPDATEON], SOURCE.[CODIG_TIPO_COBRANCA_FRETE], SOURCE.[NOME_TIPO_COBRANCA_FRETE], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);                        
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
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>"
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
