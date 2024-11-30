﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaProdutosPalavrasChavePesquisaRepository : IB2CConsultaProdutosPalavrasChavePesquisaRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaProdutosPalavrasChavePesquisaRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaProdutosPalavrasChavePesquisa>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaProdutosPalavrasChavePesquisa>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPRODUTOSPALAVRASCHAVEPESQUISA_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAPRODUTOSPALAVRASCHAVEPESQUISA_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAPRODUTOSPALAVRASCHAVEPESQUISA_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAPRODUTOSPALAVRASCHAVEPESQUISA_RAW] AS SOURCE

                                   ON (
			                           TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS]
		                           )

                                   WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS],
			                           TARGET.[ID_B2C_PALAVRAS_CHAVE_PESQUISA] = SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA],
			                           TARGET.[CODIGOPRODUTO] = SOURCE.[CODIGOPRODUTO],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS] NOT IN (SELECT [ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS] FROM [B2CCONSULTAPRODUTOSPALAVRASCHAVEPESQUISA_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS], [ID_B2C_PALAVRAS_CHAVE_PESQUISA], [CODIGOPRODUTO], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA_PRODUTOS], SOURCE.[ID_B2C_PALAVRAS_CHAVE_PESQUISA], SOURCE.[CODIGOPRODUTO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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
