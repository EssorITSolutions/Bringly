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
using Bringly.Domain.Common;
using Bringly.Domain.Enums;

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
            // return RedirectToAction("EditPersonalInformation", "User");
            return RedirectToAction("PersonalInformation");
            //return View(userProfile);
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
            return View(userDomainLogic.FavouriteLocations());
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
            if (TempData["IspopUp"] != null) {
                Success("Review added successfully");
            }
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
                TempData["IspopUp"] = true;
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
        public ActionResult ApproveReview(Guid reviewguid,string Isapprove)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.ApproveReviewLogic(reviewguid, Convert.ToBoolean(Isapprove)),JsonRequestBehavior.AllowGet);
        }
        public ActionResult MerchantReview(MyReview myreview)
        {
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Merchant)
            {
                UserDomainLogic userDomainLogic = new UserDomainLogic();
                myreview.UserGuid = UserVariables.LoggedInUserGuid;
                return View(userDomainLogic.GetMyReviewMerchant(myreview));
            }
            else {
                //Unauthorize unauthorize = new Unauthorize();
                //string url = Request.Url.AbsoluteUri;
                //unauthorize.ReturnUrl = url.Split(new[] { '?' })[0];
                return View("UnauthorizedAccess");
            }
        }

        public List<Contact> GetAllBuyers()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return userDomainLogic.GetAllBuyers();
        }

        public ActionResult NewRegistrationfromSocialPage()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            UserProfile userProfile = userDomainLogic.FindUser(UserVariables.LoggedInUserGuid);
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            return View(userProfile);
        }
        [HttpPost]
        public ActionResult NewRegistrationfromSocialPage(UserProfile userProfile)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userProfile.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress.PostCode"]);
            usrAddress.CityGuid = Guid.Parse(HttpContext.Request.Form["BillingAddress_CityGuid"]);
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["ShippingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["ShippingAddress.PostCode"]);
            usrAddress.CityGuid = Guid.Parse(HttpContext.Request.Form["ShippingAddress_CityGuid"]);
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            userdomainLogic.UpdateUserProfile(userProfile);
            Success("Saved successfully");
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            return RedirectToAction("Dashboard", "User");
            //return View(userProfile);
        }

        #region User

        public ActionResult AddEditManager(Nullable<Guid> guid)
        {
            UserProfile userProfile = new UserProfile();
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            if (guid != null && guid != Guid.Empty)
            {
                userProfile = userDomainLogic.FindUser(guid.Value);
                userProfile.Cities = commonDomainLogic.GetCities();
                userProfile.RolesList = commonDomainLogic.GetAllRoles();
            }
            else {
                userProfile.Cities = commonDomainLogic.GetCities();
                userProfile.RolesList = commonDomainLogic.GetAllRoles();
            }

            return View(userProfile);
        }
        [HttpPost]
        public ActionResult AddEditManager(UserProfile userProfile)
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
            userProfile.UserAddresses.Add(usrAddress);
            userdomainLogic.AddUpdateUser(userProfile);
            Success("Saved successfully");
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            userProfile.RolesList = commonDomainLogic.GetAllRoles();
            //return RedirectToAction("ManageManagers", "User");
            return RedirectToAction("ManageManagers");
        }

        public ActionResult ManageManagers()
        {
            ManageUserProfiles manageUserProfiles = new ManageUserProfiles();
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            manageUserProfiles.UserProfiles = userdomainLogic.GetAllUsers();
            return View(manageUserProfiles);
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid UserGuid)
        {
            Message message = new Message();
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            if (userdomainLogic.DeleteUser(UserGuid))
            {
                ModelState.Clear();
                message.MessageText = "User has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "User deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound", "No user(s) found.");
        }

        [HttpPost]
        public ActionResult FindUsertoberegistered(string email, int UserRegistrationType, Guid BusinessGuid)
        {
            if(BusinessGuid==null)
            { BusinessGuid = Guid.Empty; }
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            return Json(userdomainLogic.FindUsertoberegistered(email, UserRegistrationType, BusinessGuid),JsonRequestBehavior.AllowGet);
            //return View(userProfile);
        }

        #endregion
    }
}