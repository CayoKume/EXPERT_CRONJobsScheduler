﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaProdutosFlagsRepository : IB2CConsultaProdutosFlagsRepository
    {
        private readonly ILinxMicrovixAzureSQLRepositoryBase<B2CConsultaProdutosFlags> _linxMicrovixRepositoryBase;

        public B2CConsultaProdutosFlagsRepository(ILinxMicrovixAzureSQLRepositoryBase<B2CConsultaProdutosFlags> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaProdutosFlags> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter.tableName, new B2CConsultaProdutosFlags());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].portal, records[i].id_b2c_flags_produtos, records[i].id_b2c_flags, records[i].codigoproduto, records[i].timestamp, records[i].descricao_b2c_flags);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(
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

        public async Task<List<B2CConsultaProdutosFlags>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaProdutosFlags> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].codigoproduto}'";
                    else
                        identificadores += $"'{registros[i].codigoproduto}', ";
                }

                string sql = $"SELECT CODIGOPRODUTO, TIMESTAMP FROM B2CCONSULTAPRODUTOSFLAGS WHERE CODIGOPRODUTO IN ({identificadores})";

                return await _linxMicrovixRepositoryBase.GetRegistersExists(sql);
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
    }
}
