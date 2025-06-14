﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxMovimentoIndicacoesRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMovimentoIndicacoes? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMovimentoIndicacoes> records);
        public Task<List<LinxMovimentoIndicacoes>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMovimentoIndicacoes> registros);
    }
}
