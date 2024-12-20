﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaPedidosIdentificadorRepository : IB2CConsultaPedidosIdentificadorRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPedidosIdentificador> _linxMicrovixRepositoryBase;

        public B2CConsultaPedidosIdentificadorRepository(ILinxMicrovixRepositoryBase<B2CConsultaPedidosIdentificador> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaPedidosIdentificador> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaPedidosIdentificador());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].portal, records[i].empresa, records[i].identificador, records[i].id_venda, records[i].order_id, records[i].id_cliente, records[i].valor_frete,
                        records[i].data_origem, records[i].timestamp);
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

        public async Task<List<B2CConsultaPedidosIdentificador>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaPedidosIdentificador> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].identificador}'";
                    else
                        identificadores += $"'{registros[i].identificador}', ";
                }

                string sql = $"SELECT IDENTIFICADOR, TIMESTAMP FROM B2CCONSULTAPEDIDOSIDENTIFICADOR WHERE IDENTIFICADOR IN ({identificadores})";

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

        public async Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaPedidosIdentificador? record)
        {
            string? sql = $"INSERT INTO {jobParameter.tableName} " +
                          "([lastupdateon], [portal], [empresa], [identificador], [id_venda], [order_id], [id_cliente], [valor_frete], [data_origem], [timestamp]) " +
                          "Values " +
                          "(@lastupdateon, @portal, @empresa, @identificador, @id_venda, @order_id, @id_cliente, @valor_frete, @data_origem, @timestamp)";

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
