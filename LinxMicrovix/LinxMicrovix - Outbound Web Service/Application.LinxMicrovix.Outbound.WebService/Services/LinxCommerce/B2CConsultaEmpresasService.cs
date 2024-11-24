﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;

using System.Globalization;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaEmpresasService : IB2CConsultaEmpresasService
    {
        private readonly IAPICall _apiCall;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaEmpresas> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaEmpresasRepository _b2cConsultaEmpresasRepository;

        public B2CConsultaEmpresasService(
            IAPICall apiCall,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaEmpresas> linxMicrovixRepositoryBase,
            IB2CConsultaEmpresasRepository b2cConsultaEmpresasRepository
        )
        {
            _apiCall = apiCall;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _b2cConsultaEmpresasRepository = b2cConsultaEmpresasRepository;
        }

        public List<B2CConsultaEmpresas?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaEmpresas>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var entity = new B2CConsultaEmpresas(
                        empresa: records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault(),
                        nome_emp: records[i].Where(pair => pair.Key == "nome_emp").Select(pair => pair.Value).FirstOrDefault(),
                        cnpj_emp: records[i].Where(pair => pair.Key == "cnpjEmp").Select(pair => pair.Value).FirstOrDefault(),
                        end_unidade: records[i].Where(pair => pair.Key == "end_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        complemento_end_unidade: records[i].Where(pair => pair.Key == "complemento_end_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        nr_rua_unidade: records[i].Where(pair => pair.Key == "nr_rua_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        bairro_unidade: records[i].Where(pair => pair.Key == "bairro_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        cep_unidade: records[i].Where(pair => pair.Key == "cep_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        cidade_unidade: records[i].Where(pair => pair.Key == "cidade_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        uf_unidade: records[i].Where(pair => pair.Key == "uf_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        email_unidade: records[i].Where(pair => pair.Key == "email_unidade").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
                        data_criacao: records[i].Where(pair => pair.Key == "data_criacao").Select(pair => pair.Value).FirstOrDefault(),
                        centro_distribuicao: records[i].Where(pair => pair.Key == "centro_distribuicao").Select(pair => pair.Value).FirstOrDefault(),
                        portal: records[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).FirstOrDefault()
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
                        message = $"Error when convert record: {records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault()} - {records[i].Where(pair => pair.Key == "nome_emp").Select(pair => pair.Value).FirstOrDefault()}",
                        record = $"{records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault()} - {records[i].Where(pair => pair.Key == "nome_emp").Select(pair => pair.Value).FirstOrDefault()}",
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

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                    parametersList: parameters.Replace("[0]", "0"),
                    jobParameter: jobParameter,
                    cnpj_emp: jobParameter.docMainCompany
                );

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);
                    _b2cConsultaEmpresasRepository.BulkInsertIntoTableRaw(records: listRecords, jobParameter: jobParameter);
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
