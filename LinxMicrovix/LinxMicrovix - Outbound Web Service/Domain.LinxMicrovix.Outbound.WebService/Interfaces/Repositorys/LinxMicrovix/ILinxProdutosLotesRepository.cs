﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosLotesRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxProdutosLotes? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxProdutosLotes> records);
        public Task<List<LinxProdutosLotes>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxProdutosLotes> registros);
    }
}
