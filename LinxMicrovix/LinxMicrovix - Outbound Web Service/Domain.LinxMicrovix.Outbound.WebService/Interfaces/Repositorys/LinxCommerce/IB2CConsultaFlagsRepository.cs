﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaFlagsRepository
    {
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaFlags> records);
        public Task<List<B2CConsultaFlags>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaFlags> registros);
    }
}
