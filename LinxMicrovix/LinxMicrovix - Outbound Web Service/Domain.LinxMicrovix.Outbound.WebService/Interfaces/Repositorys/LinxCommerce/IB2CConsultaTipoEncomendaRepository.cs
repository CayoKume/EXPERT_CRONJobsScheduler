﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaTipoEncomendaRepository
    {
        public Task<bool> InsertParametersIfNotExists(LinxAPIParam jobParameter);
        public Task<bool> CreateTableMerge(LinxAPIParam jobParameter);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaTipoEncomenda> records);
        public Task<List<B2CConsultaTipoEncomenda>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaTipoEncomenda> registros);
    }
}
