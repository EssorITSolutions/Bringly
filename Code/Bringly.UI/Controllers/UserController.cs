using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;
using Bringly.DomainLogic;
namespace Bringly.UI.Controllers
{
    public class UserController : BaseClasses.AuthoriseUserControllerBase
    {
        public ActionResult PersonalInformation()
        {
            UserDomainLogic userProfile = new UserDomainLogic();
            return View(userProfile.FindUser(Guid.Parse("9327aedc-65a4-4f53-87d4-be94a3bb91a3")));
        }
        [HttpGet]
        public ActionResult EditPersonalInformation()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            UserProfile userProfile = userDomainLogic.FindUser(Guid.Parse("9327aedc-65a4-4f53-87d4-be94a3bb91a3"));
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            return View(userProfile);
        }
        [HttpPost]
        public ActionResult EditPersonalInformation(UserProfile userProfile)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userProfile.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            usrAddress.UserAddressGuid = Guid.Parse(HttpContext.Request.Form["BillingAddress_UserAddressGuid"]);
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress_Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress_PostCode"]);
            usrAddress.CityGuid = Guid.Parse(HttpContext.Request.Form["BillingAddress_CityGuid"]);
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            usrAddress = new UserAddress();
            usrAddress.UserAddressGuid = Guid.Parse(HttpContext.Request.Form["ShippingAddress_UserAddressGuid"]);
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["ShippingAddress_Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["ShippingAddress_PostCode"]);
            usrAddress.CityGuid = Guid.Parse(HttpContext.Request.Form["ShippingAddress_CityGuid"]);
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            userdomainLogic.UpdateUserProfile(userProfile);
            Success("saved sucessfully");
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            return View(userProfile);
        }

        public ActionResult UpdatePreferedCity(Guid CityGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            userDomainLogic.UpdatePreferedCity(CityGuid);
            return null;
        }
        [HttpPost]
        public JsonResult AddToFavourite(string restaurantGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.AddFavourite(Guid.Parse(restaurantGuid)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveFavourite(Guid restaurantGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.RemoveFavourite(restaurantGuid), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Favourites()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.FavouriteRestaurants());
        }
        public ActionResult partialLeftPanel()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View("_LeftPanel",userDomainLogic.FindUser(Bringly.UserVariables.LoggedInUserId));
        }
    }
}