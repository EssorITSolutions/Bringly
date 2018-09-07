using Bringly.Domain;
using Bringly.Domain.Common;
using Bringly.Domain.Enums;
using Bringly.Domain.User;
using Bringly.DomainLogic;
using Bringly.DomainLogic.Business;
using Bringly.DomainLogic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Utilities;

namespace Bringly.UI.Controllers
{
    public class UserController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            Dashboard dashboard = new Dashboard();
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            WalletDomainLogic walletDomainLogic = new WalletDomainLogic();
            dashboard.MyMessage = userdomainLogic.GetMyMessages(UserVariables.LoggedInUserGuid);

            if (UserVariables.UserRole == Bringly.Domain.Enums.User.UserRoles.Buyer)
                dashboard.MyFavourites = userdomainLogic.FavouriteLocations(2);

            else dashboard.MyFavourites = null;

            if (UserVariables.UserRole == Bringly.Domain.Enums.User.UserRoles.Merchant)
            {
                MyReview myreview = new MyReview();
                myreview.UserGuid = UserVariables.LoggedInUserGuid;
                dashboard.MyReview = userdomainLogic.GetMyReviewMerchant(myreview);
                dashboard.MyReview.GivenBusinessReviews = dashboard.MyReview.GivenBusinessReviews.Count > 0 ? dashboard.MyReview.GivenBusinessReviews.Take(2).ToList() : dashboard.MyReview.GivenBusinessReviews;
            }

            dashboard.Wallet = walletDomainLogic.GetWallet(UserVariables.LoggedInUserGuid);
            dashboard.MyOrder = orderDomainLogic.GetMyOrderCounts(UserVariables.LoggedInUserGuid, UserVariables.UserRole);

