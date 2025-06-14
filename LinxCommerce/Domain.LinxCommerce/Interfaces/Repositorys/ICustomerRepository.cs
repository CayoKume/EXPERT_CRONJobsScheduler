﻿using Domain.LinxCommerce.Entities.Parameters;
using Microsoft.Win32;

namespace Domain.LinxCommerce.Interfaces.Repositorys
{
    public interface ICustomerRepository
    {
        public bool BulkInsertIntoTableRaw(LinxCommerceJobParameter jobParameter, List<Domain.LinxCommerce.Entities.Customer.Person> registros, Guid? parentExecutionGUID);
        public Task<bool> InsertRecord(LinxCommerceJobParameter jobParameter, Domain.LinxCommerce.Entities.Customer.Person registro, Guid? parentExecutionGUID);
        public Task<List<Domain.LinxCommerce.Entities.Customer.Person>> GetRegistersExists(IEnumerable<int> customerIds);
    }
}
