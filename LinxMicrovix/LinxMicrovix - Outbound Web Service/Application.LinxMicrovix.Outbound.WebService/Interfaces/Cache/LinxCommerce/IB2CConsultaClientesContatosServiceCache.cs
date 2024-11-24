﻿using Application.IntegrationsCore.Interfaces;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce
{
    public interface IB2CConsultaClientesContatosServiceCache : ICacheService<B2CConsultaClientesContatos>
    {
        
    }
}