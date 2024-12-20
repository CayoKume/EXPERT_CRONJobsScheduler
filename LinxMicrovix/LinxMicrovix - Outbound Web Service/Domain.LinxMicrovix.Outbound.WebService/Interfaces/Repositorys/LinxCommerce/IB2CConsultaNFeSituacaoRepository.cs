﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaNFeSituacaoRepository
    {
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaNFeSituacao> records);
        public Task<List<B2CConsultaNFeSituacao>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaNFeSituacao> registros);
    }
}
