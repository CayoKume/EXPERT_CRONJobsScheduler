﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix
{
    public interface ILinxPerguntaVendaRepository
    {
        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxPerguntaVenda? record);
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxPerguntaVenda> records);
        public Task<List<LinxPerguntaVenda>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxPerguntaVenda> registros);
    }
}
