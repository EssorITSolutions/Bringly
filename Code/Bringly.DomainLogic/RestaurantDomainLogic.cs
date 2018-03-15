using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Domain;
using Bringly.DomainLogic.User;

namespace Bringly.DomainLogic
{
    public class RestaurantDomainLogic : BaseClass.DomainLogicBase
    {
        public RestaurantSearch GetRestaurantsByCity(City _city)
        {
            RestaurantSearch _restaurantSearch = new RestaurantSearch();
            _restaurantSearch.Restaurants = bringlyEntities.tblRestaurants.Select(r => new Restaurant { RestaurantImage = r.RestaurantImage, RestaurantGuid = r.RestaurantGuid, RestaurantName = r.RestaurantName, CityGuid = r.CityGuid,CityName=_city.CityName, IsFavorite = false }).ToList();
            if (_city.CityGuid != null)
            {
                _restaurantSearch.Restaurants = _restaurantSearch.Restaurants.Where(s => s.CityGuid == _city.CityGuid).ToList();
            }
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            List<Restaurant> favouriteRestaurants = userDomainLogic.FavouriteRestaurants();
            _restaurantSearch.CityGuid = _city.CityGuid;
            _restaurantSearch.CityName = _city.CityName;
            List<Guid> restaurentGuid = favouriteRestaurants.Select(c => c.RestaurantGuid).ToList();
            foreach (Restaurant restaurant in _restaurantSearch.Restaurants)
            {
                if (restaurentGuid.Contains(restaurant.RestaurantGuid))
                {
                    restaurant.IsFavorite = true;
                }
            }
            return _restaurantSearch;
        }

        public List<Items> GetItemsByRestaurantGuid(Guid restaurantGuid)
        {
            List<Items> itemslist = new List<Items>();
            if (restaurantGuid != Guid.Empty)
            {
                itemslist = bringlyEntities.tblItems.Where(x => x.RestaurantGuid == restaurantGuid && x.IsActive == true && x.IsDeleted == false).ToList().
                    Select(itm => new Items
                    {
                        ItemGuid = itm.ItemGuid,
                        ItemName = itm.ItemName,
                        ItemImage = itm.ItemImage,
                        RestaurantGuid = itm.RestaurantGuid,
                        CategoryGuid = itm.CategoryGuid.HasValue ? itm.CategoryGuid.Value : Guid.NewGuid(),
                        DeliveryCharge = itm.DeliveryCharge.HasValue ? itm.DeliveryCharge.Value : 0,
                        ItemWeight = itm.ItemWeight,
                        ItemSize = itm.ItemSize,
                        ItemPrice = itm.ItemPrice,
                        Discount = itm.Discount,
                        IsActive = itm.IsActive
                    }).ToList();
            }
            return itemslist;
        }
    }
}
