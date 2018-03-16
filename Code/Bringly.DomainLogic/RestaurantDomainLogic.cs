using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bringly.Data;
using Bringly.Domain;
using Bringly.DomainLogic.User;

namespace Bringly.DomainLogic
{
    public class RestaurantDomainLogic : BaseClass.DomainLogicBase
    {
        public string DefaultImage = "profile.png";
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
                        CategoryGuid = itm.CategoryGuid,
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

        public List<Items> GetMerchantItems(Guid restaurantGuid)
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
                        CategoryGuid = itm.CategoryGuid,
                        DeliveryCharge = itm.DeliveryCharge.HasValue ? itm.DeliveryCharge.Value : 0,
                        ItemWeight = itm.ItemWeight,
                        ItemSize = itm.ItemSize,
                        ItemPrice = itm.ItemPrice,
                        Discount = itm.Discount,
                        IsActive = itm.IsActive,
                        CategoryName=bringlyEntities.tblLookUpDomainValues.Where(x=>x.LookUpDomainValueGuid==itm.CategoryGuid).ToList().FirstOrDefault().LookUpDomainText
                    }).ToList();
            }
            return itemslist;
        }

        public Items GetMenuItem(Nullable<Guid> ItemGuid)
        {
            if (ItemGuid != null && ItemGuid!=Guid.Empty)
            {
                Items MenuItem = bringlyEntities.tblItems.Where(x => x.ItemGuid == ItemGuid && x.IsDeleted == false).
                      Select(itm => new Items
                      {
                          ItemGuid = itm.ItemGuid,
                          ItemName = itm.ItemName,
                          ItemImage = itm.ItemImage,
                          RestaurantGuid = itm.RestaurantGuid,
                          CategoryGuid = itm.CategoryGuid,
                          DeliveryCharge = itm.DeliveryCharge.HasValue ? itm.DeliveryCharge.Value : 0,
                          ItemWeight = itm.ItemWeight,
                          ItemSize = itm.ItemSize,
                          ItemPrice = itm.ItemPrice,
                          Discount = itm.Discount,
                          IsActive = itm.IsActive,
                          CategoryName = bringlyEntities.tblLookUpDomainValues.Where(x => x.LookUpDomainValueGuid == itm.CategoryGuid).ToList().FirstOrDefault().LookUpDomainText
                      })
                    .ToList().FirstOrDefault();
                MenuItem.CategoryList = CategoryList();
                return MenuItem;
            }
            else {
                Items MenuItem = new Items();
                MenuItem.CategoryList = CategoryList();
                MenuItem.RestaurantGuid = bringlyEntities.tblRestaurants.Where(x => x.IsDeleted == false).OrderByDescending(xx=>xx.DateCreated).FirstOrDefault().RestaurantGuid;
                return MenuItem;
            }
        }

        public List<LookUpDomain> CategoryList()
        {
            return bringlyEntities.tblLookUpDomainValues.Where(x=>x.IsActive==true && x.IsDeleted==false).ToList().
                        Select(itm => new LookUpDomain {
                            LookUpDomainGuid = itm.LookUpDomainGuid,
                            LookUpDomainValueGuid = itm.LookUpDomainValueGuid,
                            LookUpDomainText = itm.LookUpDomainText,
                            LookUpDomainValue = itm.LookUpDomainValue,
                            IsActive = itm.IsActive,
                        })
                        .ToList();
        }

        public bool AddEditItem(Items item)
        {
            tblItem tblItem = new tblItem();
            if (item != null && item.ItemGuid != Guid.Empty)
            {
                tblItem = bringlyEntities.tblItems.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.ItemName == item.ItemName && x.ItemGuid != item.ItemGuid).ToList().FirstOrDefault();
                if (tblItem != null) { return false; }
                else {
                    
                    tblItem.ItemGuid = item.ItemGuid;
                    tblItem.ItemName = item.ItemName;
                    tblItem.CategoryGuid = item.CategoryGuid;
                    tblItem.RestaurantGuid = item.RestaurantGuid;
                    tblItem.DeliveryCharge = item.DeliveryCharge;
                    tblItem.ItemImage = item.ItemImage;
                    tblItem.ItemName = item.ItemName;
                    tblItem.ItemWeight = item.ItemWeight;
                    tblItem.ItemSize = item.ItemSize;
                    tblItem.ItemPrice = item.ItemPrice;
                    tblItem.Discount = item.Discount;
                    tblItem.ModifiedDate = DateTime.Now;
                    tblItem.Modifiedby = UserVariables.LoggedInUserGuid;                    
                }
            }
            else
            {
                tblItem = bringlyEntities.tblItems.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.ItemName == item.ItemName).ToList().FirstOrDefault();
                if (tblItem != null) { return false; }
                else
                {
                    tblItem = new tblItem();
                    tblItem.ItemGuid = Guid.NewGuid();
                    tblItem.CreatedByGuid = UserVariables.LoggedInUserGuid;
                    tblItem.ItemName = item.ItemName;
                    tblItem.CategoryGuid = item.CategoryGuid;
                    tblItem.RestaurantGuid = item.RestaurantGuid;
                    tblItem.DeliveryCharge = item.DeliveryCharge;
                    tblItem.ItemImage = string.IsNullOrEmpty(item.ItemImage) ? DefaultImage : item.ItemImage;
                    tblItem.ItemWeight = item.ItemWeight;
                    tblItem.ItemSize = item.ItemSize;
                    tblItem.ItemPrice = item.ItemPrice;
                    tblItem.Discount = item.Discount;
                    tblItem.DateCreated = DateTime.Now;
                    tblItem.IsActive = item.IsActive;
                    tblItem.IsDeleted = false;
                    bringlyEntities.tblItems.Add(tblItem);                   
                }
            }
            bringlyEntities.SaveChanges();
            return true;
        }

        public string UploadMenuItemImage(HttpRequestBase Request)
        {
            tblUser user = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            string imageName = "";
            string imageLocation = "";
            if (Request.Files.Count > 0)
            {
                Items item = new Items();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    imageName = Path.GetFileName("Item_" + Guid.NewGuid() + Path.GetExtension(Request.Files[i].FileName));
                    imageLocation = CommonDomainLogic.GetImagePath(Domain.Enums.ImageType.Item, imageName);
                    Request.Files[i].SaveAs(HttpContext.Current.Server.MapPath(imageLocation));
                }
                item.ItemImage = imageName;
                return imageLocation;
            }
            else
            {
                return imageLocation;
            }

        }
    }
}
