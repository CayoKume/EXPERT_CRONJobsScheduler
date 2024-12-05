﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;
using Domain.IntegrationsCore.Exceptions;
using Application.IntegrationsCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.Entities.Enums;
using Application.LinxMicrovix.Outbound.WebService.Entities.Cache.LinxCommerce;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Cache.LinxCommerce;

namespace Application.LinxMicrovix.Outbound.WebService.Services
{
    /// <summary>
    /// A tabela classificacao geralmente não é grande então efetuamos a busca completa no endpoint indicando o timestamp 0.
    /// E como ela possui dados gerais não é preciso pesquisar por todos os cnpjs, ao pesquisar pelo cnpj da matriz os dados serão os 
    /// mesmos para os demais cnpjs
    /// </summary>
    public class B2CConsultaClassificacaoService : IB2CConsultaClassificacaoService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaClassificacao> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaClassificacaoRepository _b2cConsultaClassificacaoRepository;
        private static IB2CConsultaClassificacaoServiceCache _b2cConsultaClassificacaoCache { get; set; } = new B2CConsultaClassificacaoServiceCache();

        public B2CConsultaClassificacaoService(
            IAPICall apiCall,
            ILoggerService logger,
            IB2CConsultaClassificacaoRepository b2cConsultaClassificacaoRepository, 
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaClassificacao> linxMicrovixRepositoryBase
        ) 
        {
            _apiCall = apiCall;
            _logger = logger;
            _b2cConsultaClassificacaoRepository = b2cConsultaClassificacaoRepository;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
        }

        public async Task<bool> GetRecords(LinxMicrovixJobParameter jobParameter)
        {
            IList<B2CConsultaClassificacao> _listSomenteNovos = new List<B2CConsultaClassificacao>();

            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.B2CConsultaClassificacao);

                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter);

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                    parametersList: parameters.Replace("[0]", "0"),
                    jobParameter: jobParameter,
                    cnpj_emp: jobParameter.docDocMainCompany
                );

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    if (_b2cConsultaClassificacaoCache.GetList().Count == 0)
                    {
                        var list_existentes = await _b2cConsultaClassificacaoRepository.GetRegistersExists(jobParameter: jobParameter, registros: listRecords);
                        _b2cConsultaClassificacaoCache.AddList(list_existentes);
                    }

                    _listSomenteNovos = _b2cConsultaClassificacaoCache.FiltrarList(listRecords);
                    if (_listSomenteNovos.Count() > 0)
                    {
                        _b2cConsultaClassificacaoRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                        for (int i = 0; i < _listSomenteNovos.Count; i++)
                        {
                            var key = _b2cConsultaClassificacaoCache.GetKey(_listSomenteNovos[i]);
                            if (_b2cConsultaClassificacaoCache.GetDictionaryXml().ContainsKey(key))
                            {
                                var xml = _b2cConsultaClassificacaoCache.GetDictionaryXml()[key];
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
                _b2cConsultaClassificacaoCache.AddList(_listSomenteNovos);
            }

            return true;
        }

        public async Task<bool> GetRecord(LinxMicrovixJobParameter jobParameter, string? identificador, string? cnpj_emp)
        {
            try
            {
                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter);

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                    parametersList: parameters.Replace("[0]", "0").Replace("[codigo_classificacao]", identificador),
                    jobParameter: jobParameter,
                    cnpj_emp: cnpj_emp);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);  
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    foreach (var record in listRecords)
                    {
                        await _b2cConsultaClassificacaoRepository.InsertRecord(record: record, jobParameter: jobParameter);
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

        public List<B2CConsultaClassificacao?> DeserializeXMLToObject(LinxMicrovixJobParameter jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaClassificacao>();
            for(int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new B2CConsultaClassificacao(
                        listValidations: validations,
                        codigo_classificacao: records[i].Where(pair => pair.Key == "codigo_classificacao").Select(pair => pair.Value).FirstOrDefault(),
                        nome_classificacao: records[i].Where(pair => pair.Key == "nome_classificacao").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
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
                                message: $"Error when convert record - codigo_classificacao: {records[i].Where(pair => pair.Key == "codigo_classificacao").Select(pair => pair.Value).FirstOrDefault()} | nome_classificacao: {records[i].Where(pair => pair.Key == "nome_classificacao").Select(pair => pair.Value).FirstOrDefault()}\n" +
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
                        message: $"Error when convert record - codigo_classificacao: {records[i].Where(pair => pair.Key == "codigo_classificacao").Select(pair => pair.Value).FirstOrDefault()} | nome_classificacao: {records[i].Where(pair => pair.Key == "nome_classificacao").Select(pair => pair.Value).FirstOrDefault()}",
                        exceptionMessage: ex.Message
                    );
                }
            };

            return list;
        }
    }
}
