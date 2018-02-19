using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.AdminDomainLogic;

namespace Bringly.Admin.Controllers
{
    public class AdminController : BaseClasses.AuthoriseUserControllerBase
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ManageCities()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetCities());
        }
        public ActionResult EditCity(Guid id)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetCity(id));
        }
    }
}