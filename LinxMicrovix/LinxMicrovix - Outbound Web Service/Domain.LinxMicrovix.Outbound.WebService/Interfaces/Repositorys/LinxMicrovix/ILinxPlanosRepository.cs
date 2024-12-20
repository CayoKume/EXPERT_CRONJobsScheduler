﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPlanosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPlanos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPlanos> records);
        public Task<List<LinxPlanos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPlanos> registros);
    }
}
