using Bringly.Domain;
using Bringly.Domain.Enums;
using System;
using System.Linq;

namespace Bringly.DomainLogic
{
    /// <summary>
    /// Business logic and calculations for the Wallet
    /// </summary>
    public class WalletDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// Get wallet by user giud
        /// Get the user' wallet details
        /// </summary>
        /// <param name="userGuid">User Guid for the wallet</param>
        /// <returns></returns>
        public Wallet GetWallet(Guid userGuid)
        {
            return bringlyEntities.tblWallets
                .Where(w => w.IsActive == true && w.IsDeleted == false && w.tblUser.UserGuid == userGuid)
                .Select(w => new Wallet
                {
                    Amount = w.Amount,
                    FK_CreatedByGuid = w.FK_CreatedByGuid,
                    DateCreated = w.DateCreated,
                    WalletGuid = w.WalletGuid,
                    WalletID = w.WalletID,
                    BranchGuid = w.tblBranch.BranchGuid,
                    BranchName = w.tblBranch.BranchName
                }).FirstOrDefault();
        }

        /// <summary>
        /// Get wallet history
        /// </summary>
        /// <param name="userGuid">User guid of wallet holder</param>
        /// <param name="LatestPage">Current page</param>
        /// <returns>Wallet history for the current page.</returns>
        public MyWalletHistory GetWalletHistory(Guid userGuid, int LatestPage = 0)
        {
            MyWalletHistory myWalletHistory = new MyWalletHistory();
            myWalletHistory.PageSize = 10;// PageSize;
            myWalletHistory.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;

            var walletHistoriesData = (from w in bringlyEntities.tblWallets
                                       where w.IsActive == true
                                       && w.IsDeleted == false
                                       && w.FK_CreatedByGuid == userGuid
                                       join wh in bringlyEntities.tblWalletHistories
                                       on w.WalletID equals wh.FK_WalletID
                                       select new WalletHistory
                                       {
                                           Amount = wh.Amount,
                                           DateCreated = wh.DateCreated,
                                           FK_WalletID = wh.FK_WalletID,
                                           Message = wh.Message,
                                           PaymentType = (PaymentType)wh.PaymentType,
                                           TransactionReferenceID = wh.TransactionReferenceID,
                                           TransactionStatus = (TransactionStatus)wh.TransactionStatus,
                                           TransactionStatusMessage = wh.TransactionStatusMessage,
                                           TransactionType = (TransactionType)wh.TransactionType,
                                           WalletHistoryID = wh.WalletHistoryID,
                                           TotalAmount = wh.TotalAmount,
                                           BranchGuid = w.tblBranch.BranchGuid,
                                           BranchName = w.tblBranch.BranchName
                                       });

            myWalletHistory.WalletHistories = walletHistoriesData.OrderBy(wh => wh.DateCreated).ToList();
            myWalletHistory.TotalRecords = myWalletHistory.WalletHistories.Count;

            myWalletHistory.TotalAmount = bringlyEntities.tblWallets
                .Where(w => w.IsActive == true && w.IsDeleted == false && w.FK_CreatedByGuid == userGuid)
                .Select(w => w.Amount)
                .FirstOrDefault();

            int skip = 0;
            int take = myWalletHistory.PageSize;
            if (myWalletHistory.CurrentPage == 1)
                skip = 0;
            else
                skip = ((myWalletHistory.CurrentPage * myWalletHistory.PageSize) - myWalletHistory.PageSize);

            myWalletHistory.WalletHistories = myWalletHistory.WalletHistories.OrderBy(wh => wh.DateCreated).Skip(skip).Take(take).ToList();

            return myWalletHistory;
        }
    }
}
