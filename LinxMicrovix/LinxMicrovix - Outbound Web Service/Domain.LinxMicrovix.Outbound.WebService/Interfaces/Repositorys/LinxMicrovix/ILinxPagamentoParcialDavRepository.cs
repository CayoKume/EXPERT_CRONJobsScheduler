﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPagamentoParcialDavRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPagamentoParcialDav? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPagamentoParcialDav> records);
        public Task<List<LinxPagamentoParcialDav>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPagamentoParcialDav> registros);
    }
}
