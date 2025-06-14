﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxOrcamentoComponenteFormulaRepository : ILinxOrcamentoComponenteFormulaRepository
    {
        public LinxOrcamentoComponenteFormulaRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxOrcamentoComponenteFormula> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxOrcamentoComponenteFormula>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxOrcamentoComponenteFormula> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxOrcamentoComponenteFormula? record)
        {
            throw new NotImplementedException();
        }
    }
}
