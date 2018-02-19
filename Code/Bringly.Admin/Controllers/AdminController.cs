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
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            return View(cityDomainLogic.GetCities());
        }
        public Action AddCity()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditCity(Guid id)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            City city = cityDomainLogic.GetCity(id);
            return View(city == null ? new City() : city);
        }
        [HttpPost]
        public ActionResult EditCity(City city)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            cityDomainLogic.AddUpdateCity(city);
            return View(city);
        }
        public ActionResult DeleteCity(string cityGuid)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            return Json(cityDomainLogic.DeleteCityLogic(new Guid(cityGuid)), JsonRequestBehavior.AllowGet);
        }

    }
}