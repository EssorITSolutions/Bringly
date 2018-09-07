using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.Enums
{
    /// <summary>
    /// Image type
    /// User,
    /// Restaurant
    /// Item
    /// Default
    /// </summary>
    public enum ImageType
    {
        User,
        Restaurant,
        Item,
        Default
    }

    /// <summary>
    /// Message type
    /// Success,
    /// Error,
    /// Info,
    /// New user
    /// </summary>
    public enum MessageType
    {
        Success, Error, Info, NewUser
    }

    /// <summary>
    /// Template type
    /// Review,
    /// Mail,
    /// Feedback,
    /// Other
    /// </summary>
    public enum TemplateType
    {
        Review, Mail, FeedBack, Other
    }

    /// <summary>
    /// Order status
    /// 100. In-complete
    /// 101. In-progress
    /// 102. Completed
    /// 103. Cancelled
    /// 104. Pending
    /// 105. Rejected
    /// 106. Confirmed,
    /// 107. Payment Success
    /// 108. Order picked
    /// 109. Order delivered
    /// </summary>
    public enum OrderStatus : short
    {
        Incomplete = 100,
        Inprogress = 101,
        Completed = 102,
        Cancelled = 103,
        Pending = 104,
        Rejected = 105,
        Confirmed = 106,
        PaymentSuccess = 107,
        OrderPicked = 108,
        Delivered = 109
    }

    /// <summary>
    /// Payment method type
    /// 1. Online
    /// 2. Cash on delivery
    /// </summary>
    public enum PaymentMethodType
    {
        Online = 1,
        COD = 2
    }

    /// <summary>
    /// Order Type
    /// 1. Self Delivery
    /// 1. Pick up
    /// </summary>
    public enum OrderType : short
    {
        SelfDelivery = 1,
        PickUp = 2
    }

    /// <summary>
    /// Discount type
    /// 1. Assign to the total order
    /// 2. Assign to products
    /// </summary>
    public enum DiscountType : short
    {
        [Display(Name = "Assigned To Total Order")]
        AssignedToTotalOrder = 1,

        [Display(Name = "Assigned To Product")]
        AssignedToProduct = 2
    }

    /// <summary>
    /// Discount price type
    /// 1. Percentage
    /// 2. Flat
    /// </summary>
    public enum DiscountPriceType : short
    {
        Percentage = 1,
        Flat = 2
    }

    /// <summary>
    /// Discount limitation type
    /// 1. Unlimited
    /// 2. One time only
    /// </summary>
    public enum DiscountLimitationType : short
    {
        [Display(Name = "Unlimited")]
        Unlimited = 1,
        [Display(Name = "One Time Only")]
        OneTimeOnly = 2
    }

    /// <summary>
    /// Transaction type
    /// 1. Credit
    /// 2. Debit
    /// </summary>
    public enum TransactionType : short
    {
        Credit = 1,
        Debit = 2
    }

    /// <summary>
    /// Payment type
    /// 1. Debit Card
    /// 2. Credit Card
    /// 3. Net Banking
    /// 4. Refund
    /// </summary>
    public enum PaymentType : short
    {
        [Display(Name = "Debit Card")]
        DebitCard = 1,

        [Display(Name = "Credit Card")]
        CreditCard = 2,

        [Display(Name = "Net Banking")]
        NetBanking = 3,

        [Display(Name = "Refund")]
        Refund = 4,

        [Display(Name = "Cash on Delivery")]
        Cash = 5,
    }

    /// <summary>
    /// Transaction status
    /// 1. Success
    /// 0. Failed
    /// </summary>
    public enum TransactionStatus : short
    {
        Success = 1,
        Failed = 0
    }
}
