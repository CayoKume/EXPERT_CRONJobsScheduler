﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxConfiguracoesTributariasRepository : ILinxConfiguracoesTributariasRepository
    {
        public LinxConfiguracoesTributariasRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxConfiguracoesTributarias> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxConfiguracoesTributarias>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxConfiguracoesTributarias> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxConfiguracoesTributarias? record)
        {
            throw new NotImplementedException();
        }
    }
}
