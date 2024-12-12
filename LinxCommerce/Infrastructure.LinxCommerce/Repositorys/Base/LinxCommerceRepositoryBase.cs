﻿using Microsoft.Extensions.Configuration;
using Domain.LinxCommerce.Entities.Parameters;
using System.Data;
using Dapper;
using System.ComponentModel;
using Infrastructure.IntegrationsCore.Connections.SQLServer;
using Infrastructure.IntegrationsCore.Connections.MySQL;
using Infrastructure.IntegrationsCore.Connections.PostgreSQL;
using System.Data.SqlClient;
using Domain.LinxCommerce.Interfaces.Repositorys.Base;

namespace Infrastructure.LinxCommerce.Repository.Base
{
    public class LinxCommerceRepositoryBase<TEntity> : ILinxCommerceRepositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly string? _parametersTableName;

        private readonly IConfiguration _configuration;
        private readonly ISQLServerConnection? _sqlServerConnection;
        private readonly IMySQLConnection? _mySQLConnection;
        private readonly IPostgreSQLConnection? _postgreSQLConnection;

        public LinxCommerceRepositoryBase(
            ISQLServerConnection sqlServerConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _sqlServerConnection = sqlServerConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxCommerce")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        public LinxCommerceRepositoryBase(
            IMySQLConnection mySQLConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _mySQLConnection = mySQLConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxCommerce")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        public LinxCommerceRepositoryBase(
            IPostgreSQLConnection postgreSQLConnection,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _postgreSQLConnection = postgreSQLConnection;
            _parametersTableName = _configuration
                                    .GetSection("LinxCommerce")
                                    .GetSection("ProjectParametersTableName")
                                    .Value;
        }

