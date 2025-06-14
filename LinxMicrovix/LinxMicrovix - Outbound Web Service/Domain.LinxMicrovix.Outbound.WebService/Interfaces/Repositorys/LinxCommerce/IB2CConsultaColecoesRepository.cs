﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaColecoesRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaColecoes? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaColecoes> records);
        public Task<List<B2CConsultaColecoes>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaColecoes> registros);
    }
}
