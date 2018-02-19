using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.AdminDomain;
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
            City city= userDomainLogic.GetCity(id);
            return View(city==null?new City():city);
        }
        [HttpPost]
        public ActionResult EditCity(FormCollection formCityEdit)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Guid cityGuid = new Guid(formCityEdit["CityGuid"]);
            userDomainLogic.AddUpdateCity(formCityEdit["CityName"], cityGuid);
            return RedirectToAction("ManageCities");
        }
        
        public ActionResult IsDuplicateCity(string cityName,string cityGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();            ;
            return Json(userDomainLogic.IsCityExists(cityName,new Guid(cityGuid)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCity(Guid cityGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return Json(userDomainLogic.DeleteCityLogic(cityGuid), JsonRequestBehavior.AllowGet);
        }

    }
}