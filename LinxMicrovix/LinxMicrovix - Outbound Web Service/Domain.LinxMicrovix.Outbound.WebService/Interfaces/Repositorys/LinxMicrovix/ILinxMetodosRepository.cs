﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxMetodosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMetodos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMetodos> records);
        public Task<List<LinxMetodos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMetodos> registros);
    }
}
