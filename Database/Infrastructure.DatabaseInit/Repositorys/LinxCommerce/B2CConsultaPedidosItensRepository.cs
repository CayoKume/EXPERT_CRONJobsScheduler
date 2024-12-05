﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaPedidosItensRepository : IB2CConsultaPedidosItensRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaPedidosItensRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaPedidosItens>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaPedidosItens>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPEDIDOSITENS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAPEDIDOSITENS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAPEDIDOSITENS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAPEDIDOSITENS_RAW] AS SOURCE

                                   ON (
			                           TARGET.[ID_PEDIDO_ITEM] = SOURCE.[ID_PEDIDO_ITEM]
		                           )

                                   WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_PEDIDO_ITEM] = SOURCE.[ID_PEDIDO_ITEM],
			                           TARGET.[ID_PEDIDO] = SOURCE.[ID_PEDIDO],
			                           TARGET.[CODIGOPRODUTO] = SOURCE.[CODIGOPRODUTO],
			                           TARGET.[QUANTIDADE] = SOURCE.[QUANTIDADE],
			                           TARGET.[VL_UNITARIO] = SOURCE.[VL_UNITARIO],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_PEDIDO_ITEM] NOT IN (SELECT [ID_PEDIDO_ITEM] FROM [B2CCONSULTAPEDIDOSITENS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_PEDIDO_ITEM], [ID_PEDIDO], [CODIGOPRODUTO], [QUANTIDADE], [VL_UNITARIO], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_PEDIDO_ITEM], SOURCE.[ID_PEDIDO], SOURCE.[CODIGOPRODUTO], SOURCE.[QUANTIDADE], SOURCE.[VL_UNITARIO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                <Parameter id=""id_pedido"">[id_pedido]</Parameter>",
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
