﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPedidosVendaChecklistEntregaArmazenamentoRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPedidosVendaChecklistEntregaArmazenamento? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPedidosVendaChecklistEntregaArmazenamento> records);
        public Task<List<LinxPedidosVendaChecklistEntregaArmazenamento>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPedidosVendaChecklistEntregaArmazenamento> registros);
    }
}
