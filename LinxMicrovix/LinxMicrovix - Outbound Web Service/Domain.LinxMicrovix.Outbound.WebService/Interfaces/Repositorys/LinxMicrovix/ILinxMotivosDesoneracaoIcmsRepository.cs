﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxMotivosDesoneracaoIcmsRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMotivosDesoneracaoIcms? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMotivosDesoneracaoIcms> records);
        public Task<List<LinxMotivosDesoneracaoIcms>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMotivosDesoneracaoIcms> registros);
    }
}
