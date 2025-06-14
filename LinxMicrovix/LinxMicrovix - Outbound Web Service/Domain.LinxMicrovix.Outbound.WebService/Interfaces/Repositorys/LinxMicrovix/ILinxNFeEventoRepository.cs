﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxNFeEventoRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxNFeEvento? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxNFeEvento> records);
        public Task<List<LinxNFeEvento>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxNFeEvento> registros);
    }
}
