﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce
{
    public interface IB2CConsultaTiposCobrancaFreteService
    {
        public List<B2CConsultaTiposCobrancaFrete?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records);
        public Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter);
    }
}