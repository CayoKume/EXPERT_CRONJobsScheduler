﻿namespace LinxCommerce.Domain.Entities.Sales.SalesRepresentative
{
    public class SalesRepresentativePortfolio
    {
        public bool HasPortfolio { get; set; }
        public string? PortfolioAssociationType { get; set; }
        public List<SalesRepresentativeCustomerRelation> Customers { get; set; } //list obj
    }
}
