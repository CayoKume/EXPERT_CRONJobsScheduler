﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix_Outbound_Web_Service.Entites.LinxCommerce;

namespace Application.LinxMicrovix_Outbound_Web_Service.Interfaces.LinxCommerce
{
    public interface IB2CConsultaSetoresService
    {
        public List<B2CConsultaSetores?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records);
        public Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter);
        public Task<bool> GetRecord(LinxMicrovixJobParameter jobParameter, string? identificador, string? cnpj_emp);
    }
}
