﻿using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce
{
    public interface IB2CConsultaLegendasCadastrosAuxiliaresRepository
    {
        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaLegendasCadastrosAuxiliares> records);
        public Task<List<B2CConsultaLegendasCadastrosAuxiliares>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaLegendasCadastrosAuxiliares> registros);

    }
}
