﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix_Outbound_Web_Service.Entites.LinxCommerce;

namespace Application.LinxMicrovix_Outbound_Web_Service.Interfaces.LinxCommerce
{
    public interface IB2CConsultaProdutosDetalhesDepositosService
    {
        public List<B2CConsultaProdutosDetalhesDepositos?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records);
        public Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter);
        public Task<bool> GetRecord(LinxMicrovixJobParameter jobParameter, string? identificador, string? identificador1, string? cnpj_emp);
    }
}