﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxMetasVendedoresRepository : ILinxMetasVendedoresRepository
    {
        public LinxMetasVendedoresRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMetasVendedores> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxMetasVendedores>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMetasVendedores> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMetasVendedores? record)
        {
            throw new NotImplementedException();
        }
    }
}
