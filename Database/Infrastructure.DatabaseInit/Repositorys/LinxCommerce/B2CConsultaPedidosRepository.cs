﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaPedidosRepository : IB2CConsultaPedidosRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaPedidosRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaPedidos>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaPedidos>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPEDIDOS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAPEDIDOS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAPEDIDOS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAPEDIDOS_RAW] AS SOURCE

                                   ON (
			                           TARGET.[ID_PEDIDO] = SOURCE.[ID_PEDIDO]
		                           )

                                   WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_PEDIDO] = SOURCE.[ID_PEDIDO],
			                           TARGET.[DT_PEDIDO] = SOURCE.[DT_PEDIDO],
			                           TARGET.[COD_CLIENTE_ERP] = SOURCE.[COD_CLIENTE_ERP],
			                           TARGET.[COD_CLIENTE_B2C] = SOURCE.[COD_CLIENTE_B2C],
			                           TARGET.[VL_FRETE] = SOURCE.[VL_FRETE],
			                           TARGET.[FORMA_PGTO] = SOURCE.[FORMA_PGTO],
			                           TARGET.[PLANO_PAGAMENTO] = SOURCE.[PLANO_PAGAMENTO],
			                           TARGET.[ANOTACAO] = SOURCE.[ANOTACAO],
			                           TARGET.[TAXA_IMPRESSAO] = SOURCE.[TAXA_IMPRESSAO],
			                           TARGET.[FINALIZADO] = SOURCE.[FINALIZADO],
			                           TARGET.[VALOR_FRETE_GRATIS] = SOURCE.[VALOR_FRETE_GRATIS],
			                           TARGET.[TIPO_FRETE] = SOURCE.[TIPO_FRETE],
			                           TARGET.[ID_STATUS] = SOURCE.[ID_STATUS],
			                           TARGET.[COD_TRANSPORTADOR] = SOURCE.[COD_TRANSPORTADOR],
			                           TARGET.[TIPO_COBRANCA_FRETE] = SOURCE.[TIPO_COBRANCA_FRETE],
			                           TARGET.[ATIVO] = SOURCE.[ATIVO],
			                           TARGET.[EMPRESA] = SOURCE.[EMPRESA],
			                           TARGET.[ID_TABELA_PRECO] = SOURCE.[ID_TABELA_PRECO],
			                           TARGET.[VALOR_CREDITO] = SOURCE.[VALOR_CREDITO],
			                           TARGET.[COD_VENDEDOR] = SOURCE.[COD_VENDEDOR],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[DT_INSERT] = SOURCE.[DT_INSERT],
			                           TARGET.[DT_DISPONIVEL_FATURAMENTO] = SOURCE.[DT_DISPONIVEL_FATURAMENTO],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL],
			                           TARGET.[MENSAGEM_FALHA_FATURAMENTO] = SOURCE.[MENSAGEM_FALHA_FATURAMENTO],
			                           TARGET.[ID_TIPO_B2C] = SOURCE.[ID_TIPO_B2C],
			                           TARGET.[ECOMMERCE_ORIGEM] = SOURCE.[ECOMMERCE_ORIGEM],
			                           TARGET.[ORDER_ID] = SOURCE.[ORDER_ID]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_PEDIDO] NOT IN (SELECT [ID_PEDIDO] FROM [B2CCONSULTAPEDIDOS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_PEDIDO], [DT_PEDIDO], [COD_CLIENTE_ERP], [COD_CLIENTE_B2C], [VL_FRETE], [FORMA_PGTO], [PLANO_PAGAMENTO], [ANOTACAO], [TAXA_IMPRESSAO], [FINALIZADO], [VALOR_FRETE_GRATIS], [TIPO_FRETE], 
			                           [ID_STATUS], [COD_TRANSPORTADOR], [TIPO_COBRANCA_FRETE], [ATIVO], [EMPRESA], [ID_TABELA_PRECO], [VALOR_CREDITO], [COD_VENDEDOR], [TIMESTAMP], [DT_INSERT], [DT_DISPONIVEL_FATURAMENTO], [PORTAL], [MENSAGEM_FALHA_FATURAMENTO], 
			                           [ID_TIPO_B2C], [ECOMMERCE_ORIGEM], [ORDER_ID])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_PEDIDO], SOURCE.[DT_PEDIDO], SOURCE.[COD_CLIENTE_ERP], SOURCE.[COD_CLIENTE_B2C], SOURCE.[VL_FRETE], SOURCE.[FORMA_PGTO], SOURCE.[PLANO_PAGAMENTO], SOURCE.[ANOTACAO], 
			                           SOURCE.[TAXA_IMPRESSAO], SOURCE.[FINALIZADO], SOURCE.[VALOR_FRETE_GRATIS], SOURCE.[TIPO_FRETE], SOURCE.[ID_STATUS], SOURCE.[COD_TRANSPORTADOR], SOURCE.[TIPO_COBRANCA_FRETE], SOURCE.[ATIVO], SOURCE.[EMPRESA],
			                           SOURCE.[ID_TABELA_PRECO], SOURCE.[VALOR_CREDITO], SOURCE.[COD_VENDEDOR], SOURCE.[TIMESTAMP], SOURCE.[DT_INSERT], SOURCE.[DT_DISPONIVEL_FATURAMENTO], SOURCE.[PORTAL], SOURCE.[MENSAGEM_FALHA_FATURAMENTO], SOURCE.[ID_TIPO_B2C], 
			                           SOURCE.[ECOMMERCE_ORIGEM], SOURCE.[ORDER_ID]);
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
                    parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                <Parameter id=""id_pedido"">[id_pedido]</Parameter>",
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