﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxNcmRepository : ILinxNcmRepository
    {
        public LinxNcmRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxNcm> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxNcm>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxNcm> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxNcm? record)
        {
            throw new NotImplementedException();
        }
    }
}
