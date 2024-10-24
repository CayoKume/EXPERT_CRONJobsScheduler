﻿using IntegrationsCore.Domain.Entities;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce;
using LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.Base;

namespace LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.LinxCommerce
{
    public class B2CConsultaTransportadoresRepository<TEntity> : IB2CConsultaTransportadoresRepository<TEntity> where TEntity : B2CConsultaTransportadores, new()
    {
        private readonly ILinxMicrovixRepositoryBase<TEntity> _linxMicrovixRepositoryBase;

        public B2CConsultaTransportadoresRepository(ILinxMicrovixRepositoryBase<TEntity> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(JobParameter jobParameter, List<TEntity> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new TEntity());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].cod_transportador, records[i].nome, records[i].nome_fantasia, records[i].tipo_pessoa, records[i].tipo_transportador, records[i].endereco, records[i].numero_rua,
                        records[i].bairro, records[i].cep, records[i].cidade, records[i].uf, records[i].documento, records[i].fone, records[i].email, records[i].pais, records[i].obs, records[i].timestamp, records[i].portal);
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
                          "ON (TARGET.COD_TRANSPORTADOR = SOURCE.COD_TRANSPORTADOR) " +
                          "WHEN MATCHED THEN UPDATE SET " +
                          "TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], " +
                          "TARGET.[COD_TRANSPORTADOR] = SOURCE.[COD_TRANSPORTADOR], " +
                          "TARGET.[NOME] = SOURCE.[NOME], " +
                          "TARGET.[NOME_FANTASIA] = SOURCE.[NOME_FANTASIA], " +
                          "TARGET.[TIPO_PESSOA] = SOURCE.[TIPO_PESSOA], " +
                          "TARGET.[TIPO_TRANSPORTADOR] = SOURCE.[TIPO_TRANSPORTADOR], " +
                          "TARGET.[ENDRECO] = SOURCE.[ENDRECO], " +
                          "TARGET.[NUMERO_RUA] = SOURCE.[NUMERO_RUA], " +
                          "TARGET.[BAIRRO] = SOURCE.[BAIRRO], " +
                          "TARGET.[CEP] = SOURCE.[CEP], " +
                          "TARGET.[CIDADE] = SOURCE.[CIDADE], " +
                          "TARGET.[UF] = SOURCE.[UF], " +
                          "TARGET.[DOCUMENTO] = SOURCE.[DOCUMENTO], " +
                          "TARGET.[FONE] = SOURCE.[FONE], " +
                          "TARGET.[EMAIL] = SOURCE.[EMAIL], " +
                          "TARGET.[PAIS] = SOURCE.[PAIS], " +
                          "TARGET.[OBS] = SOURCE.[OBS], " +
                          "TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP], " +
                          "TARGET.[PORTAL] = SOURCE.[PORTAL] " +
                          "WHEN NOT MATCHED BY TARGET THEN " +
                          "INSERT " +
                          "([LASTUPDATEON], [COD_TRANSPORTADOR], [NOME], [NOME_FANTASIA], [TIPO_PESSOA], [TIPO_TRANSPORTADOR], [ENDRECO], [NUMERO_RUA], [BAIRRO], [CEP], [CIDADE], [UF], [DOCUMENTO], [FONE], [EMAIL], [PAIS], [OBS], [TIMESTAMP], [PORTAL])" +
                          "VALUES " +
                          "(SOURCE.[LASTUPDATEON], SOURCE.[COD_TRANSPORTADOR], SOURCE.[NOME], SOURCE.[NOME_FANTASIA], SOURCE.[TIPO_PESSOA], SOURCE.[TIPO_TRANSPORTADOR], SOURCE.[ENDRECO], SOURCE.[NUMERO_RUA], SOURCE.[BAIRRO], SOURCE.[CEP], SOURCE.[CIDADE], SOURCE.[UF], " +
                          "SOURCE.[DOCUMENTO], SOURCE.[FONE], SOURCE.[EMAIL], SOURCE.[PAIS], SOURCE.[OBS], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);";

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
                                                  <Parameter id=""documento"">[documento]</Parameter>",
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
                          "([lastupdateon], [cod_transportador], [nome], [nome_fantasia], [tipo_pessoa], [tipo_transportador], [endereco], [numero_rua], [bairro], [cep], [cidade], [uf], [documento], [fone], [email], [pais], [obs], [timestamp], [portal]) " +
                          "Values " +
                          "(@lastupdateon, @cod_transportador, @nome, @nome_fantasia, @tipo_pessoa, @tipo_transportador, @endereco, @numero_rua, @bairro, @cep, @cidade, @uf, @documento, @fone, @email, @pais, @obs, @timestamp, @portal)";

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
