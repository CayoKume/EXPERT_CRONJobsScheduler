﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxSerialVendaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxSerialVenda? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxSerialVenda> records);
        public Task<List<LinxSerialVenda>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxSerialVenda> registros);
    }
}
