﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaPlanosParcelasRepository : IB2CConsultaPlanosParcelasRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaPlanosParcelasRepository(ISQLServerConnection? conn) =>
            (_conn) = (conn);

        public async Task<bool> CreateDataTableIfNotExists(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME LIKE '%{jobParameter.jobName}%'";

            try
            {
                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.QueryAsync(sql: sql);

                    if (result.Count() == 0)
                    {
                        conn.CreateTable<B2CConsultaPlanosParcelas>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaPlanosParcelas>(tableName: $"{jobParameter.jobName}_trusted");

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateTableMerge(LinxMicrovixJobParameter jobParameter)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTAPLANOSPARCELAS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTAPLANOSPARCELAS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTAPLANOSPARCELAS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTAPLANOSPARCELAS_RAW] AS SOURCE

                                   ON (
			                           TARGET.[PLANO] = SOURCE.[PLANO]
		                           )
        
		                           WHEN MATCHED AND TARGET.[TIMESTAMP] != SOURCE.[TIMESTAMP] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[PLANO] = SOURCE.[PLANO],
			                           TARGET.[ORDEM_PARCELA] = SOURCE.[ORDEM_PARCELA],
			                           TARGET.[PRAZO_PARC] = SOURCE.[PRAZO_PARC],
			                           TARGET.[ID_PLANOS_PARCELAS] = SOURCE.[ID_PLANOS_PARCELAS],
			                           TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[PLANO] NOT IN (SELECT [PLANO] FROM [B2CCONSULTAPLANOSPARCELAS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [PLANO], [ORDEM_PARCELA], [PRAZO_PARC], [ID_PLANOS_PARCELAS], [TIMESTAMP], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[PLANO], SOURCE.[ORDEM_PARCELA], SOURCE.[PRAZO_PARC], SOURCE.[ID_PLANOS_PARCELAS], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);
	                           END'
                           )
                           END";

            try
            {
                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
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
                var parameter = new {
                    method = jobParameter.jobName,
                    timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                                <Parameter id=""plano"">[plano]</Parameter>",
                    ativo = 1
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{jobParameter.parametersTableName}] WHERE [method] = '{jobParameter.jobName}') " +
                              $"INSERT INTO [{jobParameter.parametersTableName}] ([method], [timestamp], [dateinterval], [individual], [ativo]) " +
                               "VALUES (@method, @timestamp, @dateinterval, @individual, @ativo)";


                using (var conn = _conn.GetIDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: parameter, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
