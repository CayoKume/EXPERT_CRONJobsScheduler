﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxMovimentoCamposAdicionaisRepository : ILinxMovimentoCamposAdicionaisRepository
    {
        public LinxMovimentoCamposAdicionaisRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMovimentoCamposAdicionais> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxMovimentoCamposAdicionais>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMovimentoCamposAdicionais> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMovimentoCamposAdicionais? record)
        {
            throw new NotImplementedException();
        }
    }
}
