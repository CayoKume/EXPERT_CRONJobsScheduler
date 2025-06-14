﻿using Dapper;
using Domain.IntegrationsCore.Entities.Enums;
using Domain.IntegrationsCore.Exceptions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.Base;
using Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base;
using Infrastructure.IntegrationsCore.Connections.MySQL;
using Infrastructure.IntegrationsCore.Connections.PostgreSQL;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Repositorys.Base
{
    public class LinxMicrovixSQLServerRepositoryBase<TEntity> : ILinxMicrovixSQLServerRepositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly string? _parametersTableName;

        private readonly IConfiguration _configuration;
        private readonly ISQLServerConnection? _sqlServerConnection;
        private readonly IMySQLConnection? _mySQLConnection;
        private readonly IPostgreSQLConnection? _postgreSQLConnection;

        public LinxMicrovixSQLServerRepositoryBase(
            ISQLServerConnection sqlServerConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _sqlServerConnection = sqlServerConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxMicrovix")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        public LinxMicrovixSQLServerRepositoryBase(
            IMySQLConnection mySQLConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _mySQLConnection = mySQLConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxMicrovix")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        public LinxMicrovixSQLServerRepositoryBase(
            IPostgreSQLConnection postgreSQLConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _postgreSQLConnection = postgreSQLConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxMicrovix")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        /// <summary>
        /// Call Procedure from SQL Server Database
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public async Task<bool> CallDbProcMerge(string databaseName, string? tableName)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    var result = await conn.ExecuteAsync($"[{databaseName}]..[P_{tableName}_Sincronizacao]", commandType: CommandType.StoredProcedure, commandTimeout: 2700);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.CallDbProcMerge,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to run the merge procedure: P_{tableName}_Sincronizacao",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get Companys from SQL Server Database B2CConsultaEmpresas Table
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<IEnumerable<Company>> GetB2CCompanys(string databaseName)
        {
            string? sql = $@"SELECT  
                           EMPRESA AS COD_COMPANY,
                           CNPJ_EMP AS DOC_COMPANY,
                           NOME_EMP AS REASON_COMPANY,
                           NOME_EMP AS NAME_COMPANY,
                           EMAIL_UNIDADE AS EMAIL_COMPANY,
                           END_UNIDADE AS ADDRESS_COMPANY,
                           NR_RUA_UNIDADE AS STREET_NUMBER_COMPANY,
                           COMPLEMENTO_END_UNIDADE AS COMPLEMENT_ADDRESS_COMPANY,
                           BAIRRO_UNIDADE AS NEIGHBORHOOD_COMPANY,
                           CIDADE_UNIDADE AS CITY_COMPANY,
                           UF_UNIDADE AS UF_COMPANY,
                           CEP_UNIDADE AS ZIP_CODE_COMPANY,
                           '' AS FONE_COMPANY,
                           '' AS STATE_REGISTRATION_COMPANY,
                           '' AS MUNICIPAL_REGISTRATION_COMPANY
                           FROM 
                           [{databaseName}]..[B2CCONSULTAEMPRESAS] (NOLOCK)";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryAsync<Company>(sql: sql);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetParameters,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetB2CCompanys,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get Companys from SQL Server Database LinxLojas Table
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<IEnumerable<Company>> GetMicrovixCompanys(string databaseName)
        {
            string? sql = $@"SELECT  
                           EMPRESA AS COD_COMPANY,
                           CNPJ_EMP AS DOC_COMPANY,
                           RAZAO_EMP AS REASON_COMPANY,
                           NOME_EMP AS NAME_COMPANY,
                           INSCRICAO_EMP AS STATE_REGISTRATION_COMPANY,
                           EMAIL_EMP AS EMAIL_COMPANY,
                           ENDERECO_EMP AS ADDRESS_COMPANY,
                           NUM_EMP AS STREET_NUMBER_COMPANY,
                           COMPLEMENT_EMP AS COMPLEMENT_ADDRESS_COMPANY,
                           BAIRRO_EMP AS NEIGHBORHOOD_COMPANY,
                           CIDADE_EMP AS CITY_COMPANY,
                           ESTADO_EMP AS UF_COMPANY,
                           CEP_EMP AS ZIP_CODE_COMPANY,
                           FONE_EMP AS FONE_COMPANY,
                           INSCRICAO_MUNICIPAL_EMP AS MUNICIPAL_REGISTRATION_COMPANY
                           FROM 
                           [{databaseName}]..[LINXLOJAS] (NOLOCK)";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryAsync<Company>(sql: sql);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetMicrovixCompanys,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetMicrovixCompanys,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get Companys from SQL Server Database LinxGrupoLojas Table
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<IEnumerable<Company>> GetMicrovixGroupCompanys(string databaseName)
        {
            string? sql = $@"SELECT  
                           EMPRESA AS COD_COMPANY,
                           CNPJ AS DOC_COMPANY,
                           NOME_EMPRESA AS REASON_COMPANY,
                           NOME_EMPRESA AS NAME_COMPANY
                           FROM 
                           [{databaseName}].[LINXGRUPOLOJAS] (NOLOCK)
                           WHERE CNPJ <> ''";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryAsync<Company>(sql: sql);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetMicrovixCompanys,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetMicrovixGroupCompanys,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get companys from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Insert Record to SQL Server Database
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <param name="databaseName"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<bool> InsertRecord(string? tableName, string? sql, string databaseName, object record)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: record, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.InsertRecord,
                    message: $"Error when trying to insert record in database table: {tableName}",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.InsertRecord,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to insert record in database table: {tableName}",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get Parameters from SQL Server Database LinxAPIParam Table
        /// </summary>
        /// <param name="parametersInterval"></param>
        /// <param name="parametersTableName"></param>
        /// <param name="jobName"></param>
        /// <param name="databaseName"></param>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<string?> GetParameters(string? parametersInterval, string? parametersTableName, string? jobName, string databaseName)
        {
            string? sql = $"SELECT {parametersInterval} " +
                          $"FROM [{databaseName}]..[{parametersTableName}] (NOLOCK) " +
                           "WHERE " +
                          $"METHOD = '{jobName}'";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryFirstOrDefaultAsync<string?>(sql: sql, commandTimeout: 360);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetParameters,
                    message: $"Error when trying to get parameters from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetParameters,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get parameters from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get Parameters from SQL Server Database Tables (ex: LinxProdutosDepositos, LinxProdutosTabelas, LinxSetores)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="databaseName"></param>
        /// <exception cref="SQLCommandException"></exception>
        /// <exception cref="InternalException"></exception>
        public async Task<IEnumerable<string?>> GetParameters(string sql, string databaseName)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryAsync<string?>(sql: sql, commandTimeout: 360);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetParameters,
                    message: $"Error when trying to get parameters from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetParameters,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get parameters from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Create a System DATA .NET DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public DataTable CreateSystemDataTable(string? tableName, TEntity entity)
        {
            try
            {
                var properties = TypeDescriptor.GetProperties(typeof(TEntity));
                var dataTable = new DataTable(tableName);
                foreach (PropertyDescriptor prop in properties)
                {
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.CreateSystemDataTable,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when convert system datatable to bulkinsert",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get the last timestamp value from SQL Server DataTable
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <param name="columnDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string?> GetLast7DaysMinTimestamp(string? schema, string? tableName, string? columnDate, string? databaseName)
        {
            string? sql = "SELECT ISNULL(MIN(TIMESTAMP), 0) " +
                                     $"FROM [{schema}].[{tableName}] (NOLOCK) " +
                                      "WHERE " +
                                     $"{columnDate} < GETDATE() - 7";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryFirstOrDefaultAsync<string?>(sql: sql, commandTimeout: 360);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetLast7DaysMinTimestamp,
                    message: $"Error when trying to get last timestamp from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetLast7DaysMinTimestamp,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get last timestamp from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get the last timestamp value from SQL Server DataTable
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <param name="columnDate"></param>
        /// <param name="columnCompany"></param>
        /// <param name="companyValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string?> GetLast7DaysMinTimestamp(string? schema, string? tableName, string? columnDate, string? columnCompany, string? companyValue, string? databaseName)
        {
            string? sql = "SELECT ISNULL(MIN(TIMESTAMP), 0) " +
                                     $"FROM [{schema}].[{tableName}] (NOLOCK) " +
                                      "WHERE " +
                                     $"{columnDate} < GETDATE() - 7 AND {columnCompany} = '{companyValue}'";

            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    return await conn.QueryFirstOrDefaultAsync<string?>(sql: sql, commandTimeout: 360);
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetLast7DaysMinTimestamp,
                    message: $"Error when trying to get last timestamp from database",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetLast7DaysMinTimestamp,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get last timestamp from database",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Get registers that already existis from SQL Server Database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TEntity>> GetRegistersExists(string sql, string? databaseName)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(databaseName))
                {
                    var result = await conn.QueryAsync<TEntity>(sql: sql, commandTimeout: 360);
                    return result.ToList();
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.GetRegistersExists,
                    message: $"Error when trying to get records that already exist in trusted table",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.GetRegistersExists,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to get records that already exist in trusted table",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Execute SQL Command on SQL Server Database
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExecuteQueryCommand(string? sql, string? databaseName)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetIDbConnection(databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw new SQLCommandException(
                    stage: EnumStages.ExecuteQueryCommand,
                    message: $"Error when trying to execute command sql",
                    exceptionMessage: ex.Message,
                    commandSQL: sql
                );
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.ExecuteQueryCommand,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to execute command sql",
                    exceptionMessage: ex.Message
                );
            }
        }

        /// <summary>
        /// Execute bulk insert on SQL Server Database
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dataTableRowsNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool BulkInsertIntoTableRaw(DataTable dataTable, int dataTableRowsNumber, string? databaseName)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(databaseName))
                {
                    using var bulkCopy = new SqlBulkCopy(conn);
                    bulkCopy.DestinationTableName = $"[untreated].[{dataTable.TableName}]";
                    bulkCopy.BatchSize = dataTableRowsNumber;
                    bulkCopy.BulkCopyTimeout = 360;
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    }
                    bulkCopy.WriteToServer(dataTable);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InternalException(
                    stage: EnumStages.BulkInsertIntoTableRaw,
                    error: EnumError.SQLCommand,
                    level: EnumMessageLevel.Error,
                    message: $"Error when trying to bulk insert records on table raw",
                    exceptionMessage: ex.Message
                );
            }
        }
    }
}
