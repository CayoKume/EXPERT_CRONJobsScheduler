﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxNFeEventoRepository : ILinxNFeEventoRepository
    {
        public LinxNFeEventoRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxNFeEvento> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxNFeEvento>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxNFeEvento> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxNFeEvento? record)
        {
            throw new NotImplementedException();
        }
    }
}
