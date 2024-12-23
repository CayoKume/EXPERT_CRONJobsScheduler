﻿using HangfireDashboard.Domain.Entites;
using IntegrationsCore.Domain.Entities;
using LinxMicrovix_Outbound_Web_Service.Application.Services.LinxMicrovix;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HangfireDashboard.UI.Controllers.LinxMicrovix
{
    public class LinxMicrovixIndividualController : Controller
    {
        private readonly string? _docMainCompany;
        private readonly string? _projectName;
        private readonly string? _parametersInterval;
        private readonly string? _parametersTableName;
        private readonly string? _parametersLogTableName;
        private readonly string? _key;
        private readonly string? _authentication;
        private readonly List<Method>? _methods;
        private readonly IConfiguration _configuration;

        private readonly ILinxVendedoresService _linxVendedoresService;
        private readonly ILinxProdutosCodBarService _linxProdutosCodBarService;

        public LinxMicrovixIndividualController(
            IConfiguration configuration,
            ILinxVendedoresService linxVendedoresService,
            ILinxProdutosCodBarService linxProdutosCodBarService
        )
        {
            _configuration = configuration;
            _linxVendedoresService = linxVendedoresService;
            _linxProdutosCodBarService = linxProdutosCodBarService;

            _docMainCompany = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("MainCompany")
                .Value;

            _projectName = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("ProjectName")
                .Value;

            _parametersLogTableName = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("ProjectParametersLogTableName")
                .Value;

            _parametersTableName = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("ProjectParametersTableName")
                .Value;

            _key = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("Key")
                .Value;

            _authentication = _configuration
                .GetSection("LinxMicrovix")
                .GetSection("Authentication")
                .Value;

            _parametersInterval = _configuration
                            .GetSection("LinxMicrovix")
                            .GetSection("ParametersIndividual")
                            .Value;

            _methods = _configuration
                            .GetSection("LinxMicrovix")
                            .GetSection("Methods")
                            .Get<List<Method>>();
        }

        [HttpPost("LinxVendedoresIndividual")]
        public async Task<ActionResult> LinxVendedoresIndividual([Required][FromQuery] string? cod_vendedor, [Required][FromQuery] string? cnpj_emp)
        {
            try
            {
                var method = _methods
                    .Where(m => m.MethodName == "LinxVendedores")
                    .FirstOrDefault();

                var result = await _linxVendedoresService.GetRecord(
                    new LinxMicrovixJobParameter
                    {
                        docMainCompany = _docMainCompany,
                        projectName = _projectName,
                        keyAuthorization = _key,
                        userAuthentication = _authentication,
                        parametersTableName = _parametersTableName,
                        parametersLogTableName = _parametersLogTableName,
                        parametersInterval = _parametersInterval,
                        jobName = method.MethodName,
                        tableName = "_" + method.MethodName
                    },
                    identificador: cod_vendedor,
                    cnpj_emp: cnpj_emp
                );

                if (result != true)
                    return BadRequest($"Unable to find records on endpoint.");
                else
                    return Ok($"Records integrated successfully.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content($"Unable to integrate the records.\nError: {ex.Message}");
            }
        }
    }
}
