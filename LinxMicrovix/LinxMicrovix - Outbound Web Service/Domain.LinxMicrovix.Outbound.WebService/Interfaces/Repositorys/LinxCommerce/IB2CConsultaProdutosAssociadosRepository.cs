﻿using Domain.IntegrationsCore.Entities.Parameters;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaProdutosAssociadosRepository
    {
        public Task<bool> InsertRecord(LinxMicrovixJobParameter jobParameter, B2CConsultaProdutosAssociados? record);
        public Task<bool> InsertParametersIfNotExists(LinxMicrovixJobParameter jobParameter);
        public Task<bool> CreateTableMerge(LinxMicrovixJobParameter jobParameter);
        public bool BulkInsertIntoTableRaw(LinxMicrovixJobParameter jobParameter, IList<B2CConsultaProdutosAssociados> records);
        public Task<List<B2CConsultaProdutosAssociados>> GetRegistersExists(LinxMicrovixJobParameter jobParameter, List<B2CConsultaProdutosAssociados> registros);
    }
}
