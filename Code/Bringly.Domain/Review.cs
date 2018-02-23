using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class MyReview:Paging
    {
        public Guid ReviewGuid { get; set; }
        public Guid RestaurantGuid { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsSkipped { get; set; }
        [Range(1, int.MaxValue,ErrorMessage = "Please provide rating.")]
        public int Rating { get; set; }
        [Required(ErrorMessage="Please fill review.")]
        public string Review { get; set; }
        public string RestaurantImage { get; set; }
        public int TotalRecords { get; set; }
        public List<RestaurantReview> RestaurantReviews { get; set; }
    }
    public class RestaurantReview : BaseClasses.DomainBase
    {
        public Guid ReviewGuid { get; set; }
        public Guid RestaurantGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string RestaurantName { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string RestaurantImage { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsApproved { get; set; }
        public bool IsProcessed { get; set; }
    }
}
