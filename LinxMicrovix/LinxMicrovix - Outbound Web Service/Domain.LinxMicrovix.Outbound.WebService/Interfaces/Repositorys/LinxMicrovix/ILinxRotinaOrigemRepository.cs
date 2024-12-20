﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxRotinaOrigemRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxRotinaOrigem? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxRotinaOrigem> records);
        public Task<List<LinxRotinaOrigem>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxRotinaOrigem> registros);
    }
}
