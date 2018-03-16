using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class Items : BaseClasses.DomainBase
    {
        public Guid ItemGuid { get; set; }
        public Guid RestaurantGuid { get; set; }
        [Required(ErrorMessage ="Please enter item name.")]
        public string ItemName { get; set; }
        [Required]
        public Guid CategoryGuid { get; set; }
        public string CategoryName { get; set; }
        public decimal DeliveryCharge { get; set; }
        public string ItemImage { get; set; }
        public string ItemSize { get; set; }
        public string ItemWeight { get; set; }
        [Required(ErrorMessage = "Please enter price for item.")]
        public decimal ItemPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public List<LookUpDomain> CategoryList { get; set; }
    }
    public class OrderItem : BaseClasses.DomainBase
    {
        public Guid OrderItemGuid { get; set; }
        public Guid OrderGuid { get; set; }
        public Guid ItemGuid { get;set;}
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
    public class Order : BaseClasses.DomainBase
    {
        public Guid OrderGuid { get; set; }
        public Guid OrderItemGuid { get; set; }        
        public Guid ItemGuid { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
    public class ShoppingCart
    {
        public List<Items> ItemsList { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
    public class LookUpDomain
    {
        public Guid LookUpDomainValueGuid { get; set; }
        public Guid LookUpDomainGuid { get; set; }
        public string LookUpDomainValue { get; set; }
        public string LookUpDomainText { get; set; }
        public bool IsActive { get; set; }
        public string DateCreated { get; set; }
    }
}
