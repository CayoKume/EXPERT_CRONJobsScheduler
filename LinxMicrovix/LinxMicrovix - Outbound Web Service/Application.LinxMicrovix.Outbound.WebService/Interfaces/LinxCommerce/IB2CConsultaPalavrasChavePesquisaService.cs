﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce
{
    public interface IB2CConsultaPalavrasChavePesquisaService
    {
        public List<B2CConsultaPalavrasChavePesquisa?> DeserializeXMLToObject(LinxAPIParam jobParameter, List<Dictionary<string?, string?>> records);
        public Task<bool> GetRecords(LinxAPIParam jobParameter);
    }
}
