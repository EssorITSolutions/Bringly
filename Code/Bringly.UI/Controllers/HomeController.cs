using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class HomeController : BaseClasses.AnonymousUserControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}