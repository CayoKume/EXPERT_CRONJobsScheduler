﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxCstPisFiscalRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxCstPisFiscal? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxCstPisFiscal> records);
        public Task<List<LinxCstPisFiscal>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxCstPisFiscal> registros);
    }
}
