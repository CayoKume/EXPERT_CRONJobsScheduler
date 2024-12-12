﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaClassificacaoRepository : IB2CConsultaClassificacaoRepository
    {
        private readonly ISQLServerConnection _sqlServerConnection;

        public B2CConsultaClassificacaoRepository(ISQLServerConnection sqlServerConnection) =>
            _sqlServerConnection = sqlServerConnection;

        public bool CreateDataTableIfNotExists(string databaseName, string jobName, string untreatedDatabaseName)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME = '{jobName}'";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaClassificacao>(tableName: $"{jobName}");
                }

                using (var conn = _sqlServerConnection.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaClassificacao>(tableName: $"{jobName}");
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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTACLASSIFICACAO_SYNC')
                           BEGIN
                           EXECUTE (
                               'CREATE PROCEDURE [P_B2CCONSULTACLASSIFICACAO_SYNC] AS
                               BEGIN
	                               MERGE [B2CCONSULTACLASSIFICACAO_TRUSTED] AS TARGET 
	                               USING [B2CCONSULTACLASSIFICACAO_RAW] AS SOURCE 

	                               ON (
                                        TARGET.[CODIGO_CLASSIFICACAO] = SOURCE.[CODIGO_CLASSIFICACAO]
                                   ) 
	                           
                                   WHEN MATCHED AND SOURCE.[parameters_timestamp] != TARGET.[parameters_timestamp] THEN 
                                       UPDATE SET 
	                                   TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], 
	                                   TARGET.[CODIGO_CLASSIFICACAO] = SOURCE.[CODIGO_CLASSIFICACAO], 
	                                   TARGET.[NOME_CLASSIFICACAO] = SOURCE.[NOME_CLASSIFICACAO], 
	                                   TARGET.[parameters_timestamp] = SOURCE.[parameters_timestamp], 
	                                   TARGET.[PORTAL] = SOURCE.[PORTAL] 
	                           
                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[CODIGO_CLASSIFICACAO] NOT IN (SELECT [CODIGO_CLASSIFICACAO] FROM [B2CCONSULTACLASSIFICACAO_TRUSTED]) THEN 
	                                   INSERT ([LASTUPDATEON], [CODIGO_CLASSIFICACAO], [NOME_CLASSIFICACAO], [parameters_timestamp], [PORTAL])
	                                   VALUES (SOURCE.[LASTUPDATEON], SOURCE.[CODIGO_CLASSIFICACAO], SOURCE.[NOME_CLASSIFICACAO], SOURCE.[parameters_timestamp], SOURCE.[PORTAL]);
                               END'
                           )
                           END";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
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
                                   <Parameter id=""codigo_classificacao"">[codigo_classificacao]</Parameter>"
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{parametersTableName}] WHERE [method] = '{jobName}') " +
                              $"INSERT INTO [{parametersTableName}] ([method], [parameters_timestamp], [parameters_dateinterval], [parameters_individual]) " +
                               "VALUES (@method, @timestamp, @dateinterval, @individual)";


                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
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