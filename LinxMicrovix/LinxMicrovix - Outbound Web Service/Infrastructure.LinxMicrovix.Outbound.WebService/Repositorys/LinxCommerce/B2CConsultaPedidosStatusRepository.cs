﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaPedidosStatusRepository : IB2CConsultaPedidosStatusRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPedidosStatus> _linxMicrovixRepositoryBase;

        public B2CConsultaPedidosStatusRepository(ILinxMicrovixRepositoryBase<B2CConsultaPedidosStatus> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaPedidosStatus> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaPedidosStatus());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].id, records[i].id_status, records[i].id_pedido, records[i].data_hora, records[i].anotacao, records[i].timestamp, records[i].portal);
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

        public async Task<bool> CreateTableMerge(LinxAPIParam jobParameter)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPEDIDOSSTATUS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAPEDIDOSSTATUS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAPEDIDOSSTATUS] AS TARGET
		                           USING [B2CCONSULTAPEDIDOSSTATUS] AS SOURCE

		                           ON (
			                           TARGET.[ID] = SOURCE.[ID]
		                           )

		                           WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID] = SOURCE.[ID],
			                           TARGET.[ID_STATUS] = SOURCE.[ID_STATUS],
			                           TARGET.[ID_PEDIDO] = SOURCE.[ID_PEDIDO],
			                           TARGET.[DATA_HORA] = SOURCE.[DATA_HORA],
			                           TARGET.[ANOTACAO] = SOURCE.[ANOTACAO],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

		                           WHEN NOT MATCHED BY TARGET AND SOURCE.[ID] NOT IN (SELECT [ID] FROM [B2CCONSULTAPEDIDOSSTATUS]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID], [ID_STATUS], [ID_PEDIDO], [DATA_HORA], [ANOTACAO], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID], SOURCE.[ID_STATUS], SOURCE.[ID_PEDIDO], SOURCE.[DATA_HORA], SOURCE.[ANOTACAO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
	                           END'
                           )
                           END";

            try
            {
                return await _linxMicrovixRepositoryBase.ExecuteQueryCommand(jobParameter: jobParameter, sql: sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<B2CConsultaPedidosStatus>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaPedidosStatus> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].id_pedido}'";
                    else
                        identificadores += $"'{registros[i].id_pedido}', ";
                }

                string sql = $"SELECT ID_PEDIDO, TIMESTAMP FROM B2CCONSULTAPEDIDOSSTATUS WHERE ID_PEDIDO IN ({identificadores})";

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

        public async Task<bool> InsertParametersIfNotExists(LinxAPIParam jobParameter)
        {
            try
            {
                return await _linxMicrovixRepositoryBase.InsertParametersIfNotExists(
                    jobParameter: jobParameter,
                    parameter: new
                    {
                        method = jobParameter.jobName,
                        timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                    <Parameter id=""data_inicial"">[data_inicial]</Parameter>
                                                    <Parameter id=""data_fim"">[data_fim]</Parameter>",
                        individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                  <Parameter id=""id_pedido"">[id_pedido]</Parameter>",
                        ativo = 1
                    }
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaPedidosStatus? record)
        {
            string? sql = $"INSERT INTO {jobParameter.tableName} " +
                          "([lastupdateon], [id], [id_status], [id_pedido], [data_hora], [anotacao], [timestamp], [portal]) " +
                          "Values " +
                          "(@lastupdateon, @id, @id_status, @id_pedido, @data_hora, @anotacao, @timestamp, @portal)";

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
