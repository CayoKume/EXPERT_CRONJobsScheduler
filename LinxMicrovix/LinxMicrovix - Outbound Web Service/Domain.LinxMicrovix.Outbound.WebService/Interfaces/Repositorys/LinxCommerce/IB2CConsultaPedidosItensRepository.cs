﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaPedidosItensRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaPedidosItens? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaPedidosItens> records);
        public Task<List<B2CConsultaPedidosItens>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaPedidosItens> registros);
    }
}
