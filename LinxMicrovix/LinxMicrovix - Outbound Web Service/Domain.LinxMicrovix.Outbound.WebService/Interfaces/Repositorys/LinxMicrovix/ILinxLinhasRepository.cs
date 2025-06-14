﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxLinhasRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxLinhas? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxLinhas> records);
        public Task<List<LinxLinhas>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxLinhas> registros);
    }
}
