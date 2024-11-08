﻿using LinxCommerce.Domain.Entities.Sales.Model;
using LinxCommerce.Domain.Entities.Sales.Order;

namespace LinxCommerce.Domain.Entities.Sales.Responses
{
    public class SearchOrderResponse
    {
        public class Root
        {
            public List<Result> Result { get; set; }
        }

        public class Result
        {
            public string? OrderID { get; set; }
            public string? OrderNumber { get; set; }
            public string? MarketPlaceBrand { get; set; }
            public string? OriginalOrderID { get; set; }
            public string? WebSiteID { get; set; }
            public string? WebSiteIntegrationID { get; set; }
            public string? CustomerID { get; set; }
            public string? ShopperTicketID { get; set; }
            public string? ItemsQty { get; set; }
            public string? ItemsCount { get; set; }
            public string? TaxAmount { get; set; }
            public string? DeliveryAmount { get; set; }
            public string? DiscountAmount { get; set; }
            public string? PaymentTaxAmount { get; set; }
            public string? SubTotal { get; set; }
            public string? Total { get; set; }
            public string? TotalDue { get; set; }
            public string? TotalPaid { get; set; }
            public string? TotalRefunded { get; set; }
            public string? PaymentDate { get; set; }
            public string? PaymentStatus { get; set; }
            public string? ShipmentDate { get; set; }
            public string? ShipmentStatus { get; set; }
            public string? GlobalStatus { get; set; }
            public string? DeliveryPostalCode { get; set; }
            public string? CreatedChannel { get; set; }
            public string? TrafficSourceID { get; set; }
            public string? OrderStatusID { get; set; }
            public List<OrderItem> Items { get; set; } = new List<OrderItem>();
            public List<OrderTag> Tags { get; set; } = new List<OrderTag>();
            public List<OrderProperty> Properties { get; set; } = new List<OrderProperty>();
            public List<OrderAddress> Addresses { get; set; } = new List<OrderAddress>();
            public List<OrderPaymentMethod> PaymentMethods { get; set; } = new List<OrderPaymentMethod>();
            public List<OrderDeliveryMethod> DeliveryMethods { get; set; } = new List<OrderDeliveryMethod>();
            public List<Discount> Discounts { get; set; } = new List<Discount>();
            public List<Shipment> Shipments { get; set; } = new List<Shipment>();
            public List<Fulfillment> Fulfillments { get; set; } = new List<Fulfillment>();
            public string? CreatedDate { get; set; }
            public string? CreatedBy { get; set; }
            public string? ModifiedDate { get; set; }
            public string? ModifiedBy { get; set; }
            public string? Remarks { get; set; }
            public OrderSeller Seller { get; set; } = new OrderSeller();
            public string? SellerCommissionAmount { get; set; }
            public OrderSalesRepresentative SalesRepresentative { get; set; } = new OrderSalesRepresentative();
            public string? CommissionAmount { get; set; }
            public OrderType OrderType { get; set; } = new OrderType();
            public OrderInvoice OrderInvoice { get; set; } = new OrderInvoice();
            public string? OrderGroupID { get; set; }
            public string? OrderGroupNumber { get; set; }
            public string? HasConflicts { get; set; }
            public string? AcquiredDate { get; set; }
            public OrderExternalInfo ExternalInfo { get; set; } = new OrderExternalInfo();
            public string? HasHubOrderWithoutShipmentConflict { get; set; }
            public string? CustomerType { get; set; }
            public MultiSiteTenant MultiSiteTenant { get; set; } = new MultiSiteTenant();
            public string? CancelledDate { get; set; }
            public OrderWishlist Wishlist { get; set; } = new OrderWishlist();
            public string? WebSiteName { get; set; }
            public string? CustomerName { get; set; }
            public string? CustomerEmail { get; set; }
            public string? CustomerGender { get; set; }
            public string? CustomerBirthDate { get; set; }
            public string? CustomerPhone { get; set; }
            public string? CustomerCPF { get; set; }
            public string? CustomerCNPJ { get; set; }
            public string? CustomerTradingName { get; set; }
            public string? CustomerSiteTaxPayer { get; set; }
        }
    }
}
