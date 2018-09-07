using System.Web.Mvc;
using Bringly.DomainLogic;
namespace Bringly.UI.Controllers.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    [SuperAdminUserModulesCheck]
    public class SuperAdminUserControllerBase : AuthoriseUserControllerBase
    {

    }

    /// <summary>
    /// 
    /// </summary>
    class SuperAdminUserModulesCheck : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserVariables.IsSuperAdmin) return;

            //else go away
            filterContext.Result = new RedirectResult(DomainLogic.Settings.SystemSettings.Error404PageUrl);
        }
    }
}