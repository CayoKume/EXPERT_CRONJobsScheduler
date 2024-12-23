﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix_Outbound_Web_Service.Entites.LinxCommerce;
using Domain.LinxMicrovix_Outbound_Web_Service.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix_Outbound_Web_Service.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix_Outbound_Web_Service.Repository.LinxCommerce
{
    public class B2CConsultaImagensRepository : IB2CConsultaImagensRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaImagens> _linxMicrovixRepositoryBase;

        public B2CConsultaImagensRepository(ILinxMicrovixRepositoryBase<B2CConsultaImagens> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxMicrovixJobParameter jobParameter, List<B2CConsultaImagens> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaImagens());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].id_imagem, records[i].md5, records[i].timestamp, records[i].portal, records[i].url_imagem_blob);
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

        public async Task<bool> CreateTableMerge(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAIMAGENS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAIMAGENS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAIMAGENS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAIMAGENS_RAW] AS SOURCE

                                   ON (
			                           TARGET.[ID_IMAGEM] = SOURCE.[ID_IMAGEM]
		                           )

                                   WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_IMAGEM] = SOURCE.[ID_IMAGEM],
			                           TARGET.[MD5] = SOURCE.[MD5],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL],
			                           TARGET.[URL_IMAGEM_BLOB] = SOURCE.[URL_IMAGEM_BLOB]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_IMAGEM] NOT IN (SELECT [ID_IMAGEM] FROM [B2CCONSULTAIMAGENS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_IMAGEM], [MD5], [TIMESTAMP], [PORTAL], [URL_IMAGEM_BLOB])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_IMAGEM], SOURCE.[MD5], SOURCE.[TIMESTAMP], SOURCE.[PORTAL], SOURCE.[URL_IMAGEM_BLOB]);
	                           END'
                           )
                           END";

            try
            {
                return await _linxMicrovixRepositoryBase.ExecuteQueryCommand(jobParameter: jobParameter, sql: sql);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertParametersIfNotExists(LinxMicrovixJobParameter jobParameter)
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

        public async Task<bool> InsertRecord(LinxMicrovixJobParameter jobParameter, B2CConsultaImagens? record)
        {
            string? sql = $"INSERT INTO {jobParameter.tableName}_raw " +
                          "([lastupdateon], [id_imagem], [md5], [timestamp], [portal], [url_imagem_blob]) " +
                          "Values " +
                          "(@lastupdateon, @id_imagem, @md5, @timestamp, @portal, @url_imagem_blob)";

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
