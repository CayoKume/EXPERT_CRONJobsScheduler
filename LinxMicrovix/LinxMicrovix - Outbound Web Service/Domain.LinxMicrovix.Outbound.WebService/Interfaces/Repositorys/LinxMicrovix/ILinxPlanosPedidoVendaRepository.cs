﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPlanosPedidoVendaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPlanosPedidoVenda? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPlanosPedidoVenda> records);
        public Task<List<LinxPlanosPedidoVenda>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPlanosPedidoVenda> registros);
    }
}
