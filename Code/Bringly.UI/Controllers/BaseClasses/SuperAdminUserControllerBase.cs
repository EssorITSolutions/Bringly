using System.Web.Mvc;

namespace Bringly.UI.Controllers.BaseClasses
{
    [SuperAdminUserModulesCheck]
    public class SuperAdminUserControllerBase : AuthoriseUserControllerBase
    {

    }
    class SuperAdminUserModulesCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserVariables.IsSuperAdmin) return;

            //else go away
            filterContext.Result = new RedirectResult(DomainLogic.Settings.SystemSettings.Error404PageUrl);
        }
    }
}