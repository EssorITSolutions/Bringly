using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Error404()
        {
            return View();
        }
    }
}