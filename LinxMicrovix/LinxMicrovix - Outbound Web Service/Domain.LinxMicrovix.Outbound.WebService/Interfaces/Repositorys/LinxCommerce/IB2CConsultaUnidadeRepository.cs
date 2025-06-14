﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaUnidadeRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaUnidade? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaUnidade> records);
        public Task<List<B2CConsultaUnidade>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaUnidade> registros);
    }
}
