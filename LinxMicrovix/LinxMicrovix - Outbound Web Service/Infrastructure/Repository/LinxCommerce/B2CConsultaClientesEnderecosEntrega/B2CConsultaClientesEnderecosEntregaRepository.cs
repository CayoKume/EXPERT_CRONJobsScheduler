﻿using IntegrationsCore.Domain.Entities;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce;
using LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.Base;

namespace LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.LinxCommerce
{
    public class B2CConsultaClientesEnderecosEntregaRepository<TEntity> : IB2CConsultaClientesEnderecosEntregaRepository<TEntity> where TEntity : B2CConsultaClientesEnderecosEntrega, new()
    {
        private readonly ILinxMicrovixRepositoryBase<TEntity> _linxMicrovixRepositoryBase;

        public B2CConsultaClientesEnderecosEntregaRepository(ILinxMicrovixRepositoryBase<TEntity> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(JobParameter jobParameter, List<TEntity> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new TEntity());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].id_endereco_entrega, records[i].cod_cliente_erp, records[i].cod_cliente_b2c, records[i].endereco_cliente, records[i].numero_rua_cliente, records[i].complemento_end_cli, 
                        records[i].cep_cliente, records[i].bairro_cliente, records[i].cidade_cliente, records[i].uf_cliente, records[i].descricao, records[i].principal, records[i].id_cidade, records[i].timestamp, records[i].portal);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(
                    jobParameter: jobParameter,
                    dataTable: table,
                    dataTableRowsNumber: table.Rows.Count
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ExecuteTableMerge(JobParameter jobParameter)
        {
            string sql = $"MERGE [{jobParameter.tableName}_trusted] AS TARGET " +
                         $"USING [{jobParameter.tableName}_raw] AS SOURCE " +
                         "ON (TARGET.ID_ENDERECO_ENTREGA = SOURCE.ID_ENDERECO_ENTREGA) " +
                         "WHEN MATCHED THEN UPDATE SET " +
                         "TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], " +
                         "TARGET.[ID_ENDERECO_ENTREGA] = SOURCE.[ID_ENDERECO_ENTREGA], " +
                         "TARGET.[COD_CLIENTE_ERP] = SOURCE.[COD_CLIENTE_ERP], " +
                         "TARGET.[COD_CLIENTE_B2C] = SOURCE.[COD_CLIENTE_B2C], " +
                         "TARGET.[ENDRECO_CLIENTE] = SOURCE.[ENDRECO_CLIENTE], " +
                         "TARGET.[NUMERO_RUA_CLIENTE] = SOURCE.[NUMERO_RUA_CLIENTE], " +
                         "TARGET.[COMPLEMENTO_END_CLI] = SOURCE.[COMPLEMENTO_END_CLI], " +
                         "TARGET.[CEP_CLIENTE] = SOURCE.[CEP_CLIENTE], " +
                         "TARGET.[BAIRRO_CLIENTE] = SOURCE.[BAIRRO_CLIENTE], " +
                         "TARGET.[CIDADE_CLIENTE] = SOURCE.[CIDADE_CLIENTE], " +
                         "TARGET.[UF_CLIENTE] = SOURCE.[UF_CLIENTE], " +
                         "TARGET.[DESCRICAO] = SOURCE.[DESCRICAO], " +
                         "TARGET.[PRINCIPAL] = SOURCE.[PRINCIPAL], " +
                         "TARGET.[ID_CIDADE] = SOURCE.[ID_CIDADE], " +
                         "TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP], " +
                         "TARGET.[PORTAL] = SOURCE.[PORTAL] " +
                         "WHEN NOT MATCHED BY TARGET THEN " +
                         "INSERT" +
                         "([LASTUPDATEON], [ID_ENDERECO_ENTREGA], [COD_CLIENTE_ERP], [COD_CLIENTE_B2C], [ENDRECO_CLIENTE], [NUMERO_RUA_CLIENTE], [COMPLEMENTO_END_CLI], [CEP_CLIENTE], [BAIRRO_CLIENTE], [CIDADE_CLIENTE], " +
                         "[UF_CLIENTE], [DESCRICAO], [PRINCIPAL], [ID_CIDADE], [TIMESTAMP], [PORTAL]) " +
                         "VALUES " +
                         "(SOURCE.[LASTUPDATEON], SOURCE.[ID_ENDERECO_ENTREGA], SOURCE.[COD_CLIENTE_ERP], SOURCE.[COD_CLIENTE_B2C], SOURCE.[ENDRECO_CLIENTE], SOURCE.[NUMERO_RUA_CLIENTE], SOURCE.[COMPLEMENTO_END_CLI], SOURCE.[CEP_CLIENTE], " +
                         "SOURCE.[BAIRRO_CLIENTE], SOURCE.[CIDADE_CLIENTE], SOURCE.[UF_CLIENTE], SOURCE.[DESCRICAO], SOURCE.[PRINCIPAL], SOURCE.[ID_CIDADE], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);";

            try
            {
                return await _linxMicrovixRepositoryBase.ExecuteQueryCommand(jobParameter: jobParameter, sql: sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertParametersIfNotExists(JobParameter jobParameter)
        {
            try
            {
                return await _linxMicrovixRepositoryBase.InsertParametersIfNotExists(
                    jobParameter: jobParameter,
                    parameter: new
                    {
                        method = jobParameter.jobName,
                        parameters_timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        parameters_dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                  <Parameter id=""cod_cliente_b2c"">[cod_cliente_b2c]</Parameter>",
                        ativo = 1
                    }
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertRecord(JobParameter jobParameter, TEntity? record)
        {
            string sql = $"INSERT INTO ([LASTUPDATEON], [ID_ENDERECO_ENTREGA], [COD_CLIENTE_ERP], [COD_CLIENTE_B2C], [ENDRECO_CLIENTE], [NUMERO_RUA_CLIENTE], [COMPLEMENTO_END_CLI], [CEP_CLIENTE], [BAIRRO_CLIENTE], " +
                          "[CIDADE_CLIENTE], [UF_CLIENTE], [DESCRICAO], [PRINCIPAL], [ID_CIDADE], [TIMESTAMP], [PORTAL]) " + 
                          "VALUES " +
                          "(@lastupdateon, @id_endereco_entrega, @cod_cliente_erp, @cod_cliente_b2c, @endereco_cliente, @numero_rua_cliente, @complemento_end_cli, @cep_cliente, " +
                          "@bairro_cliente, @cidade_cliente, @uf_cliente, @descricao, @principal, @id_cidade, @timestamp, @portal)";

            try
            {
                return await _linxMicrovixRepositoryBase.InsertRecord(jobParameter: jobParameter, sql: sql, record: record);
            }
            catch
            {
                throw;
            }
        }
    }
}
