using Bringly.Domain.Enums;
using Bringly.Domain.User;
using System;
using System.Collections.Generic;

namespace Bringly.Domain
{
    public class DeliveryOption
    {
        public Guid OrderGuid { get; set; }
        public long OrderId { get; set; }
        public string UserFullName { get; set; }
        public Guid UserGuid { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public UserAddress BillingAddress { get; set; }
        public UserAddress ShippingAddress { get; set; }
        public OrderType DeliveryType { get; set; }
        public string DeliveryTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<City> CityList { get; set; }
        public List<Country> CountryList { get; set; }
    }
}
