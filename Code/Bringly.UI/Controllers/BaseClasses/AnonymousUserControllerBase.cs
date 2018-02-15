using System.Web.Mvc;


namespace Bringly.UI.Controllers.BaseClasses
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
            else
            {
                return RedirectToAction("dashboard", "reports");
            }
        }
    }
}