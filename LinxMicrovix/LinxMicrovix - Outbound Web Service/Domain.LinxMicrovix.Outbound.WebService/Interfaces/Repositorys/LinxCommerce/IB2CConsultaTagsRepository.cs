﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaTagsRepository
    {
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaTags> records);
        public Task<List<B2CConsultaTags>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaTags> registros);
    }
}
