﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxSubClassesRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxSubClasses? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxSubClasses> records);
        public Task<List<LinxSubClasses>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxSubClasses> registros);
    }
}
