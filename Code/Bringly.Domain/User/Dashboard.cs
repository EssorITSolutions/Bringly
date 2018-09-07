using System;
using System.Collections.Generic;

namespace Bringly.Domain.User
{
    /// <summary>
    /// Wrapper for the dashboard data
    /// </summary>
    public class Dashboard
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Dashboard() { }

        /// <summary>
        /// Wrpper for dashboard message
        /// </summary>
        public List<MyMessage> MyMessage { get; set; }

        /// <summary>
        /// Wrapper for dashboard myfavourites data
        /// </summary>
        public List<Business.BusinessObject> MyFavourites { get; set; }

        /// <summary>
        /// Wrapper for dashboard myreview data
        /// </summary>
        public MyReview MyReview { get; set; }

        /// <summary>
        /// Wrapper for dashboard myorders
        /// </summary>
        public MyOrder MyOrder { get; set; }

        /// <summary>
        /// Wrapper for dashboard mywallet
        /// </summary>
        public Wallet Wallet { get; set; }
    }

    /// <summary>
    /// Wrapper for the message
    /// </summary>
    public class MyMessage : BaseClasses.DomainBase
    {
        public Guid EmailGuid { get; set; }
        public string FromName { get; set; }
        public Guid EmailToGuid { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailFrom { get; set; }
        public string EmailFromImagePath { get; set; }
        public bool Read { get; set; }
        public Guid? CreatedByGuid { get; set; }
    }
}
