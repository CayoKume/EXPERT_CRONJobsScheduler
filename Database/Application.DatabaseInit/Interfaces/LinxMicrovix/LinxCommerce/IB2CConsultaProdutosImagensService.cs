﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce
{
    public interface IB2CConsultaProdutosImagensService
    {
        public List<B2CConsultaProdutosImagens?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records);
        public Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter);
        public Task<bool> GetRecord(LinxMicrovixJobParameter jobParameter, string? identificador, string? cnpj_emp);
    }
}