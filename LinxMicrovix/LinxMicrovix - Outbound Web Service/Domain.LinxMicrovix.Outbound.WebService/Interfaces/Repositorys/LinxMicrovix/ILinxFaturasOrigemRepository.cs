﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxFaturasOrigemRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxFaturasOrigem? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxFaturasOrigem> records);
        public Task<List<LinxFaturasOrigem>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxFaturasOrigem> registros);
    }
}
