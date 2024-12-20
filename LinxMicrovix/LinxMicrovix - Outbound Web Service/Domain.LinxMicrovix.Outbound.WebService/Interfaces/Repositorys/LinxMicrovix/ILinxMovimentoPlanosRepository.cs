﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxMovimentoPlanosRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMovimentoPlanos? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMovimentoPlanos> records);
        public Task<List<LinxMovimentoPlanos>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMovimentoPlanos> registros);
    }
}
