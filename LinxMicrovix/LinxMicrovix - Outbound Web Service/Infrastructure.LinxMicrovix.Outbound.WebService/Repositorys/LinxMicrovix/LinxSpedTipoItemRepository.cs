﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxSpedTipoItemRepository : ILinxSpedTipoItemRepository
    {
        public LinxSpedTipoItemRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxSpedTipoItem> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxSpedTipoItem>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxSpedTipoItem> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxSpedTipoItem? record)
        {
            throw new NotImplementedException();
        }
    }
}
