﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxCestRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxCest? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxCest> records);
        public Task<List<LinxCest>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxCest> registros);
    }
}
