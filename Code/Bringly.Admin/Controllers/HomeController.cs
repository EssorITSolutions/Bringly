using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.AdminDomain;
using Bringly.AdminDomain.Common;
using Bringly.AdminDomainLogic;

namespace Bringly.Admin.Controllers
{
    public class HomeController : BaseClasses.AnonymousUserControllerBase
    {
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
            if (message.MessageType == AdminDomain.Enums.MessageType.Success)
            {
                return RedirectAfterLogin(returnUrl);
            }
            else
            {
                ErrorBlock(message.MessageText);
                return View(userLogin);
            }
        }
    }
}