﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;
using Application.IntegrationsCore.Interfaces;
using Application.LinxMicrovix.Outbound.WebService.Entities.Cache.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaPlanosService : IB2CConsultaPlanosService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaPlanos> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaPlanosRepository _b2cConsultaPlanosRepository;
        private static IB2CConsultaPlanosServiceCache _b2cConsultaPlanosCache { get; set; } = new B2CConsultaPlanosServiceCache();

        public B2CConsultaPlanosService(
            IAPICall apiCall,
            ILoggerService logger,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaPlanos> linxMicrovixRepositoryBase,
            IB2CConsultaPlanosRepository b2cConsultaPlanosRepository
        )
        {
            _apiCall = apiCall;
            _logger = logger;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _b2cConsultaPlanosRepository = b2cConsultaPlanosRepository;
        }

        public List<B2CConsultaPlanos?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaPlanos>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new B2CConsultaPlanos(
                        plano: records[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).FirstOrDefault(),
                        nome_plano: records[i].Where(pair => pair.Key == "nome_plano").Select(pair => pair.Value).FirstOrDefault(),
                        forma_pagamento: records[i].Where(pair => pair.Key == "forma_pagamento").Select(pair => pair.Value).FirstOrDefault(),
                        qtde_parcelas: records[i].Where(pair => pair.Key == "qtde_parcelas").Select(pair => pair.Value).FirstOrDefault(),
                        valor_minimo_parcela: records[i].Where(pair => pair.Key == "valor_minimo_parcela").Select(pair => pair.Value).FirstOrDefault(),
                        indice: records[i].Where(pair => pair.Key == "indice").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
                        desativado: records[i].Where(pair => pair.Key == "desativado").Select(pair => pair.Value).FirstOrDefault(),
                        tipo_plano: records[i].Where(pair => pair.Key == "tipo_plano").Select(pair => pair.Value).FirstOrDefault(),
                        portal: records[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).FirstOrDefault()
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
                                message: $"Error when convert record - plano: {records[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).FirstOrDefault()} | nome_plano: {records[i].Where(pair => pair.Key == "nome_plano").Select(pair => pair.Value).FirstOrDefault()}\n" +
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
                        message: $"Error when convert record - plano: {records[i].Where(pair => pair.Key == "plano").Select(pair => pair.Value).FirstOrDefault()} | nome_plano: {records[i].Where(pair => pair.Key == "nome_plano").Select(pair => pair.Value).FirstOrDefault()}",
                        exceptionMessage: ex.Message
                    );
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
                    parametersList: parameters.Replace("[0]", "0").Replace("[plano]", identificador),
                    jobParameter: jobParameter,
                    cnpj_emp: cnpj_emp);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    foreach (var record in listRecords)
                    {
                        await _b2cConsultaPlanosRepository.InsertRecord(record: record, jobParameter: jobParameter);
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
            IList<B2CConsultaPlanos> _listSomenteNovos = new List<B2CConsultaPlanos>();

            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.B2CConsultaPlanos);

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

                        if (_b2cConsultaPlanosCache.GetList().Count == 0)
                        {
                            var list_existentes = await _b2cConsultaPlanosRepository.GetRegistersExists(jobParameter: jobParameter, registros: listRecords);
                            _b2cConsultaPlanosCache.AddList(list_existentes);
                        }

                        _listSomenteNovos = _b2cConsultaPlanosCache.FiltrarList(listRecords);
                        if (_listSomenteNovos.Count() > 0)
                        {
                            _b2cConsultaPlanosRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                            for (int i = 0; i < _listSomenteNovos.Count; i++)
                            {
                                var key = _b2cConsultaPlanosCache.GetKey(_listSomenteNovos[i]);
                                if (_b2cConsultaPlanosCache.GetDictionaryXml().ContainsKey(key))
                                {
                                    var xml = _b2cConsultaPlanosCache.GetDictionaryXml()[key];
                                    _logger.AddRecord(key, xml);
                                }
                            }

                            await _linxMicrovixRepositoryBase.CallDbProcMerge(jobParameter: jobParameter);

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

                await _linxMicrovixRepositoryBase.CallDbProcMerge(jobParameter: jobParameter);
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
                _b2cConsultaPlanosCache.AddList(_listSomenteNovos);
            }

            return true;
        }
    }
}
