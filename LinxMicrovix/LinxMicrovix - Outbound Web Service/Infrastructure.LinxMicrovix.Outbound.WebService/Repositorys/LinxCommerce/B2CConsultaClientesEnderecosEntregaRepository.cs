﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaClientesEnderecosEntregaRepository : IB2CConsultaClientesEnderecosEntregaRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaClientesEnderecosEntrega> _linxMicrovixRepositoryBase;

        public B2CConsultaClientesEnderecosEntregaRepository(ILinxMicrovixRepositoryBase<B2CConsultaClientesEnderecosEntrega> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaClientesEnderecosEntrega> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaClientesEnderecosEntrega());

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

        public async Task<List<B2CConsultaClientesEnderecosEntrega>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaClientesEnderecosEntrega> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].id_endereco_entrega}'";
                    else
                        identificadores += $"'{registros[i].id_endereco_entrega}', ";
                }

                string sql = $"SELECT ID_ENDERECO_ENTREGA, TIMESTAMP FROM B2CCONSULTACLIENTESENDERECOSENTREGA WHERE ID_ENDERECO_ENTREGA IN ({identificadores})";

                return await _linxMicrovixRepositoryBase.GetRegistersExists(jobParameter, sql);
            }
            catch (Exception ex) when (ex is not InternalException && ex is not SQLCommandException)
            {
                throw new InternalException(
                    stage: EnumStages.GetRegistersExists,
                    error: EnumError.Exception,
                    level: EnumMessageLevel.Error,
                    message: "Error when filling identifiers to sql command",
                    exceptionMessage: ex.Message
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaClientesEnderecosEntrega? record)
        {
            string? sql = $"INSERT INTO ([LASTUPDATEON], [ID_ENDERECO_ENTREGA], [COD_CLIENTE_ERP], [COD_CLIENTE_B2C], [ENDRECO_CLIENTE], [NUMERO_RUA_CLIENTE], [COMPLEMENTO_END_CLI], [CEP_CLIENTE], [BAIRRO_CLIENTE], " +
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
