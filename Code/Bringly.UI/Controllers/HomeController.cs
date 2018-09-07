using Bringly.Domain;
using Bringly.Domain.Common;
using Bringly.Domain.User;
using Bringly.DomainLogic;
using Bringly.DomainLogic.Business;
using Bringly.DomainLogic.User;
using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class HomeController : BaseClasses.AnonymousUserControllerBase
    {
        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Login user interface
        /// </summary>
        /// <param name="returnUrl">return url to be re-directred</param>
        /// <returns>redirect after login to dishboard or the given return url</returns>
        public ActionResult Login(string returnUrl)
        {
            return UserVariables.IsAuthenticated ? RedirectAfterLogin(returnUrl) : View();
        }

        /// <summary>
        /// Get bringly user's agreement.
        /// </summary>
        /// <returns>Html view for agreement page.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ActionName("getuseragreement")]
        public ActionResult GetUserAgreement()
        {
            ViewBag.PopupTitle = "Bringly terms and conditions";
            return PartialView("_UserAgreement");
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="userLogin">User name and password</param>
        /// <param name="returnUrl">return url to be re-directred</param>
        /// <returns>redirect after login to dishboard or the given return url</returns>
        [HttpPost]
        public ActionResult Login(UserLogin userLogin, string returnUrl)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = userDomainLogic.UserLoginWithHash(userLogin);
            if (message.MessageType == Domain.Enums.MessageType.NewUser)
            {
                return RedirectToAction("NewRegistrationfromSocialPage", "User");
            }
            else if (message.MessageType == Domain.Enums.MessageType.Success)
            {
                return RedirectAfterLogin(returnUrl);
            }
            else
            {
                ErrorBlock(message.MessageText);
                return View(userLogin);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new UserRegistration());
        }

        /// <summary>
        /// Send the passowrd to the given user email address
        /// </summary>
        /// <param name="userRegistration">email address field </param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult ForgotPassword(UserRegistration userRegistration)
        {
            if (string.IsNullOrEmpty(userRegistration.EmailAddress))
                return View("ForgetPasswordNotFound");

            EmailDomainLogic emailDomainLogic = new EmailDomainLogic();
            bool isSent = emailDomainLogic.SendUserPassword(userRegistration.EmailAddress);
            if (isSent)
                return View("SentForgetPassword");
            ErrorBlock("Email address not found.");
            return View(new UserRegistration());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserRegistration()
        {
            UserRegistration userregistration = new Domain.User.UserRegistration();
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            userregistration.Cities = userDomainLogic.GetCities();
            userregistration.Cities.Add(new Domain.City { CityGuid = Guid.Empty, CityName = "Other", CityUrlName = "Other" });
            userregistration.Countries = commonDomainLogic.GetCountryList();
            userregistration.BusinessTypeList = businessDomainLogic.GetBusinessTypes();
            return View(userregistration);
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="userRegistration">User registration logic</param>
        /// <returns>Respective logic based on the logic</returns>
        [HttpPost]
        public ActionResult UserRegistration(UserRegistration userRegistration)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userRegistration.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress.PostCode"]);
            usrAddress.CityGuid = userRegistration.BillingAddress.CityGuid;

            usrAddress.Country = userRegistration.BillingAddress.Country;
            usrAddress.Latitude = userRegistration.BillingAddress.Latitude;
            usrAddress.Longitude = userRegistration.BillingAddress.Longitude;
            usrAddress.PlaceId = userRegistration.BillingAddress.PlaceId;

            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();
            var cityName = Convert.ToString(HttpContext.Request.Form["BillingAddress.CityName"]);

            if (!string.IsNullOrEmpty(cityName) && usrAddress.CityGuid == Guid.Empty)
            {
                Bringly.Domain.City city = new Domain.City
                {
                    CityGuid = Guid.NewGuid(),
                    CityName = cityName,
                    CityUrlName = cityName
                };

                CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
                usrAddress.CityGuid = commonDomainLogic.AddCity(city);
            }

            userRegistration.UserAddresses.Add(usrAddress);
            bool result = userdomainLogic.AddUserProfile(userRegistration);

            if (result)
            {
                return RedirectToAction("thankregistration", "home");
            }

            return RedirectToAction("userregistration", "home");
        }

        /// <summary>
        /// Emaill verification
        /// </summary>
        /// <param name="id">User guid to be verify</param>
        /// <param name="type">User RegistrationType</param>
        /// <returns></returns>
        public ActionResult Verification(string id, int type = 2)
        {
            Guid guid = Guid.Empty;
            bool isVerified = false;
            if (!string.IsNullOrEmpty(id))
            {
                UserDomainLogic userDomainLogic = new UserDomainLogic();
                Guid.TryParse(id, out guid);

                isVerified = userDomainLogic.VerifyUser(guid);
                if (isVerified)
                {
                    return View("ThankUConfirmation", guid);
                }
            }

            return RedirectToAction("verificationexpired", "home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGoogleMap()
        {
            return PartialView("_GoogleMap", ConfigurationManager.AppSettings["GoogleMapKey"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult CreatePassword(Guid guid, string password)
        {
            if (string.IsNullOrEmpty(password))
                return View("ThankUConfirmation", guid);

            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var userRegistrationType = userDomainLogic.CreateUserPassword(guid, password);
            if (userRegistrationType > 0)
            {
                if (userRegistrationType == 3)
                {
                    return View("ThanksPasswordCreated", true);
                }
                else
                {
                    return View("ThanksPasswordCreated", false);
                }
            }
            return RedirectToAction("", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ThankRegistration()
        {
            return View("ThankURegistration");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult VerificationExpired()
        {
            return View("VerificationExpired");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyEmailAddress()
        {
            return View("VerifyEmaillAddressError");
        }

        #region S o c i a l   L o g i n

        #region G o o g l e

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (ProviderName == null || ProviderName == "")
            {
                NameValueCollection nvs = Request.QueryString;
                if (nvs.Count > 0)
                {
                    if (nvs["state"] != null)
                    {
                        NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
                        if (provideritem["__provider__"] != null)
                        {
                            ProviderName = provideritem["__provider__"];
                        }
                    }
                }
            }

            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Account", "Login"));
            }

            // User has logged in with provider successfully
            // Check if user is already registered locally
            //You can call you user data access method to check and create users based on your model

            var id = authResult.ExtraData["id"];
            string email = authResult.ExtraData["email"];
            var name = authResult.ExtraData["name"];
            var picture = authResult.ExtraData["picture"];

            SocialLogin socialLogin = new SocialLogin();
            socialLogin.EmailAddress = email;
            socialLogin.FullName = name;
            socialLogin.IsGoogleLogin = true;
            socialLogin.GoogleProviderUserId = id;
            socialLogin.GoogleUserProfileImageUrl = picture;
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            var message = userDomainLogic.RegisterUserthroughSociallogin(socialLogin);
            if (message.MessageType == Domain.Enums.MessageType.Success)
            {
                return RedirectAfterLogin(returnUrl);
            }
            else if (message.MessageType == Domain.Enums.MessageType.NewUser)
            {
                TempData["isFromSocialMedia"] = true;
                return RedirectToAction("NewRegistrationfromSocialPage", "User", new { sm = true });
            }
            else
            {
                ErrorBlock(message.MessageText);
                return View("Login");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OpenAuth.RequestAuthentication(Provider, ReturnUrl);
            }
        }
        #endregion

        #region F a c e b o o k 

        /// <summary>
        /// 
        /// </summary>
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Redirect(userDomainLogic.FacebookLogin(RedirectUri));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult FacebookCallback(string code)
        {
            var fb = new Facebook.FacebookClient();
            dynamic accessToken = "";
            try
            {
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["AppId"],
                    client_secret = ConfigurationManager.AppSettings["AppSecret"],
                    redirect_uri = RedirectUri.AbsoluteUri.Replace("http", "https"),
                    code = code
                });
                accessToken = result.access_token;
            }
            catch (Exception) { }

            // Store the access token in the session for farther use
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information
            dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
            string email = me.email;
            string firstname = me.first_name;
            string middlename = me.middle_name;
            string lastname = me.last_name;

            SocialLogin socialLogin = new SocialLogin();
            socialLogin.EmailAddress = email;
            socialLogin.FullName = firstname + (string.IsNullOrEmpty(lastname) ? "" : " " + lastname);
            socialLogin.IsGoogleLogin = false;
            socialLogin.FacebookProviderUserId = Convert.ToString(me.id);

            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = userDomainLogic.RegisterUserthroughSociallogin(socialLogin);
            if (message.MessageType == Domain.Enums.MessageType.Success)
            {
                return RedirectToAction("Dashboard", "User");
            }
            else if (message.MessageType == Domain.Enums.MessageType.NewUser)
            {
                TempData["isFromSocialMedia"] = true;
                return RedirectToAction("NewRegistrationfromSocialPage", "User", new { sm = true });
            }
            else
            {
                ErrorBlock(message.MessageText);
                return View("Login");
            }

            //// Set the auth cookie
            //FormsAuthentication.SetAuthCookie(email, false);
            //return RedirectToAction("Dashboard", "User");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderCMSPageLinks()
        {
            CMSDomainLogic cMSDomainLogic = new CMSDomainLogic();
            var cMSPages = cMSDomainLogic.GetCMSPages();
            return View(cMSPages);
        }

        [HttpGet]
        [ActionName("termsconditions")]
        public ActionResult GetTermsAndConditions()
        {
            CMSDomainLogic cMSDomainLogic = new CMSDomainLogic();
            string pageAlieas = "T-C";
            var content = cMSDomainLogic.GetCmsPageContent(pageAlieas);
            if (content == null)
                return RedirectTo404();
            content.PageDescription = System.Net.WebUtility.HtmlDecode(content.PageDescription);

            return View("TermsConditions", content);
        }

        [HttpGet]
        [ActionName("aboutus")]
        public ActionResult AboutUs()
        {
            CMSDomainLogic cMSDomainLogic = new CMSDomainLogic();
            string pageAlieas = "about-us";
            var content = cMSDomainLogic.GetCmsPageContent(pageAlieas);
            if (content == null)
                return RedirectTo404();
            content.PageDescription = System.Net.WebUtility.HtmlDecode(content.PageDescription);

            return View("AboutUS", content);
        }

        [HttpGet]
        [ActionName("contactus")]
        public ActionResult ContactUs()
        {
            return View("ContactUs");
        }

        [HttpPost]
        [ActionName("contactus")]
        public ActionResult ContactUs(ComposeEmail composeEmail)
        {
            if (!ModelState.IsValid)
            {
                return View("ContactUs", composeEmail);
            }

            if (!CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid captcha. Please try again."))
            {
                ViewBag.CaptchaErrMessage = "Invalid captcha. Please try again.";
                return View("ContactUs", composeEmail);
            }

            EmailDomainLogic email = new EmailDomainLogic();
            var result = email.SendContactUsMessage(composeEmail);
            if (result)
                Success("Message has been sent.");
            else
                ErrorBlock("Internal problem.");

            ModelState.Clear();

            return View("ContactUs", composeEmail);

        }

        #endregion

        #endregion


    }
}