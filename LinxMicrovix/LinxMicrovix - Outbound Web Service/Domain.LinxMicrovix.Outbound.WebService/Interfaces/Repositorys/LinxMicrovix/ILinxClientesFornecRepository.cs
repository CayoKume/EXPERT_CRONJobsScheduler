﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxClientesFornecRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxClientesFornec? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxClientesFornec> records);
        public Task<List<LinxClientesFornec>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxClientesFornec> registros);
    }
}
