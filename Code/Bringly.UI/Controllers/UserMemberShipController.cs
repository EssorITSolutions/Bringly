using System.Web.Mvc;
using System.Web.Security;

namespace Bringly.UI.Controllers
{
    public class UserMemberShipController : BaseClasses.AnonymousUserControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            return UserVariables.IsAuthenticated ? RedirectAfterLogin(returnUrl) :
                View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(object userLogin, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string result = "Hello";
                if (!string.IsNullOrEmpty(result))
                {
                    ErrorBlock(result);
                }
                else
                {
                    return RedirectAfterLogin(returnUrl);
                }
            }
            return View(userLogin);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToLogin();
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
    }
}