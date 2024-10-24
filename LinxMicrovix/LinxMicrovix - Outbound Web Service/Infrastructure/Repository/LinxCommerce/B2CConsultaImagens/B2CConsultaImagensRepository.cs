﻿using IntegrationsCore.Domain.Entities;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites;
using LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce;
using LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.Base;

namespace LinxMicrovix_Outbound_Web_Service.Infrastructure.Repository.LinxCommerce
{
    public class B2CConsultaImagensRepository<TEntity> : IB2CConsultaImagensRepository<TEntity> where TEntity : B2CConsultaImagens, new()
    {
        private readonly ILinxMicrovixRepositoryBase<TEntity> _linxMicrovixRepositoryBase;

        public B2CConsultaImagensRepository(ILinxMicrovixRepositoryBase<TEntity> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(JobParameter jobParameter, List<TEntity> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new TEntity());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].id_imagem, records[i].imagem, records[i].md5, records[i].timestamp, records[i].portal, records[i].url_imagem_blob);
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
                          "ON (TARGET.ID_IMAGEM = SOURCE.ID_IMAGEM) " +
                          "WHEN MATCHED THEN UPDATE SET " +
                          "TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], " +
                          "TARGET.[ID_IMAGEM] = SOURCE.[ID_IMAGEM], " +
                          "TARGET.[IMAGEM] = SOURCE.[IMAGEM], " +
                          "TARGET.[MD5] = SOURCE.[MD5], " +
                          "TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP], " +
                          "TARGET.[PORTAL] = SOURCE.[PORTAL], " +
                          "TARGET.[URL_IMAGEM_BLOB] = SOURCE.[URL_IMAGEM_BLOB] " +
                          "WHEN NOT MATCHED BY TARGET THEN " +
                          "INSERT " +
                          "([LASTUPDATEON], [ID_IMAGEM], [IMAGEM], [MD5], [TIMESTAMP], [PORTAL], [URL_IMAGEM_BLOB])" +
                          "VALUES " +
                          "(SOURCE.[LASTUPDATEON], SOURCE.[ID_IMAGEM], SOURCE.[IMAGEM], SOURCE.[MD5], SOURCE.[TIMESTAMP], SOURCE.[PORTAL], SOURCE.[URL_IMAGEM_BLOB]);";

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
                                                  <Parameter id=""id_imagem"">[id_imagem]</Parameter>",
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
                          "([lastupdateon], [id_imagem], [imagem], [md5], [timestamp], [portal], [url_imagem_blob]) " +
                          "Values " +
                          "(@lastupdateon, @id_imagem, @imagem, @md5, @timestamp, @portal, @url_imagem_blob)";

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
