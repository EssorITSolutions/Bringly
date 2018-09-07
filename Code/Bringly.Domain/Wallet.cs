using Bringly.Domain.Enums;
using Bringly.Domain.User;
using System;
using System.Collections.Generic;

namespace Bringly.Domain
{
    /// <summary>
    /// Wallet wrapper model
    /// </summary>
    public class Wallet
    {
        public long WalletID { get; set; }
        public System.Guid WalletGuid { get; set; }
        public System.Guid FK_CreatedByGuid { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> BranchGuid { get; set; }
        public string BranchName { get; set; }
        /// <summary>
        /// User Profile of associated user 
        /// </summary>
        public UserProfile User { get; set; }

        /// <summary>
        /// List of wallet history
        /// </summary>
        public List<WalletHistory> WalletHistories { get; set; }
    }

    /// <summary>
    /// Wallet history model object wrapper
    /// </summary>
    public class WalletHistory
    {
        public long WalletHistoryID { get; set; }
        public long FK_WalletID { get; set; }

        /// <summary>
        /// Enum for the Transaction type
        /// </summary>
        public TransactionType TransactionType { get; set; }
        public string TransactionReferenceID { get; set; }

        /// <summary>
        /// Enum for the Payment type
        /// </summary>
        public PaymentType PaymentType { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// Enum for the transaction status
        /// </summary>
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusMessage { get; set; }
        public string Message { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<System.Guid> BranchGuid { get; set; }
        public string BranchName { get; set; }
        /// <summary>
        /// Wallet model object
        /// </summary>
        public Wallet Wallet { get; set; }
    }

    /// <summary>
    /// Model to wrap wallet history
    /// </summary>
    public class MyWalletHistory : Paging
    {
        /// <summary>
        /// List of wallet history
        /// </summary>
        public List<WalletHistory> WalletHistories { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
