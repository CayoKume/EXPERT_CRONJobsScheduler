﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxProdutosOpticosTipoAroRepository : ILinxProdutosOpticosTipoAroRepository
    {
        public LinxProdutosOpticosTipoAroRepository()
        {

        }

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxProdutosOpticosTipoAro> records)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxProdutosOpticosTipoAro>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxProdutosOpticosTipoAro> registros)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxProdutosOpticosTipoAro? record)
        {
            throw new NotImplementedException();
        }
    }
}
