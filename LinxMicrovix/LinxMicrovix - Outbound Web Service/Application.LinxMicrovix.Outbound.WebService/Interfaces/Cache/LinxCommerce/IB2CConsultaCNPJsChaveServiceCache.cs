﻿using Application.IntegrationsCore.Interfaces;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce
{
    public interface IB2CConsultaCNPJsChaveServiceCache : ICacheService<B2CConsultaCNPJsChave>
    {
        
    }
}