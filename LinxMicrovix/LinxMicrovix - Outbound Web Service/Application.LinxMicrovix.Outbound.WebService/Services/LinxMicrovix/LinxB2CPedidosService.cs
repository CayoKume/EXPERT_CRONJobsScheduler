﻿using Application.IntegrationsCore.Interfaces;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.Base;
using Application.LinxMicrovix.Outbound.WebService.Interfaces.LinxMicrovix;
using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Api;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using System.ComponentModel.DataAnnotations;

namespace Application.LinxMicrovix.Outbound.WebService.Services.LinxMicrovix
{
    public class LinxB2CPedidosService : ILinxB2CPedidosService
    {
        private readonly IAPICall _apiCall;
        private readonly ILoggerService _logger;
        private readonly ILinxMicrovixServiceBase _linxMicrovixServiceBase;
        private readonly ILinxMicrovixAzureSQLRepositoryBase<LinxB2CPedidos> _linxMicrovixRepositoryBase;
        private readonly ILinxB2CPedidosRepository _linxB2CPedidosRepository;
        private static List<string?> _linxB2CPedidosCache { get; set; } = new List<string?>();

        public LinxB2CPedidosService(
            IAPICall apiCall,
            ILoggerService logger,
            ILinxMicrovixServiceBase linxMicrovixServiceBase,
            ILinxMicrovixAzureSQLRepositoryBase<LinxB2CPedidos> linxMicrovixRepositoryBase,
            ILinxB2CPedidosRepository linxB2CPedidosRepository
        )
        {
            _apiCall = apiCall;
            _logger = logger;
            _linxMicrovixServiceBase = linxMicrovixServiceBase;
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;
            _linxB2CPedidosRepository = linxB2CPedidosRepository;
        }

        public List<LinxB2CPedidos?> DeserializeXMLToObject(LinxAPIParam jobParameter, List<Dictionary<string?, string?>> records)
        {
            var list = new List<LinxB2CPedidos>();
            for (int i = 0; i < records.Count(); i++)
            {
                try
                {
                    var validations = new List<ValidationResult>();

                    var entity = new LinxB2CPedidos(
                        listValidations: validations,
                        order_id: records[i].Where(pair => pair.Key == "order_id").Select(pair => pair.Value).FirstOrDefault(),
                        id_pedido: records[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).FirstOrDefault(),
                        dt_pedido: records[i].Where(pair => pair.Key == "dt_pedido").Select(pair => pair.Value).FirstOrDefault(),
                        cod_cliente_erp: records[i].Where(pair => pair.Key == "cod_cliente_erp").Select(pair => pair.Value).FirstOrDefault(),
                        cod_cliente_b2c: records[i].Where(pair => pair.Key == "cod_cliente_b2c").Select(pair => pair.Value).FirstOrDefault(),
                        vl_frete: records[i].Where(pair => pair.Key == "vl_frete").Select(pair => pair.Value).FirstOrDefault(),
                        forma_pgto: records[i].Where(pair => pair.Key == "forma_pgto").Select(pair => pair.Value).FirstOrDefault(),
                        plano_pagamento: records[i].Where(pair => pair.Key == "plano_pagamento").Select(pair => pair.Value).FirstOrDefault(),
                        anotacao: records[i].Where(pair => pair.Key == "anotacao").Select(pair => pair.Value).FirstOrDefault(),
                        taxa_impressao: records[i].Where(pair => pair.Key == "taxa_impressao").Select(pair => pair.Value).FirstOrDefault(),
                        finalizado: records[i].Where(pair => pair.Key == "finalizado").Select(pair => pair.Value).FirstOrDefault(),
                        valor_frete_gratis: records[i].Where(pair => pair.Key == "valor_frete_gratis").Select(pair => pair.Value).FirstOrDefault(),
                        tipo_frete: records[i].Where(pair => pair.Key == "tipo_frete").Select(pair => pair.Value).FirstOrDefault(),
                        id_status: records[i].Where(pair => pair.Key == "id_status").Select(pair => pair.Value).FirstOrDefault(),
                        cod_transportador: records[i].Where(pair => pair.Key == "cod_transportador").Select(pair => pair.Value).FirstOrDefault(),
                        tipo_cobranca_frete: records[i].Where(pair => pair.Key == "tipo_cobranca_frete").Select(pair => pair.Value).FirstOrDefault(),
                        ativo: records[i].Where(pair => pair.Key == "ativo").Select(pair => pair.Value).FirstOrDefault(),
                        empresa: records[i].Where(pair => pair.Key == "empresa").Select(pair => pair.Value).FirstOrDefault(),
                        id_tabela_preco: records[i].Where(pair => pair.Key == "id_tabela_preco").Select(pair => pair.Value).FirstOrDefault(),
                        valor_credito: records[i].Where(pair => pair.Key == "valor_credito").Select(pair => pair.Value).FirstOrDefault(),
                        cod_vendedor: records[i].Where(pair => pair.Key == "cod_vendedor").Select(pair => pair.Value).FirstOrDefault(),
                        dt_insert: records[i].Where(pair => pair.Key == "dt_insert").Select(pair => pair.Value).FirstOrDefault(),
                        dt_disponivel_faturamento: records[i].Where(pair => pair.Key == "dt_disponivel_faturamento").Select(pair => pair.Value).FirstOrDefault(),
                        mensagem_falha_faturamento: records[i].Where(pair => pair.Key == "mensagem_falha_faturamento").Select(pair => pair.Value).FirstOrDefault(),
                        id_tipo_b2c: records[i].Where(pair => pair.Key == "id_tipo_b2c").Select(pair => pair.Value).FirstOrDefault(),
                        ecommerce_origem: records[i].Where(pair => pair.Key == "ecommerce_origem").Select(pair => pair.Value).FirstOrDefault(),
                        marketplace_loja: records[i].Where(pair => pair.Key == "marketplace_loja").Select(pair => pair.Value).FirstOrDefault(),
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
                                message: $"Error when convert record - id_pedido: {records[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).FirstOrDefault()} | order_id: {records[i].Where(pair => pair.Key == "order_id").Select(pair => pair.Value).FirstOrDefault()}\n" +
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
                        message: $"Error when convert record - id_pedido: {records[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).FirstOrDefault()} | order_id: {records[i].Where(pair => pair.Key == "order_id").Select(pair => pair.Value).FirstOrDefault()}",
                        exceptionMessage: ex.Message
                    );
                }
            };

            return list;
        }

