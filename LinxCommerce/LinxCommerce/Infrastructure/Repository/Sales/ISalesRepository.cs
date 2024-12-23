﻿using IntegrationsCore.Domain.Entities;
using static Dapper.SqlMapper;
using System.Data;

namespace LinxCommerce.Infrastructure.Repository.Sales
{
    public interface ISalesRepository<TEntity> where TEntity : class, new()
    {
        public Task<bool> CreateDataTableIfNotExists(LinxMicrovixJobParameter jobParameter);
        public DataTable CreateSystemDataTable(LinxMicrovixJobParameter jobParameter, TEntity entity);
    }
}
