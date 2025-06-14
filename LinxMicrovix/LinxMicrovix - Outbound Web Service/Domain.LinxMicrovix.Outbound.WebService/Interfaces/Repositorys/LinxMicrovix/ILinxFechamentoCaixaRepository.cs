﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxFechamentoCaixaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxFechamentoCaixa? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxFechamentoCaixa> records);
        public Task<List<LinxFechamentoCaixa>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxFechamentoCaixa> registros);
    }
}