        public bool BulkInsertIntoTableRaw(LinxCommerceJobParameter jobParameter, DataTable dataTable, int dataTableRowsNumber)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    using var bulkCopy = new SqlBulkCopy(conn);
                    bulkCopy.DestinationTableName = $"[{jobParameter.tableName}_raw]";
                    bulkCopy.BatchSize = dataTableRowsNumber;
                    bulkCopy.BulkCopyTimeout = 360;
                    bulkCopy.WriteToServer(dataTable);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"BulkInsertIntoTableRaw",
                //    message = $"Error when trying to bulk insert records on table raw",
                //    schema = $"[{jobParameter.tableName}_raw]",
                //    command = " - ",
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> CallDbProcMerge(LinxCommerceJobParameter jobParameter)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync($"P_{jobParameter.tableName}_Sync", commandType: CommandType.StoredProcedure, commandTimeout: 2700);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteWithoutCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"CallDbProcMerge",
                //    message = $"Error when trying to run the merge procedure: P_{jobParameter.tableName}_Sync",
                //    schema = $"{jobParameter.tableName}",
                //    exception = ex.Message
                //};
            }
        }

        public Task<bool> CreateDataTableIfNotExists(LinxCommerceJobParameter jobParameter)
        {
            throw new NotImplementedException();
        }

        public DataTable CreateSystemDataTable(LinxCommerceJobParameter jobParameter, TEntity entity)
        {
            try
            {
                var properties = TypeDescriptor.GetProperties(typeof(TEntity));
                var dataTable = new DataTable(jobParameter.tableName);
                foreach (PropertyDescriptor prop in properties)
                {
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
                //throw new InternalErrorException()
                //{
                //    project = jobParameter.projectName,
                //    job = jobParameter.jobName,
                //    method = "CreateSystemDataTable",
                //    message = $"Error when convert system datatable to bulkinsert",
                //    record = $" - ",
                //    propertie = " - ",
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> DeleteLogResponse(LinxCommerceJobParameter jobParameter)
        {
            string? sql = $"DELETE FROM [{jobParameter.parametersLogTableName}] " +
                          $"WHERE METHOD = '{jobParameter.jobName}' " +
                          $"AND ID NOT IN (SELECT TOP 15 ID FROM [{jobParameter.parametersLogTableName}] ORDER BY ID DESC)";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"DeleteLogResponse",
                //    message = $"Error when trying to clear parameters log table",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> ExecuteQueryCommand(LinxCommerceJobParameter jobParameter, string? sql)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"ExecuteQueryCommand",
                //    message = $"Error when trying to execute command sql",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> ExecuteTruncateRawTable(LinxCommerceJobParameter jobParameter)
        {
            string? sql = $"TRUNCATE TABLE [{jobParameter.tableName}_raw]";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"ExecuteTruncateRawTable",
                //    message = $"Error when trying to truncate table raw",
                //    schema = $"[{jobParameter.tableName}_raw]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        //public async Task<string?> GetParameters(LinxCommerceJobParameter jobParameter)
        //{
        //    string? sql = $"SELECT {jobParameter.parametersInterval} " +
        //                 $"FROM [{jobParameter.parametersTableName}] (NOLOCK) " +
        //                  "WHERE " +
        //                 $"METHOD = '{jobParameter.jobName}'";

        //    try
        //    {
        //        throw new NotImplementedException();
        //        using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
        //        {
        //            return await conn.QueryFirstOrDefaultAsync<string?>(sql: sql, commandTimeout: 360);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //        throw new ObjectsNotFoundExcpetion()
        //        {
        //            project = $"{jobParameter.projectName} - IntegrationsCore",
        //            job = jobParameter.jobName,
        //            method = $"GetParameters",
        //            message = $"Error when trying to get parameters from database",
        //            schema = $"[{jobParameter.parametersTableName}]",
        //            command = sql,
        //            exception = ex.Message
        //        };
        //    }
        //}

        public async Task<string?> GetParameters(LinxCommerceJobParameter jobParameter)
        {
            string sql = $@"SELECT NUMBEROFDAYS FROM [BLOOMERS_LINX].[dbo].[LINXAPIPARAM] WHERE METHOD = '{jobParameter.tableName}'";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    return await conn.QueryFirstAsync<string>(sql: sql, commandTimeout: 360);
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"GetParameters",
                //    message = $"Error when trying to insert record in database table: {jobParameter.tableName}",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> InsertLogResponse(LinxCommerceJobParameter jobParameter, string? response, object record)
        {
            string? sql = $"INSERT INTO {jobParameter.parametersLogTableName} " +
              "([method], [execution_date], [parameters_interval], [response]) " +
              "Values " +
              "(@method, GETDATE(), @parameters_interval, @response)";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: record, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"InsertRecord",
                //    message = $"Error when trying to insert record in database table: {jobParameter.tableName}",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> InsertParametersIfNotExists(LinxCommerceJobParameter jobParameter, object parameter)
        {
            string? sql = $"IF NOT EXISTS (SELECT * FROM [{jobParameter.parametersTableName}] WHERE [method] = '{jobParameter.jobName}') " +
                         $"INSERT INTO [{jobParameter.parametersTableName}] ([method], [timestamp], [dateinterval], [individual], [ativo]) " +
                          "VALUES (@method, @timestamp, @dateinterval, @individual, @ativo)";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: parameter, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"InsertRecord",
                //    message = $"Error when trying to insert record in database table: {jobParameter.tableName}",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> InsertRecord(LinxCommerceJobParameter jobParameter, string? sql, object record)
        {
            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, param: record, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"InsertRecord",
                //    message = $"Error when trying to insert record in database table: {jobParameter.tableName}",
                //    schema = $"[{jobParameter.tableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }

        public async Task<bool> UpdateLogParameters(LinxCommerceJobParameter jobParameter, string? lastResponse)
        {
            string? sql = $"UPDATE {jobParameter.parametersTableName} " +
                          "SET LAST_EXECUTION = GETDATE(), " +
                          $"LAST_RESPONSE = '{lastResponse}' " +
                           "WHERE " +
                          $"METHOD = '{jobParameter.jobName}'";

            try
            {
                using (var conn = _sqlServerConnection.GetDbConnection(jobParameter.databaseName))
                {
                    var result = await conn.ExecuteAsync(sql: sql, commandTimeout: 360);

                    if (result > 0)
                        return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new ExecuteCommandException()
                //{
                //    project = $"{jobParameter.projectName} - IntegrationsCore",
                //    job = jobParameter.jobName,
                //    method = $"UpdateLogParameters",
                //    message = $"Error when trying to update date of last execution date in database table: {jobParameter.parametersLogTableName}",
                //    schema = $"[{jobParameter.parametersLogTableName}]",
                //    command = sql,
                //    exception = ex.Message
                //};
            }
        }
    }
}
