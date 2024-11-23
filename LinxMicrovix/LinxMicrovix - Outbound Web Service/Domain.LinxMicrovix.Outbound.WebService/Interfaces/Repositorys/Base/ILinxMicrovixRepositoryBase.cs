﻿using Domain.IntegrationsCore.Entities.Parameters;
using Domain.LinxMicrovix.Outbound.WebService.Entites.Base;
using System.Data;

namespace Domain.LinxMicrovix.Outbound.WebService.Interfaces.Repositorys.Base
{
    public interface ILinxMicrovixRepositoryBase<TEntity> where TEntity : class
    {
        public Task<string?> GetParameters(LinxMicrovixJobParameter jobParameter);
        public Task<IEnumerable<Company>> GetB2CCompanys(LinxMicrovixJobParameter jobParameter);
        public Task<IEnumerable<Company>> GetMicrovixCompanys(LinxMicrovixJobParameter jobParameter);
        public Task<List<TEntity>> GetRegistersExists(LinxMicrovixJobParameter jobParameter, string sql);
        public Task<string?> GetLast7DaysMinTimestamp(LinxMicrovixJobParameter jobParameter, string? columnDate);
        public Task<bool> InsertRecord(LinxMicrovixJobParameter jobParameter, string? sql, object record);
        public Task<bool> ExecuteQueryCommand(LinxMicrovixJobParameter jobParameter, string? sql);
        public bool BulkInsertIntoTableRaw(LinxMicrovixJobParameter jobParameter, DataTable dataTable, int dataTableRowsNumber);
        public Task<bool> CallDbProcMerge(LinxMicrovixJobParameter jobParameter);
        public DataTable CreateSystemDataTable(LinxMicrovixJobParameter jobParameter, TEntity entity);

        #region MOVER PARA PROJETO DATABASE INITIALIZATION
        public Task<bool> InsertParametersIfNotExists(LinxMicrovixJobParameter jobParameter, object parameter);
        //public Task<bool> InsertLogResponse(LinxMicrovixJobParameter jobParameter, string? response, object record);
        //public Task<bool> DeleteLogResponse(LinxMicrovixJobParameter jobParameter);
        //public Task<bool> ExecuteTruncateRawTable(LinxMicrovixJobParameter jobParameter);
        //public Task<bool> UpdateLogParameters(LinxMicrovixJobParameter jobParameter, string? lastResponse);
        //public Task<bool> CreateDataTableIfNotExists(LinxMicrovixJobParameter jobParameter);
        #endregion
    }
}
