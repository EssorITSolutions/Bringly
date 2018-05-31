using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;
using Bringly.Domain.Common;
using Bringly.DomainLogic;
using Microsoft.AspNet.Membership.OpenAuth;
using DotNetOpenAuth.GoogleOAuth2;
using System.Collections.Specialized;
using System.Web.Security;
using OAuth;
using DotNetOpenAuth.AspNet.Clients;
using Facebook;
using System.Configuration;
using Bringly.DomainLogic.Business;

namespace Bringly.UI.Controllers
{
    public class HomeController : BaseClasses.AnonymousUserControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string returnUrl)
        {
            return UserVariables.IsAuthenticated ? RedirectAfterLogin(returnUrl) : View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin userLogin, string returnUrl)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = userDomainLogic.UserLogin(userLogin);
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
        [HttpPost]
        public ActionResult ForgotPassword(UserRegistration userRegistration)
        {

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
            userregistration.Cities = userDomainLogic.GetCities();
            userregistration.BusinessTypeList = businessDomainLogic.GetBusinessTypes();
            return View(userregistration);
        }
        [HttpPost]
        public ActionResult UserRegistration(UserRegistration userRegistration)
        {
            UserDomainLogic userdomainLogic = new UserDomainLogic();
            userRegistration.UserAddresses = new List<UserAddress>();
            UserAddress usrAddress = new UserAddress();
            usrAddress.Address = Convert.ToString(HttpContext.Request.Form["BillingAddress.Address"]);
            usrAddress.PostCode = Convert.ToString(HttpContext.Request.Form["BillingAddress.PostCode"]);
            usrAddress.CityGuid = Guid.Parse(HttpContext.Request.Form["BillingAddress.CityGuid"]);
            usrAddress.AddressType = Bringly.Domain.Enums.User.UserAddressType.Billing.ToString();
            userRegistration.UserAddresses.Add(usrAddress);
            userdomainLogic.AddUserProfile(userRegistration);
            Success("Saved successfully");
            if (userRegistration.UserRegistrationType == "3")
            {
                return RedirectToAction("EditPersonalInformation", "User");
            }
            else
            {
                return RedirectToAction("Dashboard", "User");
            }
        }

        #region Social Login

        #region Google

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
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = userDomainLogic.RegisterUserthroughSociallogin(socialLogin);
            if (message.MessageType == Domain.Enums.MessageType.Success)
            {
                return RedirectAfterLogin(returnUrl);
            }
            else if (message.MessageType == Domain.Enums.MessageType.NewUser)
            {
                return RedirectToAction("NewRegistrationfromSocialPage", "User");
            }
            else
            {
                ErrorBlock(message.MessageText);
                return View("Login");
            }
        }

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

        #region Facebook
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
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Redirect(userDomainLogic.FacebookLogin(RedirectUri));
        }
        public ActionResult FacebookCallback(string code)
        {            
            var fb = new Facebook.FacebookClient();
            dynamic accessToken="";
            try
            {
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["AppId"],
                    client_secret = ConfigurationManager.AppSettings["AppSecret"],
                    redirect_uri = RedirectUri.AbsoluteUri.Replace("http","https"),
                    code = code
                });
                accessToken = result.access_token;
            }
            catch (Exception ex) { }

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
            socialLogin.FullName = firstname + (string.IsNullOrEmpty(lastname)?"":" "+ lastname);
            socialLogin.IsGoogleLogin = false;
         
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = userDomainLogic.RegisterUserthroughSociallogin(socialLogin);
            if (message.MessageType == Domain.Enums.MessageType.Success)
            {
                return RedirectToAction("Dashboard", "User");
            }
            else if (message.MessageType == Domain.Enums.MessageType.NewUser)
            {
                return RedirectToAction("NewRegistrationfromSocialPage", "User");
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


        #endregion

        #endregion

        
    }
}