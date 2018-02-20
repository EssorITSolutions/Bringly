using System.Web.Mvc;


namespace Bringly.Admin.Controllers.BaseClasses
{
    public class AnonymousUserControllerBase : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        protected ActionResult RedirectAfterLogin(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("ManageCities", "Admin");
        }
    }
}