﻿using Application.IntegrationsCore.Interfaces;
using Application.IntegrationsCore.Services;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.LinxMicrovix.Outbound.WebService.Entities.Cache.LinxCommerce
{
    public class B2CConsultaClientesSaldoLinxServiceCache : CacheService<B2CConsultaClientesSaldoLinx>, ICacheService<B2CConsultaClientesSaldoLinx>, IB2CConsultaClientesSaldoLinxServiceCache, ICacheBase
    {
        public override string GetKey(B2CConsultaClientesSaldoLinx entity)
        {
            throw new NotImplementedException();
        }

        public override string GetKeyInDictionary(IDictionary<string, string> dictionaryFields)
        {
            throw new NotImplementedException();
        }
    }
}