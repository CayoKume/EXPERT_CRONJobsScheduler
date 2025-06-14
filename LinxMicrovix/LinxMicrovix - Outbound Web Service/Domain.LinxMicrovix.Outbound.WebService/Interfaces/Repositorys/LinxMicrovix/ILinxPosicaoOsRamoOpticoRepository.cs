﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPosicaoOsRamoOpticoRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPosicaoOsRamoOptico? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPosicaoOsRamoOptico> records);
        public Task<List<LinxPosicaoOsRamoOptico>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPosicaoOsRamoOptico> registros);
    }
}
