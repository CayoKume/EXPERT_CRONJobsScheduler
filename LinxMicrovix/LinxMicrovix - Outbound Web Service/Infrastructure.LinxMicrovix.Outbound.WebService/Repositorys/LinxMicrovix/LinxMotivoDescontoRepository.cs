﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxMotivoDescontoRepository : ILinxMotivoDescontoRepository
    {
        public LinxMotivoDescontoRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMotivoDesconto> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxMotivoDesconto>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMotivoDesconto> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMotivoDesconto? record)
        {
            throw new NotImplementedException();
        }
    }
}
