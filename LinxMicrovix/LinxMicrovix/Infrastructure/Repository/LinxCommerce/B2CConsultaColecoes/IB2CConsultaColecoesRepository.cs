﻿using Domain.IntegrationsCore.Entities.Parameters;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce;

namespace LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.LinxCommerce
{
    public interface IB2CConsultaColecoesRepository
    {
        public Task<bool> InsertRecord(LinxMicrovixJobParameter jobParameter, B2CConsultaColecoes? record);
        public Task<bool> InsertParametersIfNotExists(LinxMicrovixJobParameter jobParameter);
        public Task<bool> CreateTableMerge(LinxMicrovixJobParameter jobParameter);
        public bool BulkInsertIntoTableRaw(LinxMicrovixJobParameter jobParameter, List<B2CConsultaColecoes> records);
    }
}
