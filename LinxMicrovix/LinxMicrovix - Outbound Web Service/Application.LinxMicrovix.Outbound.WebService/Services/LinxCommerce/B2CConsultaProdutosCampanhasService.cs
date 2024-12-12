﻿using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
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
    public class B2CConsultaProdutosCampanhasService : IB2CConsultaProdutosCampanhasService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaProdutosCampanhas> _linxMicrovixRepositoryBase;
        private readonly IB2CConsultaProdutosCampanhasRepository _b2cConsultaProdutosCampanhasRepository;
        private static IB2CConsultaProdutosCampanhasServiceCache _b2cConsultaProdutosCampanhasCache { get; set; } = new B2CConsultaProdutosCampanhasServiceCache();

        public B2CConsultaProdutosCampanhasService(
            IAPICall apiCall,
            ILoggerService logger,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixRepositoryBase<B2CConsultaProdutosCampanhas> linxMicrovixRepositoryBase,
            IB2CConsultaProdutosCampanhasRepository b2cConsultaProdutosCampanhasRepository
        )
        {
            _apiCall = apiCall;
            _logger = logger;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _b2cConsultaProdutosCampanhasRepository = b2cConsultaProdutosCampanhasRepository;
        }

        public List<B2CConsultaProdutosCampanhas?> DeserializeXMLToObject(LinxAPIParam jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<B2CConsultaProdutosCampanhas>();

            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new B2CConsultaProdutosCampanhas(
                        listValidations: validations,
                        codigo_campanha: records[i].Where(pair => pair.Key == "codigo_campanha").Select(pair => pair.Value).FirstOrDefault(),
                        nome_campanha: records[i].Where(pair => pair.Key == "nome_campanha").Select(pair => pair.Value).FirstOrDefault(),
                        timestamp: records[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).FirstOrDefault(),
                        vigencia_inicio: records[i].Where(pair => pair.Key == "vigencia_inicio").Select(pair => pair.Value).FirstOrDefault(),
                        vigencia_fim: records[i].Where(pair => pair.Key == "vigencia_fim").Select(pair => pair.Value).FirstOrDefault(),
                        ativo: records[i].Where(pair => pair.Key == "ativo").Select(pair => pair.Value).FirstOrDefault(),
                        observacao: records[i].Where(pair => pair.Key == "observacao").Select(pair => pair.Value).FirstOrDefault(),
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
                                message: $"Error when convert record - codigo_campanha: {records[i].Where(pair => pair.Key == "codigo_campanha").Select(pair => pair.Value).FirstOrDefault()} | nome_campanha: {records[i].Where(pair => pair.Key == "nome_campanha").Select(pair => pair.Value).FirstOrDefault()}\n" +
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
                        message: $"Error when convert record - codigo_campanha: {records[i].Where(pair => pair.Key == "codigo_campanha").Select(pair => pair.Value).FirstOrDefault()} | nome_campanha: {records[i].Where(pair => pair.Key == "nome_campanha").Select(pair => pair.Value).FirstOrDefault()}",
                        exceptionMessage: ex.Message
                    );
                }
            }

            return list;
        }

        public async Task<bool> GetRecord(LinxAPIParam jobParameter, string? identificador, string? cnpj_emp)
        {
            try
            {
                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter);

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                    parametersList: parameters.Replace("[0]", "0").Replace("[codigo_campanha]", identificador),
                    jobParameter: jobParameter,
                    cnpj_emp: cnpj_emp);

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    foreach (var record in listRecords)
                    {
                        await _b2cConsultaProdutosCampanhasRepository.InsertRecord(record: record, jobParameter: jobParameter);
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

        public async Task<bool> GetRecords(LinxAPIParam jobParameter)
        {
            IList<B2CConsultaProdutosCampanhas> _listSomenteNovos = new List<B2CConsultaProdutosCampanhas>();

            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.B2CConsultaProdutosCampanhas);

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

                        if (_b2cConsultaProdutosCampanhasCache.GetList().Count == 0)
                        {
                            var list_existentes = await _b2cConsultaProdutosCampanhasRepository.GetRegistersExists(jobParameter: jobParameter, registros: listRecords);
                            _b2cConsultaProdutosCampanhasCache.AddList(list_existentes);
                        }

                        _listSomenteNovos = _b2cConsultaProdutosCampanhasCache.FiltrarList(listRecords);
                        if (_listSomenteNovos.Count() > 0)
                        {
                            _b2cConsultaProdutosCampanhasRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                            for (int i = 0; i < _listSomenteNovos.Count; i++)
                            {
                                var key = _b2cConsultaProdutosCampanhasCache.GetKey(_listSomenteNovos[i]);
                                if (_b2cConsultaProdutosCampanhasCache.GetDictionaryXml().ContainsKey(key))
                                {
                                    var xml = _b2cConsultaProdutosCampanhasCache.GetDictionaryXml()[key];
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
                _b2cConsultaProdutosCampanhasCache.AddList(_listSomenteNovos);
            }

            return true;
        }
    }
}
