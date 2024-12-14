﻿using Domain.IntegrationsCore.Entities.Enums;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Parameters;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.LinxCommerce;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repository.LinxCommerce
{
    public class B2CConsultaClientesEstadoCivilRepository : IB2CConsultaClientesEstadoCivilRepository
    {
        private readonly ILinxMicrovixRepositoryBase<B2CConsultaClientesEstadoCivil> _linxMicrovixRepositoryBase;

        public B2CConsultaClientesEstadoCivilRepository(ILinxMicrovixRepositoryBase<B2CConsultaClientesEstadoCivil> linxMicrovixRepositoryBase) =>
            (_linxMicrovixRepositoryBase) = (linxMicrovixRepositoryBase);

        public bool BulkInsertIntoTableRaw(LinxAPIParam jobParameter, IList<B2CConsultaClientesEstadoCivil> records)
        {
            try
            {
                var table = _linxMicrovixRepositoryBase.CreateSystemDataTable(jobParameter, new B2CConsultaClientesEstadoCivil());

                for (int i = 0; i < records.Count(); i++)
                {
                    table.Rows.Add(records[i].lastupdateon, records[i].id_estado_civil, records[i].estado_civil, records[i].timestamp, records[i].portal);
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
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTACLIENTESESTADOCIVIL_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTACLIENTESESTADOCIVIL_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTACLIENTESESTADOCIVIL] AS TARGET
		                           USING [B2CCONSULTACLIENTESESTADOCIVIL] AS SOURCE

		                           ON (
			                           TARGET.[ID_ESTADO_CIVIL] = SOURCE.[ID_ESTADO_CIVIL]
		                           )

		                           WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_ESTADO_CIVIL] = SOURCE.[ID_ESTADO_CIVIL],
			                           TARGET.[ESTADO_CIVIL] = SOURCE.[ESTADO_CIVIL],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

		                           WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_ESTADO_CIVIL] NOT IN (SELECT [ID_ESTADO_CIVIL] FROM [B2CCONSULTACLIENTESESTADOCIVIL]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_ESTADO_CIVIL], [ESTADO_CIVIL], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_ESTADO_CIVIL], SOURCE.[ESTADO_CIVIL], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
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

        public async Task<List<B2CConsultaClientesEstadoCivil>> GetRegistersExists(LinxAPIParam jobParameter, List<B2CConsultaClientesEstadoCivil> registros)
        {
            try
            {
                var identificadores = String.Empty;
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (i == registros.Count() - 1)
                        identificadores += $"'{registros[i].id_estado_civil}'";
                    else
                        identificadores += $"'{registros[i].id_estado_civil}', ";
                }

                string sql = $"SELECT ID_ESTADO_CIVIL, TIMESTAMP FROM B2CCONSULTACLIENTESESTADOCIVIL WHERE ID_ESTADO_CIVIL IN ({identificadores})";

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
                        individual = @"<Parameter id=""timestamp"">[0]</Parameter>",
                        ativo = 1
                    }
                );
            }
            catch
            {
                throw;
            }
        }
    }
}