            return View(dashboard);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInformation()
        {
            UserDomainLogic userProfile = new UserDomainLogic();
            if (TempData["Success"] != null)
                Success(TempData["Success"].ToString());
            return View(userProfile.FindUser(UserVariables.LoggedInUserGuid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPersonalInformation()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            UserProfile userProfile = userDomainLogic.FindUser(UserVariables.LoggedInUserGuid);
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            return View(userProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPersonalInformation(UserProfile userProfile)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userProfile.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            Guid cityGuid = Guid.Empty;
            usrAddress.UserAddressGuid = Guid.Parse(HttpContext.Request.Form["BillingAddress_UserAddressGuid"]);
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress.PostCode"]);
            Guid.TryParse(HttpContext.Request.Form["BillingAddress.CityGuid"], out cityGuid);
            usrAddress.CityGuid = cityGuid;
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();
            if (usrAddress.CityGuid != Guid.Empty)
                userProfile.UserAddresses.Add(usrAddress);
            usrAddress = new UserAddress();
            usrAddress.UserAddressGuid = Guid.Parse(HttpContext.Request.Form["ShippingAddress_UserAddressGuid"]);
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["ShippingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["ShippingAddress.PostCode"]);
            Guid.TryParse(HttpContext.Request.Form["ShippingAddress.CityGuid"], out cityGuid);
            usrAddress.CityGuid = cityGuid;
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            userdomainLogic.UpdateUserProfile(userProfile);
            TempData["Success"] = "Saved successfully";
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();

            return RedirectToAction("PersonalInformation");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CityGuid"></param>
        /// <returns></returns>
        public ActionResult UpdatePreferedCity(Guid CityGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            userDomainLogic.UpdatePreferedCity(CityGuid);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurantGuid"></param>
        /// <param name="IsFavourite"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddToFavourite(string restaurantGuid, string IsFavourite)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.AddFavourite(Guid.Parse(restaurantGuid), IsFavourite), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurantGuid"></param>
        /// <param name="IsFavourite"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveFavourite(Guid restaurantGuid, string IsFavourite)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.RemoveFavourite(restaurantGuid, IsFavourite), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Favourites()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.FavouriteLocations());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult partialLeftPanel()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View("_LeftPanel", userDomainLogic.FindUser(UserVariables.LoggedInUserGuid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RestaurantReview()
        {
            if (TempData["IspopUp"] != null)
            {
                Success("Review added successfully");
            }

            int Currentpage = 0;
            TempData["CurrentPage"] = null;

            if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
            {
                TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                TempData.Keep();
            }

            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetMyReviewBuyer(UserVariables.LoggedInUserGuid, Currentpage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReviewGuid"></param>
        /// <returns></returns>
        public ActionResult AddReview(Guid ReviewGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var data = userDomainLogic.GetReviewByGuid(ReviewGuid);
            ViewBag.PopupTitle = "Add Review for " + data.BusinessName;
            return PartialView("_AddReview", data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myReview"></param>
        /// <returns></returns>
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

            var data = userDomainLogic.GetReviewByGuid(myReview.ReviewGuid);
            ViewBag.PopupTitle = "Add Review for " + data.BusinessName;

            return PartialView("_AddReview", data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewguid"></param>
        /// <returns></returns>
        public bool SkipReview(Guid reviewguid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            MyReview myReview = new MyReview();
            myReview.Review = "";
            myReview.IsSkipped = true;
            myReview.ReviewGuid = reviewguid;
            myReview = userDomainLogic.InsertReview(myReview);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewguid"></param>
        /// <param name="Isapprove"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveReview(Guid reviewguid, string Isapprove)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.ApproveReviewLogic(reviewguid, Convert.ToBoolean(Isapprove)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myreview"></param>
        /// <returns></returns>
        public ActionResult MerchantReview(MyReview myreview)
        {
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Merchant || UserVariables.UserRole == Domain.Enums.User.UserRoles.Manager)
            {
                int Currentpage = 0;
                TempData["CurrentPage"] = null;

                if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
                {
                    TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                    Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                    TempData.Keep();
                }

                UserDomainLogic userDomainLogic = new UserDomainLogic();
                myreview.UserGuid = UserVariables.LoggedInUserGuid;
                return View(userDomainLogic.GetMyReviewMerchant(myreview, Currentpage));
            }
            else
            {
                //Unauthorize unauthorize = new Unauthorize();
                //string url = Request.Url.AbsoluteUri;
                //unauthorize.ReturnUrl = url.Split(new[] { '?' })[0];
                return View("UnauthorizedAccess");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAllBuyers()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return userDomainLogic.GetAllBuyers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NewRegistrationfromSocialPage()
        {
            bool isFromSocialMedia = Convert.ToBoolean(QueryStringHelper.getValue("sm"));
            if (!isFromSocialMedia)
            {
                isFromSocialMedia = TempData["isFromSocialMedia"] == null ? false : (bool)TempData["isFromSocialMedia"];
            }
            ViewBag.isFromSocialMedia = isFromSocialMedia;
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            UserProfile userProfile = userDomainLogic.FindUser(UserVariables.LoggedInUserGuid);
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.BusinessTypeList = businessDomainLogic.GetBusinessTypes();
            userProfile.Cities = commonDomainLogic.GetCities();
            userProfile.Cities.Add(new Domain.City { CityGuid = Guid.Empty, CityName = "Other", CityUrlName = "Other" });
            ViewBag.isFromSocialMedia = isFromSocialMedia;
            return View(userProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewRegistrationfromSocialPage(UserProfile userProfile)
        {
            Guid cityGuid = Guid.Empty;
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userProfile.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress.PostCode"]);
            Guid.TryParse(HttpContext.Request.Form["BillingAddress_CityGuid"], out cityGuid);
            usrAddress.CityGuid = cityGuid;
            var billingCityName = Convert.ToString(HttpContext.Request.Form["BillingAddress.CityName"]);
            var shippingCityName = Convert.ToString(HttpContext.Request.Form["ShippingAddress.CityName"]);

            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();

            if (!string.IsNullOrEmpty(billingCityName) && usrAddress.CityGuid == Guid.Empty)
            {
                Bringly.Domain.City city = new Domain.City
                {
                    CityGuid = Guid.NewGuid(),
                    CityName = billingCityName,
                    CityUrlName = billingCityName
                };

                commonDomainLogic.AddCity(city);
                usrAddress.CityGuid = city.CityGuid;
            }

            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();

            userProfile.UserAddresses.Add(usrAddress);
            usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["ShippingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["ShippingAddress.PostCode"]);
            Guid.TryParse(HttpContext.Request.Form["ShippingAddress_CityGuid"], out cityGuid);
            usrAddress.CityGuid = cityGuid;

            if (!string.IsNullOrEmpty(shippingCityName) && usrAddress.CityGuid == Guid.Empty && !billingCityName.Equals(shippingCityName))
            {
                Bringly.Domain.City city = new Domain.City
                {
                    CityGuid = Guid.NewGuid(),
                    CityName = shippingCityName,
                    CityUrlName = shippingCityName
                };

                commonDomainLogic.AddCity(city);
                usrAddress.CityGuid = city.CityGuid;
            }

            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString();
            userProfile.UserAddresses.Add(usrAddress);
            userdomainLogic.UpdateUserProfile(userProfile);
            Success("Saved successfully");
            userProfile.Cities = commonDomainLogic.GetCities();

            return RedirectToAction("Dashboard", "User");
        }

        #region User

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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
            else
            {
                userProfile.Cities = commonDomainLogic.GetCities();
                userProfile.RolesList = commonDomainLogic.GetAllRoles();
            }

            return View(userProfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEditManager(UserProfile userProfile)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userdomainLogic.AddUpdateUser(userProfile);
            TempData["Success"] = "Saved successfully";
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userProfile.Cities = commonDomainLogic.GetCities();
            userProfile.RolesList = commonDomainLogic.GetAllRoles();

            return RedirectToAction("ManageManagers");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageManagers()
        {
            ManageUserProfiles manageUserProfiles = new ManageUserProfiles();
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            if (TempData["Success"] != null)
                Success(TempData["Success"].ToString());
            manageUserProfiles.UserProfiles = userdomainLogic.GetAllUsers();
            return View(manageUserProfiles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound", "No user(s) found.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userRegistrationType"></param>
        /// <param name="businessGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult FindUsertoberegistered(string email, int userRegistrationType, Guid? businessGuid)
        {
            if (businessGuid == null)
            { businessGuid = Guid.Empty; }
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            var userProfileData = userdomainLogic.FindUsertoberegistered(email, userRegistrationType, businessGuid);

            return Json(new { userProfile = userProfileData, isExists = userProfileData.UserGuid == Guid.Empty ? false : true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gel html view for settings
        /// </summary>
        /// <returns>Html view foe setting page.</returns>
        [HttpGet]
        [ActionName("settings")]
        public ActionResult MerchantSettings()
        {
            ViewBag.isPopUpOpen = TempData["isPopUpOpen"];

            if (TempData["hasError"] != null && (bool)TempData["hasError"] == true)
            {
                ErrorBlock(Convert.ToString(TempData["message"]));
            }

            else if (TempData["hasError"] != null && (bool)TempData["hasError"] == false)
            {
                Success(Convert.ToString(TempData["message"]));
            }

            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Merchant)
            {
                UserDomainLogic userDomainLogic = new UserDomainLogic();
                return View("Settings", userDomainLogic.IsSelfDeliveryActive(UserVariables.LoggedInUserGuid));
            }
            else if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
            {
                return View("Settings", false);
            }
            else
            {
                //unauthorizes

                return View("UnauthorizedAccess");
            }
        }

        /// <summary>
        /// Set self delvery option for the business.
        /// </summary>
        /// <param name="isSelfDelivery">true to "on" self delivery else false.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("setselfdelivery")]
        public ActionResult SetSelfDelivery(bool isSelfDelivery)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.SetSelfDeliveryForMerchant(UserVariables.LoggedInUserGuid, isSelfDelivery), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get html view th chnage user's account password.
        /// </summary>
        /// <returns>Html view to changes user's account password.</returns>
        [HttpGet]
        [ActionName("changepassword")]
        public ActionResult ChangePassword()
        {
            ViewBag.PopupTitle = "Change Password";
            return View("_ChangePassword");
        }

        /// <summary>
        /// Change user acccount password.
        /// </summary>
        /// <param name="cPw">Current password.</param>
        /// <param name="nPw">New password.</param>
        /// <returns>true of success else false.</returns>
        [HttpPost]
        [ActionName("changepassword")]
        public ActionResult ChangePassword(string cPw, string nPw)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var result = userDomainLogic.ChangePassword(cPw, nPw, UserVariables.LoggedInUserGuid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get html for de-active user account.
        /// </summary>
        /// <returns>Html view to de-activate user account.</returns>
        [HttpGet]
        [ActionName("deactivateaccount")]
        public ActionResult DeActivateAccount()
        {
            ViewBag.PopupTitle = "De-Activate Account";
            return View("DeactivateAccount");
        }

        /// <summary>
        /// De-activate user account.
        /// </summary>
        /// <param name="password">User account password to be de-activated.</param>
        /// <returns>true if success else false.</returns>
        [HttpPost]
        [ActionName("deactivateaccount")]
        public ActionResult DeActivateAccount(string password)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var result = userDomainLogic.DeActivateAccount(password, UserVariables.LoggedInUserGuid);
            if (result)
            {
                FormsAuthentication.SignOut();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get delete user account html view
        /// </summary>
        /// <returns>Html view to delete user  account.</returns>
        [HttpGet]
        [ActionName("deleteaccount")]
        public ActionResult DeleteAccount()
        {
            ViewBag.PopupTitle = "Delete Account";
            return View("_DeleteAccount");
        }
        
        /// <summary>
        /// Delete user account
        /// </summary>
        /// <param name="password">Password of the user account to be deleted.</param>
        /// <returns>true if success else false.</returns>
        [HttpPost]
        [ActionName("deleteaccount")]
        public ActionResult DeleteAccount(string password)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var result = userDomainLogic.DeleteAccount(password, UserVariables.LoggedInUserGuid);
            if (result)
            {
                FormsAuthentication.SignOut();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}