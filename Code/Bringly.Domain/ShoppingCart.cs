using Bringly.Domain.Enums;
using Bringly.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bringly.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Items : BaseClasses.DomainBase
    {
        public Guid ItemGuid { get; set; }
        public Guid BusinessGuid { get; set; }
        [Required(ErrorMessage = "Please enter item name.")]
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

        /// <summary>
        /// 
        /// </summary>
        public List<LookUpDomain> CategoryList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OrderItem : BaseClasses.DomainBase
    {
        public Guid OrderItemGuid { get; set; }
        public Guid OrderGuid { get; set; }
        public Guid ItemGuid { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryCharge { get; set; }
        public System.Guid CategoryGuid { get; set; }

        public string ItemImage { get; set; }
        public string ItemWeight { get; set; }
        public string ItemSize { get; set; }
        public System.Guid BusinessGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Order : BaseClasses.DomainBase
    {
        public string OrderNumber { get; set; }
        public Guid OrderGuid { get; set; }
        public Guid OrderItemGuid { get; set; }
        public Guid ItemGuid { get; set; }
        public Guid BusinessGuid { get; set; }
        public string BusinessName { get; set; }
        public string BusinessImage { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<OrderItem> OrderItems { get; set; }

        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserRegistration UserProfile { get; set; }

        public string PaymentMethod { get; set; }

        public string OrderType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeliveryBoy DeliveryBoy { get; set; }

        public DateTime DeliveryDate { get; set; }
        public DateTime DeliveryTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeliveryBoy
    {
        public string DeliveryBoyName { get; set; }
        public string DeliveryBoyContactNumber { get; set; }
        public Guid DeliveryBoyGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ShoppingCart
    {
        public List<Items> ItemsList { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public Guid OrderGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LookUpDomain
    {
        public Guid LookUpDomainValueGuid { get; set; }
        public Guid LookUpDomainGuid { get; set; }
        public string LookUpDomainValue { get; set; }
        public string LookUpDomainText { get; set; }
        public bool IsActive { get; set; }
        public string DateCreated { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyOrder : Paging
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Order> TotalOrders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Order> CompletedOrders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Order> PendingOrders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Order> CancelledOrders { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Order> RejectedOrders { get; set; }
        public int PendingOrderCount { get; set; }
        public int CompletedOrderCount { get; set; }
        public int CancelledOrderCount { get; set; }
        public int RejectedOrderCount { get; set; }
        public int TotalOrderCount { get; set; }
        public string SortByColumn { get; set; }
        public string SortOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OrderStatus? SortingOrderStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Discount : BaseClasses.DomainBase
    {
        public int CouponId { get; set; }

        [Required(ErrorMessage = "Please Select Branch.")]
        public Guid BranchGuid { get; set; }

        public string BranchName { get; set; }

        [Required(ErrorMessage = "Please Select Discount Type.")]
        public short DiscountType { get; set; }

        [Required(ErrorMessage = "Coupon name is required.")]
        [MaxLength(19, ErrorMessage = "Coupon name must be less than 20 characters.")]
        [RegularExpression("^[a-zA-Z0-9 $%]{2,19}$", ErrorMessage = "Coupon name must be less than 20 characters.")]
        public string CouponName { get; set; }

        public string CouponDescription { get; set; }

        [Required(ErrorMessage = "Please Select Discount Price Type")]
        public short DiscountPriceType { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please Select Discount Limitation")]
        public short DiscountLimitationType { get; set; }

        public Guid CreatedByGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CustomSelectListItem> BranchList { get; set; }

        public string[] ProductIds { get; set; }

        public string ProductIdsString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CustomSelectListItem> ProductList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyDiscount : Paging
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Discount> Discounts { get; set; }
    }
}
