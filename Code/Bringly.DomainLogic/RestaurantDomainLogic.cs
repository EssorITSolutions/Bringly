using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Business;
using Bringly.DomainLogic.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bringly.DomainLogic
{
    public class RestaurantDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string DefaultImage = "profile.png";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_city"></param>
        /// <returns></returns>
        public RestaurantSearch GetRestaurantsByCity(City _city)
        {
            RestaurantSearch _restaurantSearch = new RestaurantSearch();
            _restaurantSearch.Restaurants = bringlyEntities.tblRestaurants
                .Select(r => new Restaurant
                {
                    RestaurantImage = r.RestaurantImage,
                    RestaurantGuid = r.RestaurantGuid,
                    RestaurantName = r.RestaurantName,
                    CityGuid = r.CityGuid,
                    CityName = _city.CityName,
                    IsFavorite = false
                }).ToList();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessGuid"></param>
        /// <returns></returns>
        public BusinessObject GetRestaurantByRestaurantGuid(Guid businessGuid)
        {
            BusinessObject business = new BusinessObject();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            List<City> list = commonDomainLogic.GetCities();
            business = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == businessGuid).Select(r => new BusinessObject
            {
                BusinessImage = r.BusinessImage
                ,
                BusinessGuid = r.BusinessGuid
                ,
                BusinessName = r.BusinessName
                ,
                CityGuid = r.CityGuid
                ,
                BusinessTypeGuid = r.FK_BusinessTypeGuid
                ,
                PNumber = r.PNumber
                ,
                Phone = r.Phone
                ,
                PinCode = r.PinCode
                ,
                CreatedByGuid = r.FK_CreatedByGuid
                ,
                ManagerGuid = r.ManagerUserGuid
                ,
                Address = r.Address
                ,
                Email = r.Email
                ,
                OrderTiming = r.OrderTiming
                ,
                PickUpTiming = r.PickUpTiming
                ,
                ServiceCharge = r.ServiceCharge
                ,
                ServiceTax = r.ServiceTax
                ,
                FlatRate = r.FlatRate
                ,
                RateAfterKm = r.RateAfterKm
                ,
                Description = r.Description
            }).FirstOrDefault();
            business.CityList = list;
            business.CityName = business.CityGuid != Guid.Empty ? bringlyEntities.tblCities.Where(x => x.CityGuid == business.CityGuid).FirstOrDefault().CityName : "";
            return business;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public string UpdateRestaurantProfile(BusinessObject BusinessObject)
        {
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            tblBusiness business = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == BusinessObject.BusinessGuid).FirstOrDefault();
            business.BusinessName = BusinessObject.BusinessName;
            business.CityGuid = BusinessObject.CityGuid;
            business.PNumber = BusinessObject.PNumber;
            business.Phone = BusinessObject.Phone;
            business.PinCode = BusinessObject.PinCode;
            business.ModifiedBy = UserVariables.LoggedInUserGuid;
            business.ModifiedDate = DateTime.Now;
            business.Address = BusinessObject.Address;
            business.Email = BusinessObject.Email;

            business.OrderTiming = BusinessObject.OrderTiming;
            business.PickUpTiming = BusinessObject.PickUpTiming;
            business.ServiceCharge = BusinessObject.ServiceCharge;
            business.ServiceTax = BusinessObject.ServiceTax;
            business.FlatRate = BusinessObject.FlatRate;
            business.RateAfterKm = BusinessObject.RateAfterKm;
            business.Description = BusinessObject.Description;

            bringlyEntities.SaveChanges();
            string cityname = bringlyEntities.tblCities.Where(x => x.CityGuid == BusinessObject.CityGuid).FirstOrDefault().CityName;
            return cityname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurantGuid"></param>
        /// <returns></returns>
        public List<Items> GetItemsByRestaurantGuid(Guid restaurantGuid)
        {
            List<Items> itemslist = new List<Items>();
            if (restaurantGuid != Guid.Empty)
            {
                itemslist = bringlyEntities.tblItems.Where(x => x.FK_BusinessGuid == restaurantGuid && x.IsActive == true
                && x.IsDeleted == false).ToList().
                    Select(itm => new Items
                    {
                        ItemGuid = itm.ItemGuid,
                        ItemName = itm.ItemName,
                        ItemImage = itm.ItemImage,
                        BusinessGuid = itm.FK_BusinessGuid,
                        CategoryGuid = itm.FK_CategoryGuid,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurantGuid"></param>
        /// <returns></returns>
        public List<Items> GetMerchantItems(Guid restaurantGuid)
        {
            List<Items> itemslist = new List<Items>();
            if (restaurantGuid != Guid.Empty)
            {
                itemslist = bringlyEntities.tblItems.Where(x => x.FK_BusinessGuid == restaurantGuid && x.IsActive == true && x.IsDeleted == false).ToList().
                    Select(itm => new Items
                    {
                        ItemGuid = itm.ItemGuid,
                        ItemName = itm.ItemName,
                        ItemImage = itm.ItemImage,
                        BusinessGuid = itm.FK_BusinessGuid,
                        CategoryGuid = itm.FK_CategoryGuid,
                        DeliveryCharge = itm.DeliveryCharge.HasValue ? itm.DeliveryCharge.Value : 0,
                        ItemWeight = itm.ItemWeight,
                        ItemSize = itm.ItemSize,
                        ItemPrice = itm.ItemPrice,
                        Discount = itm.Discount,
                        IsActive = itm.IsActive,
                        CategoryName = bringlyEntities.tblLookUpDomainValues.Where(x => x.LookUpDomainValueGuid == itm.FK_CategoryGuid).FirstOrDefault().LookUpDomainText
                    }).ToList();
            }
            return itemslist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <returns></returns>
        public Items GetMenuItem(Nullable<Guid> ItemGuid)
        {
            if (ItemGuid != null && ItemGuid != Guid.Empty)
            {
                Items MenuItem = bringlyEntities.tblItems.Where(x => x.ItemGuid == ItemGuid && x.IsDeleted == false).
                      Select(itm => new Items
                      {
                          ItemGuid = itm.ItemGuid,
                          ItemName = itm.ItemName,
                          ItemImage = itm.ItemImage,
                          BusinessGuid = itm.FK_BusinessGuid,
                          CategoryGuid = itm.FK_CategoryGuid,
                          DeliveryCharge = itm.DeliveryCharge.HasValue ? itm.DeliveryCharge.Value : 0,
                          ItemWeight = itm.ItemWeight,
                          ItemSize = itm.ItemSize,
                          ItemPrice = itm.ItemPrice,
                          Discount = itm.Discount,
                          IsActive = itm.IsActive,
                          CategoryName = bringlyEntities.tblLookUpDomainValues
                          .Where(x => x.LookUpDomainValueGuid == itm.FK_CategoryGuid)
                          .FirstOrDefault()
                          .LookUpDomainText
                      }).FirstOrDefault();

                MenuItem.CategoryList = CategoryList();
                return MenuItem;
            }
            else
            {
                Items MenuItem = new Items();
                MenuItem.CategoryList = CategoryList();
                MenuItem.BusinessGuid = bringlyEntities.tblRestaurants
                    .Where(x => x.IsDeleted == false).OrderByDescending(xx => xx.DateCreated)
                    .FirstOrDefault()
                    .RestaurantGuid;

                return MenuItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<LookUpDomain> CategoryList()
        {
            return bringlyEntities.tblLookUpDomainValues.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().
                        Select(itm => new LookUpDomain
                        {
                            LookUpDomainGuid = itm.FK_LookUpDomainGuid,
                            LookUpDomainValueGuid = itm.LookUpDomainValueGuid,
                            LookUpDomainText = itm.LookUpDomainText,
                            LookUpDomainValue = itm.LookUpDomainValue,
                            IsActive = itm.IsActive,
                        })
                        .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddEditItem(Items item)
        {
            tblItem tblItem = new tblItem();
            if (item != null && item.ItemGuid != Guid.Empty)
            {
                tblItem = bringlyEntities.tblItems
                    .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.ItemName == item.ItemName && x.ItemGuid != item.ItemGuid)
                    .FirstOrDefault();

                if (tblItem != null) { return false; }
                else
                {

                    tblItem.ItemGuid = item.ItemGuid;
                    tblItem.ItemName = item.ItemName;
                    tblItem.FK_CategoryGuid = item.CategoryGuid;
                    tblItem.FK_BusinessGuid = item.BusinessGuid;
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
                tblItem = bringlyEntities.tblItems
                    .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.ItemName == item.ItemName)
                    .FirstOrDefault();

                if (tblItem != null) { return false; }
                else
                {
                    tblItem = new tblItem();
                    tblItem.ItemGuid = Guid.NewGuid();
                    tblItem.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
                    tblItem.ItemName = item.ItemName;
                    tblItem.FK_CategoryGuid = item.CategoryGuid;
                    tblItem.FK_BusinessGuid = item.BusinessGuid;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
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
