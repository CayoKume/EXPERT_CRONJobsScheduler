﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxListaDaVezRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxListaDaVez? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxListaDaVez> records);
        public Task<List<LinxListaDaVez>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxListaDaVez> registros);
    }
}
