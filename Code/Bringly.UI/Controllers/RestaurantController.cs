using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.DomainLogic;
using Bringly.Domain;
using Bringly.DomainLogic.User;
using Bringly.Domain.User;

namespace Bringly.UI.Controllers
{
    public class RestaurantController : BaseClasses.AuthoriseUserControllerBase
    {
        // GET: Restaurant
        public ActionResult City(string id)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            CommonDomainLogic _commonDomainLogic = new CommonDomainLogic();
            ChooseCity chooseCity = new ChooseCity();
            if (!string.IsNullOrEmpty(id))
            {
                Guid _cityguid = _commonDomainLogic.FindCityGuid(id);
                UserDomainLogic _userDomainLogic = new UserDomainLogic();
                _userDomainLogic.UpdatePreferedCity(_cityguid);
                chooseCity.Cities = _commonDomainLogic.GetCityByGUID(_cityguid);
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }
            else
            {
                chooseCity.Cities = _commonDomainLogic.GetCities();
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }

            return View("ListRestaurants", restaurantDomainLogic.GetRestaurantsByCity(chooseCity.SelectedCity));
        }
        public ActionResult MyRestaurant() {
            return View();
        }
        public ActionResult RestaurantItems(Nullable<Guid> RestaurantGuid)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            RestaurantGuid = new Guid("7B9D0CFF-F15F-49CA-95EF-EA81CDEA4E34");
            List<Items> itemslist = new List<Items>();
            itemslist=restaurantDomainLogic.GetItemsByRestaurantGuid(RestaurantGuid.HasValue?RestaurantGuid.Value:Guid.Empty);
            return View(itemslist);
        }
        [HttpPost]
        public ActionResult addToCart(Items item)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.addToCart(item),JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCartCount()
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.getCartCount(),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deleteItemFromCart(Guid ItemGuid)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.deleteItemFromCart(ItemGuid), JsonRequestBehavior.AllowGet);
        }
    }
}