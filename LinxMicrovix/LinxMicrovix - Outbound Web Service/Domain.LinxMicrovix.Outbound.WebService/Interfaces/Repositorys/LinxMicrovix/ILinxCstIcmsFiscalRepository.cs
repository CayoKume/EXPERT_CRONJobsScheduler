﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxCstIcmsFiscalRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxCstIcmsFiscal? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxCstIcmsFiscal> records);
        public Task<List<LinxCstIcmsFiscal>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxCstIcmsFiscal> registros);
    }
}
