﻿using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;

using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaClientesSaldoLinxService : IB2CConsultaClientesSaldoLinxService
    {
        private readonly IAPICall _apiCall;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaClientesSaldoLinx> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaClientesSaldoLinxRepository _b2cConsultaClientesSaldoLinxRepository;

        public B2CConsultaClientesSaldoLinxService(
            IAPICall apiCall,
            IB2CConsultaClientesSaldoLinxRepository b2cConsultaClientesSaldoLinxRepository,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaClientesSaldoLinx> linxMicrovixRepositoryBase
        )
        {
            _apiCall = apiCall;
            _b2cConsultaClientesSaldoLinxRepository = b2cConsultaClientesSaldoLinxRepository;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
        }

        public List<B2CConsultaClientesSaldoLinx?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaClientesSaldoLinx>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var entity = new B2CConsultaClientesSaldoLinx(
                        cod_cliente_erp: records[i].Where(pair => pair.Key == "cod_cliente_erp").Select(pair => pair.Value).FirstOrDefault(),
                        cod_cliente_b2c: records[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).FirstOrDefault(),
                        empresa: records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault(),
                        valor: records[i].Where(pair => pair.Key == "valor").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
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
                        message = $"Error when convert record: Saldo - {records[i].Where(pair => pair.Key == "saldo").Select(pair => pair.Value).FirstOrDefault()} Cliente - {records[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).FirstOrDefault()}",
                        record = $"Saldo - {records[i].Where(pair => pair.Key == "saldo").Select(pair => pair.Value).FirstOrDefault()} Cliente - {records[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).FirstOrDefault()}",
                        propertie = " - ",
                        exception = ex.Message
                    };
                }
            }

            return list;
        }

        public async Task<bool> GetRecord(LinxMicrovixJobParameter jobParameter, string? identificador, string? cnpj_emp)
        {
            try
            {
                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter);

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                    parametersList: parameters.Replace("[0]", "0").Replace("[cod_cliente_erp]", identificador),
                    jobParameter: jobParameter,
                    cnpj_emp: cnpj_emp);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    foreach (var record in listRecords)
                    {
                        await _b2cConsultaClientesSaldoLinxRepository.InsertRecord(record: record, jobParameter: jobParameter);
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
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
                        _b2cConsultaClientesSaldoLinxRepository.BulkInsertIntoTableRaw(records: listRecords, jobParameter: jobParameter);
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
