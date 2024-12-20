﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxConfiguracoesTributariasDetalhesRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxConfiguracoesTributariasDetalhes? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxConfiguracoesTributariasDetalhes> records);
        public Task<List<LinxConfiguracoesTributariasDetalhes>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxConfiguracoesTributariasDetalhes> registros);
    }
}
