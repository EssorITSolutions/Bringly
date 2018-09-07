using System;
using System.Collections.Generic;

namespace Bringly.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class RestaurantSearch
    {
        public string CityName { get; set; }
        public Guid CityGuid { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Restaurant : BaseClasses.DomainBase
    {
        public Guid RestaurantGuid { get; set; }
        public string RestaurantName { get; set; }
        public bool IsFavorite { get; set; }
        public string CityName { get; set; }
        public string RestaurantImage { get; set; }
        public Guid CityGuid { get; set; }
    }
   
}
