﻿namespace LinxCommerce.Domain.Entities.Sales.Model
{
    public class MultiSiteTenant
    {
        public string? CompanyId { get; set; }
        public string? BrandId { get; set; }
        public string? BrandType { get; set; }
        public string? DeviceType { get; set; }
    }
}