﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosDetalhesDepositosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxProdutosDetalhesDepositos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxProdutosDetalhesDepositos> records);
        public Task<List<LinxProdutosDetalhesDepositos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxProdutosDetalhesDepositos> registros);
    }
}
