﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxRespostaVendaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxRespostaVenda? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxRespostaVenda> records);
        public Task<List<LinxRespostaVenda>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxRespostaVenda> registros);
    }
}
