﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxRemessasIdentificadoresRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxRemessasIdentificadores? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxRemessasIdentificadores> records);
        public Task<List<LinxRemessasIdentificadores>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxRemessasIdentificadores> registros);
    }
}
