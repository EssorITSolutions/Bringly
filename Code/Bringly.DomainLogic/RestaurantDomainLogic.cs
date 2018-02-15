using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Domain;
namespace Bringly.DomainLogic
{
    public class RestaurantDomainLogic : BaseClass.DomainLogicBase
    {
        public RestaurantSearch GetRestaurantsByCity(City _city)
        {
            RestaurantSearch _restaurantSearch = new RestaurantSearch();
            _restaurantSearch.Restaurants = bringlyEntities.tblRestaurants.Select(r => new Restaurant { RestaurantGuid = r.RestaurantGuid, RestaurantName = r.RestaurantName, CityGuid=r.CityGuid, IsFavorite = false }).ToList();
            if (_city.CityGuid != null)
            {
                _restaurantSearch.Restaurants = _restaurantSearch.Restaurants.Where(s => s.CityGuid == _city.CityGuid).ToList();
            }
            _restaurantSearch.CityGuid = _city.CityGuid;
            _restaurantSearch.CityName = _city.CityName;
            return _restaurantSearch;
        }
    }
}
