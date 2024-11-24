﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;

using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaLegendasCadastrosAuxiliaresService : IB2CConsultaLegendasCadastrosAuxiliaresService
    {
        private readonly IAPICall _apiCall;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaLegendasCadastrosAuxiliares> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaLegendasCadastrosAuxiliaresRepository _b2cConsultaLegendasCadastrosAuxiliaresRepository;

        public B2CConsultaLegendasCadastrosAuxiliaresService(
            IAPICall apiCall,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaLegendasCadastrosAuxiliares> linxMicrovixRepositoryBase,
            IB2CConsultaLegendasCadastrosAuxiliaresRepository b2cConsultaLegendasCadastrosAuxiliaresRepository
        )
        {
            _apiCall = apiCall;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _b2cConsultaLegendasCadastrosAuxiliaresRepository = b2cConsultaLegendasCadastrosAuxiliaresRepository;
        }

        public List<B2CConsultaLegendasCadastrosAuxiliares?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaLegendasCadastrosAuxiliares>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var entity = new B2CConsultaLegendasCadastrosAuxiliares(
                        empresa: records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_setor: records[i].Where(pair => pair.Key == "legenda_setor").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_linha: records[i].Where(pair => pair.Key == "legenda_linha").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_marca: records[i].Where(pair => pair.Key == "legenda_marca").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_colecao: records[i].Where(pair => pair.Key == "legenda_colecao").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_grade1: records[i].Where(pair => pair.Key == "legenda_grade1").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_grade2: records[i].Where(pair => pair.Key == "legenda_grade2").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_espessura: records[i].Where(pair => pair.Key == "legenda_espessura").Select(pair => pair.Value).FirstOrDefault(),
                        legenda_classificacao: records[i].Where(pair => pair.Key == "legenda_classificacao").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault()
                    );

                    list.Add(entity);
                }
                catch (Exception ex)
                {
                    throw new InternalErrorException()
                    {
                        project = jobParameter.projectName,
                        job = jobParameter.jobName,
                        method = "DeserializeXMLToObject",
                        message = $"Error when convert record: {records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault()} - {records[i].Where(pair => pair.Key == "legenda_setor").Select(pair => pair.Value).FirstOrDefault()}",
                        record = $"{records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault()} - {records[i].Where(pair => pair.Key == "legenda_setor").Select(pair => pair.Value).FirstOrDefault()}",
                        propertie = " - ",
                        exception = ex.Message
                    };
                }
            }

            return list;
        }

        public async Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter)
        {
            try
            {
                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter);
                var cnpjs_emp = await _linxMicrovixRepositoryBase.GetB2CCompanys(jobParameter);

                foreach (var cnpj_emp in cnpjs_emp)
                {
                    var body = _linxMicrovixServiceBase.BuildBodyRequest(
                                parametersList: parameters.Replace("[0]", "0"),
                                jobParameter: jobParameter,
                                cnpj_emp: cnpj_emp.doc_company
                            );

                    string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                    var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                    if (xmls.Count() > 0)
                    {
                        var listRecords = DeserializeXMLToObject(jobParameter, xmls);
                        _b2cConsultaLegendasCadastrosAuxiliaresRepository.BulkInsertIntoTableRaw(records: listRecords, jobParameter: jobParameter);
                    }
                }

                await _linxMicrovixRepositoryBase.CallDbProcMerge(jobParameter: jobParameter);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
