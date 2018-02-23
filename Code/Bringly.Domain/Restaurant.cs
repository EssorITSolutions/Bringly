using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class RestaurantSearch
    {
        public string CityName { get; set; }
        public Guid CityGuid { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }
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
