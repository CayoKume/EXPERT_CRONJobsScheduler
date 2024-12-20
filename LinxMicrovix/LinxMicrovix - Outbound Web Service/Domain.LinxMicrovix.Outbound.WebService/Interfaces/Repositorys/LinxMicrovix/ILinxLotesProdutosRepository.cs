﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxLotesProdutosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxLotesProdutos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxLotesProdutos> records);
        public Task<List<LinxLotesProdutos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxLotesProdutos> registros);
    }
}
