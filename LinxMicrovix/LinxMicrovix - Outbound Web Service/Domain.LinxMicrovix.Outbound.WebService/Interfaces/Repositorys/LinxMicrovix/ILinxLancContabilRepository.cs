﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxLancContabilRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxLancContabil? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxLancContabil> records);
        public Task<List<LinxLancContabil>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxLancContabil> registros);
    }
}
