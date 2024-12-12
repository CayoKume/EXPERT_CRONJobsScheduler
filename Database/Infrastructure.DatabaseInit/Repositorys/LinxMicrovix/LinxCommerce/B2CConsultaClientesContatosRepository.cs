﻿using Dapper;
using Domain.DatabaseInit.Interfaces.LinxMicrovix.LinxCommerce;

using Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using System.Reflection.Metadata;
using Z.Dapper.Plus;

namespace Infrastructure.DatabaseInit.Repositorys.LinxMicrovix.LinxCommerce
{
    public class B2CConsultaClientesContatosRepository : IB2CConsultaClientesContatosRepository
    {
        private readonly ISQLServerConnection? _conn;

        public B2CConsultaClientesContatosRepository(ISQLServerConnection? conn) =>
            _conn = conn;

        public bool CreateDataTableIfNotExists(string databaseName, string jobName, string untreatedDatabaseName)
        {
            string? sql = @$"SELECT DISTINCT * FROM [INFORMATION_SCHEMA].[TABLES] (NOLOCK) WHERE TABLE_NAME = '{jobName}'";

            try
            {
                using (var conn = _conn.GetIDbConnection(databaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaClientesContatos>(tableName: $"{jobName}");
                }

                using (var conn = _conn.GetIDbConnection(databaseName))
                {
                    var result = conn.Query(sql: sql);

                    if (result.Count() == 0)
                        conn.CreateTable<B2CConsultaClientesContatos>(tableName: $"{jobName}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateTableMerge(string databaseName, string tableName)
        {
            string? sql = @"IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'P_B2CCONSULTACLIENTESCONTATOS_SYNC')
                           BEGIN
                           EXECUTE (
	                           'CREATE PROCEDURE [P_B2CCONSULTACLIENTESCONTATOS_SYNC] AS
	                           BEGIN
		                           MERGE [B2CCONSULTACLIENTESCONTATOS_TRUSTED] AS TARGET
                                   USING [B2CCONSULTACLIENTESCONTATOS_RAW] AS SOURCE

                                   ON (
			                           TARGET.ID_CLIENTES_CONTATOS = SOURCE.ID_CLIENTES_CONTATOS
		                           )
        
		                           WHEN MATCHED AND SOURCE.[parameters_timestamp] != TARGET.[parameters_timestamp] THEN 
			                           UPDATE SET
			                           TARGET.[LASTUPDATEON] = SOURCE.[LASTUPDATEON],
			                           TARGET.[ID_CLIENTES_CONTATOS] = SOURCE.[ID_CLIENTES_CONTATOS],
			                           TARGET.[ID_CONTATO_B2C] = SOURCE.[ID_CONTATO_B2C],
			                           TARGET.[NOME_CONTATO] = SOURCE.[NOME_CONTATO],
			                           TARGET.[DATA_NASC_CONTATO] = SOURCE.[DATA_NASC_CONTATO],
			                           TARGET.[SEXO_CONTATO] = SOURCE.[SEXO_CONTATO],
			                           TARGET.[ID_PARENTESCO] = SOURCE.[ID_PARENTESCO],
			                           TARGET.[FONE_CONTATO] = SOURCE.[FONE_CONTATO],
			                           TARGET.[CELULAR_CONTATO] = SOURCE.[CELULAR_CONTATO],
			                           TARGET.[EMAIL_CONTATO] = SOURCE.[EMAIL_CONTATO],
			                           TARGET.[COD_CLIENTE_ERP] = SOURCE.[COD_CLIENTE_ERP],
			                           TARGET.[parameters_timestamp] = SOURCE.[parameters_timestamp],
			                           TARGET.[PORTAL] = SOURCE.[PORTAL]

                                   WHEN NOT MATCHED BY TARGET AND SOURCE.[ID_CLIENTES_CONTATOS] NOT IN (SELECT [ID_CLIENTES_CONTATOS] FROM [B2CCONSULTACLIENTESCONTATOS_TRUSTED]) THEN
			                           INSERT
			                           ([LASTUPDATEON], [ID_CLIENTES_CONTATOS], [ID_CONTATO_B2C], [NOME_CONTATO], [DATA_NASC_CONTATO], [SEXO_CONTATO],
			                           [ID_PARENTESCO], [FONE_CONTATO], [CELULAR_CONTATO], [EMAIL_CONTATO], [COD_CLIENTE_ERP], [parameters_timestamp], [PORTAL])
			                           VALUES
			                           (SOURCE.[LASTUPDATEON], SOURCE.[ID_CLIENTES_CONTATOS], SOURCE.[ID_CONTATO_B2C], SOURCE.[NOME_CONTATO], SOURCE.[DATA_NASC_CONTATO], SOURCE.[SEXO_CONTATO],
			                           SOURCE.[ID_PARENTESCO], SOURCE.[FONE_CONTATO], SOURCE.[CELULAR_CONTATO], SOURCE.[EMAIL_CONTATO], SOURCE.[COD_CLIENTE_ERP], SOURCE.[parameters_timestamp], SOURCE.[PORTAL]);
	                           END'
                           )
                           END";

            try
            {
                using (var conn = _conn.GetIDbConnection(databaseName))
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

        public async Task<bool> InsertParametersIfNotExists(string jobName, string parametersTableName, string databaseName)
        {
            try
            {
                var parameter = new
                {
                    method = jobName,
                    timestamp = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    dateinterval = @"<Parameter id=""timestamp"">[0]</Parameter>",
                    individual = @"<Parameter id=""timestamp"">[0]</Parameter>"
                };

                string? sql = $"IF NOT EXISTS (SELECT * FROM [{parametersTableName}] WHERE [method] = '{jobName}') " +
                              $"INSERT INTO [{parametersTableName}] ([method], [parameters_timestamp], [parameters_dateinterval], [parameters_individual]) " +
                               "VALUES (@method, @timestamp, @dateinterval, @individual)";


                using (var conn = _conn.GetIDbConnection(databaseName))
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