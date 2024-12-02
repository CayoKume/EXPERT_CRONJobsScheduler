﻿using Dapper;
using DatabaseInit.Domain.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repository.LinxCommerce
{
    public class B2CConsultaPalavrasChavePesquisaRepository : IB2CConsultaPalavrasChavePesquisaRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaPalavrasChavePesquisaRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaPalavrasChavePesquisa>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaPalavrasChavePesquisa>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = $@"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPALAVRASCHAVEPESQUISA_SYNC')
                            BEGIN
                            EXECUTE (
	                            'CREATE PROCEDURE [P_B2CCONSULTAPALAVRASCHAVEPESQUISA_SYNC] AS
	                            BEGIN
		                            MERGE [B2CCONSULTAPALAVRASCHAVEPESQUISA_TRUSTED] AS TARGET
                                    USING [B2CCONSULTAPALAVRASCHAVEPESQUISA_RAW] AS SOURCE

                                    ON (TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA])

                                    WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                            UPDATE SET
			                            TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                            TARGET.[PORTAL] = SOURCE.[PORTAL],
			                            TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA],
			                            TARGET.[NOME_COLECAO] = SOURCE.[NOME_COLECAO],
			                            TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP]

                                    WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] NOT IN (SELECT [ID_B2C_PALAVRAS_CHAVE_PESQUISA] FROM [B2CCONSULTAPALAVRASCHAVEPESQUISA_TRUSTED]) THEN
			                            INSERT
			                            ([LASTUPDATEON], [PORTAL], [ID_B2C_PALAVRAS_CHAVE_PESQUISA], [NOME_COLECAO], [TIMESTAMP])
			                            VALUES
			                            (SOURCE.[LASTUPDATEON], SOURCE.[PORTAL], SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA], SOURCE.[NOME_COLECAO], SOURCE.[TIMESTAMP]);
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