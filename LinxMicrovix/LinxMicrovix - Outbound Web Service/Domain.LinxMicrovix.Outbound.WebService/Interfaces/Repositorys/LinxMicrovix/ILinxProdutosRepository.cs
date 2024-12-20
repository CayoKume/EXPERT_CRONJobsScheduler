﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxProdutos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxProdutos> records);
        public Task<List<LinxProdutos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxProdutos> registros);
    }
}
