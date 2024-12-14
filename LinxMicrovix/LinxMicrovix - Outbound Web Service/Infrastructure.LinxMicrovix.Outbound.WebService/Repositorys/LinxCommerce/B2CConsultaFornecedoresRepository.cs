﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaFornecedoresRepository : IB2CConsultaFornecedoresRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaFornecedores> _linxMicrovixRepositoryBase;

        public B2CConsultaFornecedoresRepository(ILinxMicrovixRepositoryBase<B2CConsultaFornecedores> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaFornecedores> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaFornecedores());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].cod_fornecedor, records[i].nome, records[i].nome_fantasia, records[i].tipo_pessoa, records[i].tipo_fornecedor, records[i].endereco, records[i].numero_rua,
                        records[i].bairro, records[i].cep, records[i].cidade, records[i].uf, records[i].documento, records[i].fone, records[i].email, records[i].pais, records[i]. obs, records[i].timestamp, records[i].portal);
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

        public async Task<bool> CreateTableMerge(LinxAPIParam jobParameter)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAFORNECEDORES_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAFORNECEDORES_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAFORNECEDORES] AS TARGET
                                   USING [B2CCONSULTAFORNECEDORES] AS SOURCE

                                   ON (
			                           TARGET.COD_FORNECEDOR = SOURCE.COD_FORNECEDOR
		                           )

                                   WHEN MATCHED AND SOURCE.[TIMESTAMP] != TARGET.[TIMESTAMP] THEN UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[COD_FORNECEDOR] = SOURCE.[COD_FORNECEDOR],
			                           TARGET.[NOME] = SOURCE.[NOME],
			                           TARGET.[NOME_FANTASIA] = SOURCE.[NOME_FANTASIA],
			                           TARGET.[TIPO_PESSOA] = SOURCE.[TIPO_PESSOA],
			                           TARGET.[TIPO_FORNECEDOR] = SOURCE.[TIPO_FORNECEDOR],
			                           TARGET.[ENDERECO] = SOURCE.[ENDERECO],
			                           TARGET.[NUMERO_RUA] = SOURCE.[NUMERO_RUA],
			                           TARGET.[BAIRRO] = SOURCE.[BAIRRO],
			                           TARGET.[CEP] = SOURCE.[CEP],
			                           TARGET.[CIDADE] = SOURCE.[CIDADE],
			                           TARGET.[UF] = SOURCE.[UF],
			                           TARGET.[DOCUMENTO] = SOURCE.[DOCUMENTO],
			                           TARGET.[FONE] = SOURCE.[FONE],
			                           TARGET.[EMAIL] = SOURCE.[EMAIL],
			                           TARGET.[PAIS] = SOURCE.[PAIS],
			                           TARGET.[OBS] = SOURCE.[OBS],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND [COD_FORNECEDOR] NOT IN (SELECT [COD_FORNECEDOR] FROM [B2CCONSULTAFORNECEDORES]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [COD_FORNECEDOR], [NOME], [NOME_FANTASIA], [TIPO_PESSOA], [TIPO_FORNECEDOR], [ENDERECO], [NUMERO_RUA], [BAIRRO], [CEP], [CIDADE], [UF], [DOCUMENTO], [FONE], [EMAIL], [PAIS], [OBS], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[COD_FORNECEDOR], SOURCE.[NOME], SOURCE.[NOME_FANTASIA], SOURCE.[TIPO_PESSOA], SOURCE.[TIPO_FORNECEDOR], SOURCE.[ENDERECO], SOURCE.[NUMERO_RUA], SOURCE.[BAIRRO], SOURCE.[CEP], SOURCE.[CIDADE], SOURCE.[UF], 
			                           SOURCE.[DOCUMENTO], SOURCE.[FONE], SOURCE.[EMAIL], SOURCE.[PAIS], SOURCE.[OBS], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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

        public async Task<List<B2CConsultaFornecedores>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaFornecedores> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].documento}'";
                    else
                        identificadores += $"'{registros[i].documento}', ";
                }

                string sql = $"SELECT DOCUMENTO, TIMESTAMP FROM B2CCONSULTAFORNECEDORES WHERE DOCUMENTO IN ({identificadores})";

                return await _linxMicrovixRepositoryBase.GetRegistersExists(jobParameter, sql);
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

        public async Task<bool> InsertParametersIfNotExists(LinxAPIParam jobParameter)
        {
            try
            {
                return await _linxMicrovixRepositoryBase.InsertParametersIfNotExists(
                    jobParameter: jobParameter,
                    parameter: new
                    {
                        method = jobParameter.jobName,
                        timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        individual = @"<Parameter id=""timestamp"">[0]</Parameter>
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

        public async Task<bool> InsertRecord(LinxAPIParam jobParameter, B2CConsultaFornecedores? record)
        {
            string? sql = $"INSERT INTO {jobParameter.tableName} " +
                          "([lastupdateon], [cod_fornecedor], [nome], [nome_fantasia], [tipo_pessoa], [tipo_fornecedor], [endereco], [numero_rua], [bairro], [cep], [cidade], [uf], [documento], [fone], [email], [pais], [obs], [timestamp], [portal]) " +
                          "Values " +
                          "(@lastupdateon, @cod_fornecedor, @nome, @nome_fantasia, @tipo_pessoa, @tipo_fornecedor, @endereco, @numero_rua, @bairro, @cep, @cidade, @uf, @documento, @fone, @email, @pais, @obs, @timestamp, @portal)";

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
