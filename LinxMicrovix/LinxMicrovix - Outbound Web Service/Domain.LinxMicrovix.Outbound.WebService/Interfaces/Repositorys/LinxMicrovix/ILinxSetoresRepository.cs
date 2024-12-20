﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxSetoresRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxSetores? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxSetores> records);
        public Task<List<LinxSetores>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxSetores> registros);
    }
}
