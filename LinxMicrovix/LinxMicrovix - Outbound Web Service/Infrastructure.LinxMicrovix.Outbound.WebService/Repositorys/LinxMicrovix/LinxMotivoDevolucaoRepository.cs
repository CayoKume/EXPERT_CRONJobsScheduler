﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxMotivoDevolucaoRepository : ILinxMotivoDevolucaoRepository
    {
        public LinxMotivoDevolucaoRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMotivoDevolucao> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxMotivoDevolucao>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMotivoDevolucao> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMotivoDevolucao? record)
        {
            throw new NotImplementedException();
        }
    }
}