        public async Task<bool> GetRecords(LinxAPIParam jobParameter)
        {
            try
            {
                _logger
                   .Clear()
                   .AddLog(EnumJob.LinxB2CPedidos);

                string? parameters = await _linxMicrovixRepositoryBase.GetParameters(jobParameter.parametersInterval, jobParameter.parametersTableName, jobParameter.jobName);

                var timestamp = await _linxMicrovixRepositoryBase.GetLast7DaysMaxTimestamp(
                    jobParameter.schema, 
                    jobParameter.tableName
                );

                var body = _linxMicrovixServiceBase.BuildBodyRequest(
                            parametersList: parameters.Replace("[0]", timestamp),
                            jobParameter: jobParameter,
                            cnpj_emp: jobParameter.docMainCompany
                        );

                string? response = await _apiCall.PostAsync(jobParameter: jobParameter, body: body);
                var xmls = _linxMicrovixServiceBase.DeserializeResponseToXML(jobParameter, response);

                if (xmls.Count() > 0)
                {
                    var listRecords = DeserializeXMLToObject(jobParameter, xmls);

                    if (_linxB2CPedidosCache.Count == 0)
                        _linxB2CPedidosCache = await _linxB2CPedidosRepository.GetRegistersExists(
                            jobParameter: jobParameter, 
                            registros: listRecords
                                        .Select(x => x.id_pedido)
                                        .ToList()
                        );

                    var _listSomenteNovos = listRecords.Where(x => !_linxB2CPedidosCache.Any(y => 
                        y == x.recordKey
                    )).ToList();

                    if (_listSomenteNovos.Count() > 0)
                    {
                        _linxB2CPedidosRepository.BulkInsertIntoTableRaw(records: _listSomenteNovos, jobParameter: jobParameter);
                        await _linxMicrovixRepositoryBase.CallDbProcMerge(jobParameter.schema, jobParameter.tableName, _logger.GetExecutionGuid());

                        for (int i = 0; i < _listSomenteNovos.Count; i++)
                        {
                            _logger.AddRecord(_listSomenteNovos[i].recordKey, _listSomenteNovos[i].recordXml);
                        }

                        _linxB2CPedidosCache.AddRange(_listSomenteNovos.Select(x => x.recordKey));

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
                _logger.SetLogEndDate();
                await _logger.CommitAllChanges();
            }

            return true;
        }
    }
}
