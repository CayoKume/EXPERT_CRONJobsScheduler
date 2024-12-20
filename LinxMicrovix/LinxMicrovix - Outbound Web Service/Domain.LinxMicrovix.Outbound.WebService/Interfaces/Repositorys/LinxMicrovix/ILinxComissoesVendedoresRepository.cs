﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxComissoesVendedoresRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxComissoesVendedores? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxComissoesVendedores> records);
        public Task<List<LinxComissoesVendedores>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxComissoesVendedores> registros);
    }
}
