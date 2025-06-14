﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxSetoresRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxSetores? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxSetores> records);
        public Task<List<string>> GetRegistersExists();
    }
}
