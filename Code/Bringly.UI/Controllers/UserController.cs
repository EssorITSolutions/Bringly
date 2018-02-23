using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;
using Bringly.DomainLogic;
using System.IO;
using System.Web.Security;
using Bringly.Domain;
using System.Web.UI;

namespace Bringly.UI.Controllers
{
    public class UserController : BaseClasses.AuthoriseUserControllerBase
    {
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult PersonalInformation()
        {
            UserDomainLogic userProfile = new UserDomainLogic();
            return View(userProfile.FindUser(UserVariables.LoggedInUserGuid));
        }
        [HttpGet]
        public ActionResult EditPersonalInformation()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            UserProfile userProfile = userDomainLogic.FindUser(UserVariables.LoggedInUserGuid);
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
            Success("Saved successfully");
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
        public JsonResult AddToFavourite(string restaurantGuid, string IsFavourite)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.AddFavourite(Guid.Parse(restaurantGuid), IsFavourite), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveFavourite(Guid restaurantGuid, string IsFavourite)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.RemoveFavourite(restaurantGuid, IsFavourite), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Favourites()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.FavouriteRestaurants());
        }
        public ActionResult partialLeftPanel()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View("_LeftPanel", userDomainLogic.FindUser(UserVariables.LoggedInUserGuid));
        }

        public ActionResult UploadProfileImage(FormCollection frm)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string imageName = "";
                    if (Request.Files.Count > 0)
                    {
                        UserDomainLogic userdomainLogic = new UserDomainLogic();
                        imageName = userdomainLogic.UpdateProfileImage(Request);
                    }
                    return Json(new { NewImage = imageName, Message = "File uploaded successfully", IsSuccess = true });
                }
                catch (Exception ex)
                {
                    return Json(new { NewImage = "", Message = "Error occurred. Error details: " + ex.Message, IsSuccess = false });
                }
            }
            else
            {
                return Json(new { NewImage = "", Message = "No image selected.", IsSuccess = false });
            }

        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RestaurantReview()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetMyReviewBuyer(UserVariables.LoggedInUserGuid));
        }
        //[HttpPost]
        //public ActionResult RestaurantReview(MyReview myReview, string Command)
        //{
        //    UserDomainLogic userDomainLogic = new UserDomainLogic();
        //    if (Command == "Leave Review")
        //    {
        //        myReview.IsSkipped = false;
        //        userDomainLogic.InsertReview(myReview);
        //        Success("Review saved successfully");
        //    }
        //    else
        //    {
        //        myReview.IsSkipped = true;
        //        userDomainLogic.InsertReview(myReview);
        //        Success("Review skipped successfully");
        //    }            
        //    return View(userDomainLogic.InsertReview(myReview));
        //}

        public ActionResult AddReview(Guid ReviewGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return PartialView("_AddReview", userDomainLogic.GetReviewByGuid(ReviewGuid));
        }
        [HttpPost]
        public ActionResult AddReview(MyReview myReview)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            if (ModelState.IsValid)
            {                
                myReview.IsSkipped = false;
                userDomainLogic.InsertReview(myReview);
                ViewBag.IspopUp = true;               
            }
            return PartialView("_AddReview", userDomainLogic.GetReviewByGuid(myReview.ReviewGuid));
        }

        public bool SkipReview(Guid reviewguid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            MyReview myReview = new MyReview();
            myReview.Review = "";
            myReview.IsSkipped = true;
            myReview.ReviewGuid = reviewguid;
            myReview=userDomainLogic.InsertReview(myReview);                    
            return true;
        }
        [HttpPost]
        public bool ApproveReview(Guid reviewguid,string Isapprove)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return userDomainLogic.ApproveReviewLogic(reviewguid, Convert.ToBoolean(Isapprove));
        }
        public ActionResult MerchantReview()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetMyReviewMerchant(UserVariables.LoggedInUserGuid));
        }
    }
}