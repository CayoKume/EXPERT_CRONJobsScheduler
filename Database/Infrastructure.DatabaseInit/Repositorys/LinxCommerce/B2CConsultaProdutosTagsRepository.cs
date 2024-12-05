﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaProdutosTagsRepository : IB2CConsultaProdutosTagsRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaProdutosTagsRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaProdutosTags>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaProdutosTags>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPRODUTOSTAGS_SYNC')
                            BEGIN
                            EXECUTE (
	                            'CREATE PROCEDURE [P_B2CCONSULTAPRODUTOSTAGS_SYNC] AS
	                            BEGIN
		                            MERGE [B2CCONSULTAPRODUTOSTAGS_TRUSTED] AS TARGET
                                    USING [B2CCONSULTAPRODUTOSTAGS_RAW] AS SOURCE

                                    ON (
			                            TARGET.[ID_B2C_TAGS_PRODUTOS] = SOURCE.[ID_B2C_TAGS_PRODUTOS]
		                            )

                                    WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                            UPDATE SET
			                            TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                            TARGET.[ID_B2C_TAGS_PRODUTOS] = SOURCE.[ID_B2C_TAGS_PRODUTOS],
			                            TARGET.[ID_B2C_TAGS] = SOURCE.[ID_B2C_TAGS],
			                            TARGET.[CODIGOPRODUTO] = SOURCE.[CODIGOPRODUTO],
			                            TARGET.[DESCRICAO_B2C_TAGS] = SOURCE.[DESCRICAO_B2C_TAGS],
			                            TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                            TARGET.[PORTAL] = SOURCE.[PORTAL]

                                    WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_B2C_TAGS_PRODUTOS] NOT IN (SELECT [ID_B2C_TAGS_PRODUTOS] FROM [B2CCONSULTAPRODUTOSTAGS_TRUSTED]) THEN
			                            INSERT
			                            ([LASTUPDATEON], [ID_B2C_TAGS_PRODUTOS], [ID_B2C_TAGS], [CODIGOPRODUTO], [DESCRICAO_B2C_TAGS], [TIMESTAMP], [PORTAL])
			                            VALUES
			                            (SOURCE.[LASTUPDATEON], SOURCE.[ID_B2C_TAGS_PRODUTOS], SOURCE.[ID_B2C_TAGS], SOURCE.[CODIGOPRODUTO], SOURCE.[DESCRICAO_B2C_TAGS], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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
                    timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    ativo = 1
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{jobParameter.parametersTableName}] WHERE [method] = '{jobParameter.jobName}') " +
                              $"INSERT INTO [{jobParameter.parametersTableName}] ([method], [timestamp], [dateinterval], [individual], [ativo]) " +
                               "VALUES (@method, @timestamp, @dateinterval, @individual, @ativo)";


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
