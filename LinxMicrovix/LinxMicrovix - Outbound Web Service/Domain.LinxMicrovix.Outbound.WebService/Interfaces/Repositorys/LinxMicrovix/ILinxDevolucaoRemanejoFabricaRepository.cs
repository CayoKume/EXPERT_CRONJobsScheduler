﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxDevolucaoRemanejoFabricaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxDevolucaoRemanejoFabrica? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxDevolucaoRemanejoFabrica> records);
        public Task<List<LinxDevolucaoRemanejoFabrica>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxDevolucaoRemanejoFabrica> registros);
    }
}
