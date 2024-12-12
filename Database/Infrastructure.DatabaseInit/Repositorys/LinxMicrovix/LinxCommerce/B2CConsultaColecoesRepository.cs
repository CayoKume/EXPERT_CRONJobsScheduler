﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaColecoesRepository : IB2CConsultaColecoesRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaColecoesRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaColecoes>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaColecoes>(tableName: $"{jobName}");
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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTACOLECOES_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTACOLECOES_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTACOLECOES_TRUSTED] AS TARGET
                                   USING [B2CCONSULTACOLECOES_RAW] AS SOURCE

                                   ON (
			                           TARGET.CODIGO_COLECAO = SOURCE.CODIGO_COLECAO
		                           )

                                   WHEN MATCHED AND TARGET.[parameters_timestamp] != SOURCE.[parameters_timestamp] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[CODIGO_COLECAO] = SOURCE.[CODIGO_COLECAO],
			                           TARGET.[NOME_COLECAO] = SOURCE.[NOME_COLECAO],
			                           TARGET.[parameters_timestamp] = SOURCE.[parameters_timestamp],
			                           TARGET.[MARCAS] = SOURCE.[MARCAS],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[CODIGO_COLECAO] NOT IN (SELECT [CODIGO_COLECAO] FROM [B2CCONSULTACOLECOES_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [CODIGO_COLECAO], [NOME_COLECAO], [parameters_timestamp], [MARCAS], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[CODIGO_COLECAO], SOURCE.[NOME_COLECAO], SOURCE.[parameters_timestamp], SOURCE.[MARCAS], SOURCE.[PORTAL]);
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
                                                <Parameter id=""codigo_colecao"">[codigo_colecao]</Parameter>"
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