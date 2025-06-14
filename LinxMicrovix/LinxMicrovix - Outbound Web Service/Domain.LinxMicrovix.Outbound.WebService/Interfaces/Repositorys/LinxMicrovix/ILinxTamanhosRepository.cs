﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxTamanhosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxTamanhos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxTamanhos> records);
        public Task<List<LinxTamanhos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxTamanhos> registros);
    }
}
