﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxMicrovix;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxMicrovix
{
    public class LinxMovimentoCartoesRepository : ILinxMovimentoCartoesRepository
    {
        private readonly ILinxMicrovixAzureSQLRepositoryBase<LinxMovimentoCartoes> _linxMicrovixRepositoryBase;

        public LinxMovimentoCartoesRepository(ILinxMicrovixAzureSQLRepositoryBase<LinxMovimentoCartoes> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<LinxMovimentoCartoes> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter.tableName, new LinxMovimentoCartoes());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].identificador, records[i].portal, records[i].cnpj_emp, records[i].codlojasitef, records[i].data_lancamento,
                        records[i].cupomfiscal, records[i].credito_debito, records[i].id_cartao_bandeira, records[i].descricao_bandeira, records[i].valor, records[i].ordem_cartao,
                        records[i].nsu_host, records[i].nsu_sitef, records[i].cod_autorizacao, records[i].id_antecipacoes_financeiras, records[i].transacao_servico_terceiro,
                        records[i].texto_comprovante, records[i].id_maquineta_pos, records[i].descricao_maquineta, records[i].serie_maquineta, records[i].timestamp, records[i].cartao_prepago);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(
                    dataTable: table,
                    dataTableRowsNumber: table.Rows.Count
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string?>> GetRegistersExists(LinxAPIParam jobParameter, List<LinxMovimentoCartoes?> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].identificador}'";
                    else
                        identificadores += $"'{registros[i].identificador}', ";
                }

                string sql = $"SELECT CONCAT('[', CNPJ_EMP, ']', '|', '[', CUPOMFISCAL, ']', '|', '[', COD_AUTORIZACAO, ']', '|', '[', IDENTIFICADOR, ']', '|', '[', [TIMESTAMP], ']') FROM [linx_microvix_erp].[LinxMovimentoCartoes] WHERE identificador IN ({identificadores})";

                return await _linxMicrovixRepositoryBase.GetKeyRegistersAlreadyExists(sql);
            }
            catch (Exception ex) when (ex is not InternalException && ex is not SQLCommandException)
            {
                throw new InternalException(
                    stage: EnumStages.GetRegistersExists,
                    error: EnumError.Exception,
                    level: EnumMessageLevel.Error,
                    message: "Error when filling identifiers to sql command",
                    exceptionMessage: ex.Message
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertRecord(LinxAPIParam jobParameter, LinxMovimentoCartoes? record)
        {
            string? sql = @$"INSERT INTO [untreated].[{jobParameter.tableName}]
                            ([lastupdateon],[identificador],[portal],[cnpj_emp],[codlojasitef],[data_lancamento],[cupomfiscal],[credito_debito],[id_cartao_bandeira],[descricao_bandeira],
                             [valor],[ordem_cartao],[nsu_host],[nsu_sitef],[cod_autorizacao],[id_antecipacoes_financeiras],[transacao_servico_terceiro],[texto_comprovante],
                             [id_maquineta_pos],[descricao_maquineta],[serie_maquineta],[timestamp],[cartao_prepago])
                            Values
                            (@lastupdateon,@identificador,@portal,@cnpj_emp,@codlojasitef,@data_lancamento,@cupomfiscal,@credito_debito,@id_cartao_bandeira,@descricao_bandeira,
                             @valor,@ordem_cartao,@nsu_host,@nsu_sitef,@cod_autorizacao,@id_antecipacoes_financeiras,@transacao_servico_terceiro,@texto_comprovante,@id_maquineta_pos,
                             @descricao_maquineta,@serie_maquineta,@timestamp,@cartao_prepago)";

            try
            {
                return await _linxMicrovixRepositoryBase.InsertRecord(jobParameter.tableName, sql: sql, record: record);
            }
            catch
            {
                throw;
            }
        }
    }
}
