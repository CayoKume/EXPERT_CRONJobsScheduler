﻿using IntegrationsCore.Domain.Entities;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce;
using LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.Base;

namespace LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.LinxCommerce
{
    public class B2CConsultaProdutosDetalhesDepositosRepository<TEntity> : IB2CConsultaProdutosDetalhesDepositosRepository<TEntity> where TEntity : B2CConsultaProdutosDetalhesDepositos, new()
    {
        private readonly ILinxMicrovixRepositoryBase<TEntity> _linxMicrovixRepositoryBase;

        public B2CConsultaProdutosDetalhesDepositosRepository(ILinxMicrovixRepositoryBase<TEntity> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(JobParameter jobParameter, List<TEntity> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new TEntity());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].codigoproduto, records[i].empresa, records[i].id_deposito, records[i].saldo, records[i].timestamp, records[i].portal, records[i].deposito, records[i].tempo_preparacao_estoque);
                }

                _linxMicrovixRepositoryBase.BulkInsertIntoTableRaw(
                    jobParameter: jobParameter,
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

        public async Task<bool> ExecuteTableMerge(JobParameter jobParameter)
        {
            string sql = $"MERGE [{jobParameter.tableName}_trusted] AS TARGET " +
                         $"USING [{jobParameter.tableName}_raw] AS SOURCE " +
                          "ON (TARGET.CODIGOPRODUTO = SOURCE.CODIGOPRODUTO) " +
                          "WHEN MATCHED THEN UPDATE SET " +
                          "TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], " +
                          "TARGET.[CODIGOPRODUTO] = SOURCE.[CODIGOPRODUTO], " +
                          "TARGET.[EMPRESA] = SOURCE.[EMPRESA], " +
                          "TARGET.[ID_DEPOSITO] = SOURCE.[ID_DEPOSITO], " +
                          "TARGET.[SALDO] = SOURCE.[SALDO], " +
                          "TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP], " +
                          "TARGET.[PORTAL] = SOURCE.[PORTAL], " +
                          "TARGET.[DEPOSITO] = SOURCE.[DEPOSITO], " +
                          "TARGET.[TEMPO_PREPARACAO_ESTOQUE] = SOURCE.[TEMPO_PREPARACAO_ESTOQUE] " +
                          "WHEN NOT MATCHED BY TARGET THEN " +
                          "INSERT " +
                          "([LASTUPDATEON], [CODIGOPRODUTO], [EMPRESA], [ID_DEPOSITO], [SALDO], [TIMESTAMP], [PORTAL], [DEPOSITO], [TEMPO_PREPARACAO_ESTOQUE])" +
                          "VALUES " +
                          "(SOURCE.[LASTUPDATEON], SOURCE.[CODIGOPRODUTO], SOURCE.[EMPRESA], SOURCE.[ID_DEPOSITO], SOURCE.[SALDO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL], SOURCE.[DEPOSITO], SOURCE.[TEMPO_PREPARACAO_ESTOQUE]);";

            try
            {
                return await _linxMicrovixRepositoryBase.ExecuteQueryCommand(jobParameter: jobParameter, sql: sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertParametersIfNotExists(JobParameter jobParameter)
        {
            try
            {
                return await _linxMicrovixRepositoryBase.InsertParametersIfNotExists(
                    jobParameter: jobParameter,
                    parameter: new
                    {
                        method = jobParameter.jobName,
                        parameters_timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        parameters_dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                  <Parameter id=""codigoproduto"">[codigoproduto]</Parameter>
                                                  <Parameter id=""deposito"">[deposito]</Parameter>",
                        ativo = 1
                    }
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertRecord(JobParameter jobParameter, TEntity? record)
        {
            string sql = $"INSERT INTO {jobParameter.tableName}_raw " +
                          "([lastupdateon], [codigoproduto], [empresa], [id_deposito], [saldo], [timestamp], [portal], [deposito], [tempo_preparacao_estoque]) " +
                          "Values " +
                          "(@lastupdateon, @codigoproduto, @empresa, @id_deposito, @saldo, @timestamp, @portal, @deposito, @tempo_preparacao_estoque)";

            try
            {
                return await _linxMicrovixRepositoryBase.InsertRecord(jobParameter: jobParameter, sql: sql, record: record);
            }
            catch
            {
                throw;
            }
        }
    }
}
