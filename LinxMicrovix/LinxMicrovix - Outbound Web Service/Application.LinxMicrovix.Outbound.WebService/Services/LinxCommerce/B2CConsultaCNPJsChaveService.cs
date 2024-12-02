﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Application.LinxMicrovix.Outbound.WebService.Entities.Cache.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce;
using Application.IntegrationsCore.Interfaces;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    public class B2CConsultaCNPJsChaveService : IB2CConsultaCNPJsChaveService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaCNPJsChave> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaCNPJsChaveRepository _b2cConsultaCNPJsChaveRepository;
        private static IB2CConsultaCNPJsChaveServiceCache _b2cConsultaCNPJsChaveServiceCache { get; set; } = new B2CConsultaCNPJsChaveServiceCache();

        public B2CConsultaCNPJsChaveService(
            IAPICall apiCall,
            ILoggerService logger,
            IB2CConsultaCNPJsChaveRepository b2cConsultaCNPJsChaveRepository,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaCNPJsChave> linxMicrovixRepositoryBase
        )
        {
            _apiCall = apiCall;
            _logger = logger;
            _b2cConsultaCNPJsChaveRepository = b2cConsultaCNPJsChaveRepository;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
        }

        public List<B2CConsultaCNPJsChave?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaCNPJsChave>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new B2CConsultaCNPJsChave(
                        listValidations: validations,
                        cnpj: records[i].Where(pair => pair.Key == "cnpj").Select(pair => pair.Value).FirstOrDefault(),
                        nome_empresa: records[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).FirstOrDefault(),
                        id_empresas_rede: records[i].Where(pair => pair.Key == "id_empresas_rede").Select(pair => pair.Value).FirstOrDefault(),
                        rede: records[i].Where(pair => pair.Key == "rede").Select(pair => pair.Value).FirstOrDefault(),
                        portal: records[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).FirstOrDefault(),
                        nome_portal: records[i].Where(pair => pair.Key == "nome_portal").Select(pair => pair.Value).FirstOrDefault(),
                        empresa: records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault(),
                        classificacao_portal: records[i].Where(pair => pair.Key == "classificacao_portal").Select(pair => pair.Value).FirstOrDefault(),
                        b2c: records[i].Where(pair => pair.Key == "b2c").Select(pair => pair.Value).FirstOrDefault(),
                        oms: records[i].Where(pair => pair.Key == "oms").Select(pair => pair.Value).FirstOrDefault()
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
                                message: $"Error when convert record - cnpj: {records[i].Where(pair => pair.Key == "cnpj").Select(pair => pair.Value).FirstOrDefault()} | nome_empresa: {records[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).FirstOrDefault()}\n" +
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
                        message: $"Error when convert record - cnpj: {records[i].Where(pair => pair.Key == "cnpj").Select(pair => pair.Value).FirstOrDefault()} | nome_empresa: {records[i].Where(pair => pair.Key == "nome_empresa").Select(pair => pair.Value).FirstOrDefault()}",
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
                    parametersList: parameters.Replace("[id_classificacao]", identificador),
                    jobParameter: jobParameter,
                    cnpj_emp: cnpj_emp);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    foreach (var record in listRecords)
                    {
                        await _b2cConsultaCNPJsChaveRepository.InsertRecord(record: record, jobParameter: jobParameter);
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
            IList<B2CConsultaCNPJsChave> _listSomenteNovos = new List<B2CConsultaCNPJsChave>();

            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.B2CConsultaCNPJsChave);

                var body = _linxMicrovixServiceBase.BuildBodyRequest(jobParameter: jobParameter);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    if (_b2cConsultaCNPJsChaveServiceCache.GetList().Count == 0)
                    {
                        var list_existentes = await _b2cConsultaCNPJsChaveRepository.GetRegistersExists(jobParameter: jobParameter, registros: listRecords);
                        _b2cConsultaCNPJsChaveServiceCache.AddList(list_existentes);
                    }

                    _listSomenteNovos = _b2cConsultaCNPJsChaveServiceCache.FiltrarList(listRecords);
                    if (_listSomenteNovos.Count() > 0)
                    {
                        _b2cConsultaCNPJsChaveRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                        for (int i = 0; i < _listSomenteNovos.Count; i++)
                        {
                            var key = _b2cConsultaCNPJsChaveServiceCache.GetKey(_listSomenteNovos[i]);
                            if (_b2cConsultaCNPJsChaveServiceCache.GetDictionaryXml().ContainsKey(key))
                            {
                                var xml = _b2cConsultaCNPJsChaveServiceCache.GetDictionaryXml()[key];
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
                _b2cConsultaCNPJsChaveServiceCache.AddList(_listSomenteNovos);
            }

            return true;
        }
    }
}
