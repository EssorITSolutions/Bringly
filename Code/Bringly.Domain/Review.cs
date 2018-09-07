using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain
{
    /// <summary>
    /// Review wrapper for buyer
    /// </summary>
    public class MyReview : Paging
    {
        public Guid ReviewGuid { get; set; }
        public Guid BusinessGuid { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsSkipped { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please provide rating.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Please fill review.")]
        [MaxLength(500)]
        public string Review { get; set; }
        public string BusinessImage { get; set; }
        public string BusinessName { get; set; }

        /// <summary>
        /// List of business review with completed status
        /// </summary>
        public List<BusinessReview> GivenBusinessReviews { get; set; }

        /// <summary>
        /// List of business review with pending status
        /// </summary>
        public List<BusinessReview> PendingBusinessReviews { get; set; }
    }

    /// <summary>
    /// Business Review Wrapper 
    /// </summary>
    public class BusinessReview : BaseClasses.DomainBase
    {
        public Guid ReviewGuid { get; set; }
        public Guid BusinessGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string BusinessName { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string BusinessImage { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsApproved { get; set; }
        public bool IsProcessed { get; set; }
    }
}
