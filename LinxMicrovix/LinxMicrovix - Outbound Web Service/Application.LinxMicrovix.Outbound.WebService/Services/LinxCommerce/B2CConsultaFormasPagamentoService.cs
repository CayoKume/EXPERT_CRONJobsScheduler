﻿using Application.IntegrationsCore.Interfaces;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using System.ComponentModel.DataAnnotations;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaFormasPagamentoService : IB2CConsultaFormasPagamentoService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixAzureSQLRepositoryBase<B2CConsultaFormasPagamento> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaFormasPagamentoRepository _b2cConsultaFormasPagamentoRepository;
        private static List<string?> _b2cConsultaFormasPagamentoCache { get; set; } = new List<string?>();

        public B2CConsultaFormasPagamentoService(
            IAPICall apiCall,
            ILoggerService logger,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixAzureSQLRepositoryBase<B2CConsultaFormasPagamento> linxMicrovixRepositoryBase,
            IB2CConsultaFormasPagamentoRepository b2cConsultaFormasPagamentoRepository
        )
        {
            _apiCall = apiCall;
            _logger = logger;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _b2cConsultaFormasPagamentoRepository = b2cConsultaFormasPagamentoRepository;
        }

        public List<B2CConsultaFormasPagamento?> DeserializeXMLToObject(LinxAPIParam jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaFormasPagamento>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new B2CConsultaFormasPagamento(
                        listValidations: validations,
                        cod_forma_pgto: records[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).FirstOrDefault(),
                        forma_pgto: records[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
                        portal: records[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).FirstOrDefault(),
                        recordXml: records[i].Where(pair => pair.Key == "recordXml").Select(pair => pair.Value).FirstOrDefault()
                    );

                    var contexto = new ValidationContext(entity, null, null);
                    Validator.TryValidateObject(entity, contexto, validations, true);

                    if (validations.Count() > 0)
                    {
                        for (int j = 0; j < validations.Count(); j++)
                        {
                            _logger.AddMessage(
                                stage: EnumStages.DeserializeXMLToObject,
                                error: EnumError.Validation,
                                logLevel: EnumMessageLevel.Warning,
                                message: $"Error when convert record - cod_forma_pgto: {records[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).FirstOrDefault()} | forma_pgto: {records[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).FirstOrDefault()}\n" +
                                         $"{validations[j].ErrorMessage}"
                            );
                        }
                        continue;
                    }

                    list.Add(entity);
                }
                catch (Exception ex)
                {
                    throw new InternalException(
                        stage: EnumStages.DeserializeXMLToObject,
                        error: EnumError.Exception,
                        level: EnumMessageLevel.Error,
                        message: $"Error when convert record - cod_forma_pgto: {records[i].Where(pair => pair.Key == "cod_forma_pgto").Select(pair => pair.Value).FirstOrDefault()} | forma_pgto: {records[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).FirstOrDefault()}",
                        exceptionMessage: ex.Message
                    );
                }
            }

            return list;
        }

        public async Task<bool> GetRecords(LinxAPIParam jobParameter)
        {
            IList<B2CConsultaFormasPagamento> _listSomenteNovos = new List<B2CConsultaFormasPagamento>();

            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.B2CConsultaFormasPagamento);

                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter.parametersInterval, jobParameter.tableName, jobParameter.jobName);
                var cnpjs_emp = await _linxMicrovixRepositoryBase.GetB2CCompanys();

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

                        //if (_b2cConsultaFormasPagamentoCache.GetList().Count == 0)
                        //{
                        //    var list_existentes = await _b2cConsultaFormasPagamentoRepository.GetRegistersExists(jobParameter: jobParameter, registros: listRecords);
                        //    _b2cConsultaFormasPagamentoCache.AddList(list_existentes);
                        //}

                        //_listSomenteNovos = _b2cConsultaFormasPagamentoCache.FiltrarList(listRecords);

                        if (_listSomenteNovos.Count() > 0)
                        {
                            _b2cConsultaFormasPagamentoRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                            
                            //for (int i = 0; i < _listSomenteNovos.Count; i++)
                            //{
                            //    var key = _b2cConsultaFormasPagamentoCache.GetKey(_listSomenteNovos[i]);
                            //    if (_b2cConsultaFormasPagamentoCache.GetDictionaryXml().ContainsKey(key))
                            //    {
                            //        var xml = _b2cConsultaFormasPagamentoCache.GetDictionaryXml()[key];
                            //        _logger.AddRecord(key, xml);
                            //    }
                            //}

                            await _linxMicrovixRepositoryBase.CallDbProcMerge(jobParameter.schema, jobParameter.tableName, _logger.GetExecutionGuid());

                            _logger.AddMessage(
                                $"Concluída com sucesso: {_listSomenteNovos.Count} registro(s) novo(s) inserido(s)!"
                            );
                        }
                        else
                            _logger.AddMessage(
                                $"Concluída com sucesso: {_listSomenteNovos.Count} registro(s) novo(s) inserido(s)!"
                            );
                    }
                }
            }
            catch (SQLCommandException ex)
            {
                _logger.AddMessage(
                    stage: ex.Stage,
                    error: ex.Error,
                    logLevel: ex.MessageLevel,
                    message: ex.Message,
                    exceptionMessage: ex.ExceptionMessage,
                    commandSQL: ex.CommandSQL
                );

                throw;
            }
            catch (InternalException ex)
            {
                _logger.AddMessage(
                    stage: ex.stage,
                    error: ex.Error,
                    logLevel: ex.MessageLevel,
                    message: ex.Message,
                    exceptionMessage: ex.ExceptionMessage
                );

                throw;
            }
            catch (Exception ex)
            {
                _logger.AddMessage(
                    message: "Error when executing GetRecords method",
                    exceptionMessage: ex.Message
                );
            }
            finally
            {
                //await _logger.CommitAllChanges();
            }

            return true;
        }
    }
}
