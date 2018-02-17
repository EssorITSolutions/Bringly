using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;
using Bringly.Domain.Common;
using Bringly.DomainLogic;

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
            if (message.MessageType == Domain.Enums.MessageType.Success)
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
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }
    }
}