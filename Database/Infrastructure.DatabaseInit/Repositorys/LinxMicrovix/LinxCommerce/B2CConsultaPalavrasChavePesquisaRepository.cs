﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaPalavrasChavePesquisaRepository : IB2CConsultaPalavrasChavePesquisaRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaPalavrasChavePesquisaRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaPalavrasChavePesquisa>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(untreatedDatabaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaPalavrasChavePesquisa>(tableName: $"{jobName}");
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
            string? sql = $@"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPALAVRASCHAVEPESQUISA_SYNC')
                            BEGIN
                            EXECUTE (
	                            'CREATE PROCEDURE [P_B2CCONSULTAPALAVRASCHAVEPESQUISA_SYNC] AS
	                            BEGIN
		                            MERGE [LINX_MICROVIX_COMMERCE].[dbo].[B2CCONSULTAPALAVRASCHAVEPESQUISA] AS TARGET
                                    USING [UNTREATED].[dbo].[B2CCONSULTAPALAVRASCHAVEPESQUISA] AS SOURCE

                                    ON (TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA])

                                    WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                            UPDATE SET
			                            TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                            TARGET.[PORTAL] = SOURCE.[PORTAL],
			                            TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA],
			                            TARGET.[NOME_COLECAO] = SOURCE.[NOME_COLECAO],
			                            TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP]

                                    WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] NOT IN (SELECT [ID_B2C_PALAVRAS_CHAVE_PESQUISA] FROM [LINX_MICROVIX_COMMERCE].[dbo].[B2CCONSULTAPALAVRASCHAVEPESQUISA]) THEN
			                            INSERT
			                            ([LASTUPDATEON], [PORTAL], [ID_B2C_PALAVRAS_CHAVE_PESQUISA], [NOME_COLECAO], [TIMESTAMP])
			                            VALUES
			                            (SOURCE.[LASTUPDATEON], SOURCE.[PORTAL], SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA], SOURCE.[NOME_COLECAO], SOURCE.[TIMESTAMP]);
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
