﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxCommerce;
using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxCommerce
{
    public class B2CConsultaVendedoresRepository : IB2CConsultaVendedoresRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaVendedoresRepository(ISQLServerConnection? conn) =>
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
                        conn.CreateTable<B2CConsultaVendedores>(tableName: $"{jobParameter.jobName}_raw");
                        conn.CreateTable<B2CConsultaVendedores>(tableName: $"{jobParameter.jobName}_trusted");

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
            string? sql = $"MERGE [{jobParameter.tableName}_trusted] AS TARGET " +
                         $"USING [{jobParameter.tableName}_raw] AS SOURCE " +
                          "ON (TARGET.COD_VENDEDOR = SOURCE.COD_VENDEDOR) " +
                          "WHEN MATCHED THEN UPDATE SET " +
                          "TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON], " +
                          "TARGET.[COD_VENDEDOR] = SOURCE.[COD_VENDEDOR], " +
                          "TARGET.[NOME_VENDEDOR] = SOURCE.[NOME_VENDEDOR], " +
                          "TARGET.[COMISSAO_SERVICOS] = SOURCE.[COMISSAO_SERVICOS], " +
                          "TARGET.[TIPO] = SOURCE.[TIPO], " +
                          "TARGET.[ATIVO] = SOURCE.[ATIVO], " +
                          "TARGET.[COMISSIONADO] = SOURCE.[COMISSIONADO], " +
                          "TARGET.[TIMESTAMP] = SOURCE.[TIMESTAMP], " +
                          "TARGET.[PORTAL] = SOURCE.[PORTAL] " +
                          "WHEN NOT MATCHED BY TARGET THEN " +
                          "INSERT " +
                          "([LASTUPDATEON], [COD_VENDEDOR], [NOME_VENDEDOR], [COMISSAO_SERVICOS], [TIPO], [ATIVO], [COMISSIONADO], [TIMESTAMP], [PORTAL])" +
                          "VALUES " +
                          "(SOURCE.[LASTUPDATEON], SOURCE.[COD_VENDEDOR], SOURCE.[NOME_VENDEDOR], SOURCE.[COMISSAO_SERVICOS], SOURCE.[TIPO], SOURCE.[ATIVO], SOURCE.[COMISSIONADO], SOURCE.[TIMESTAMP], SOURCE.[PORTAL]);";

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
                    parameters_timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    parameters_dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    parameters_individual = @"<Parameter id=""timestamp"">[0]</Parameter>
                                              <Parameter id=""cod_vendedor"">[cod_vendedor]</Parameter>",
                    ativo = 1
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{jobParameter.parametersTableName}] WHERE [method] = '{jobParameter.jobName}') " +
                              $"INSERT INTO [{jobParameter.parametersTableName}] ([method], [parameters_timestamp], [parameters_dateinterval], [parameters_individual], [ativo]) " +
                               "VALUES (@method, @parameters_timestamp, @parameters_dateinterval, @parameters_individual, @ativo)";


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