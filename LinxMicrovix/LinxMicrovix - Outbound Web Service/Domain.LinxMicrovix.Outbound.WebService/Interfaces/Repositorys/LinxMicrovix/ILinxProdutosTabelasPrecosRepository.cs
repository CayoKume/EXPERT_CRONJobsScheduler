﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosTabelasPrecosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxProdutosTabelasPrecos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxProdutosTabelasPrecos> records);
        public Task<List<LinxProdutosTabelasPrecos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxProdutosTabelasPrecos> registros);
    }
}
