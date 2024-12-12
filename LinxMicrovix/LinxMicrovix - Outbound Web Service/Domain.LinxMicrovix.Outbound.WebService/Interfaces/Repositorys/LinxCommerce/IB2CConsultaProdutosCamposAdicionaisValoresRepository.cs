﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaProdutosCamposAdicionaisValoresRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaProdutosCamposAdicionaisValores? record);
        public Task<bool> InsertParametersIfNotExists(LinxAPIParam jobParameter);
        public Task<bool> CreateTableMerge(LinxAPIParam jobParameter);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaProdutosCamposAdicionaisValores> records);
        public Task<List<B2CConsultaProdutosCamposAdicionaisValores>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaProdutosCamposAdicionaisValores> registros);
    }
}
