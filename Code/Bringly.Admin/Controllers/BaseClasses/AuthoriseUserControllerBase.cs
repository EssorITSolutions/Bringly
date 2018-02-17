using System.Web.Mvc;

namespace Bringly.Admin.Controllers.BaseClasses
{
    [Authorize]
    public abstract class AuthoriseUserControllerBase : ControllerBase
    {
    }
}