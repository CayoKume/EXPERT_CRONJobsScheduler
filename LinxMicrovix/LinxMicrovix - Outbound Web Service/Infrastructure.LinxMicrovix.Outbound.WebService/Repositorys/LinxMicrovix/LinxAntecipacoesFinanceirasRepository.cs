﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxAntecipacoesFinanceirasRepository : ILinxAntecipacoesFinanceirasRepository
    {
        public LinxAntecipacoesFinanceirasRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxAntecipacoesFinanceiras> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxAntecipacoesFinanceiras>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxAntecipacoesFinanceiras> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxAntecipacoesFinanceiras? record)
        {
            throw new NotImplementedException();
        }
    }
}
