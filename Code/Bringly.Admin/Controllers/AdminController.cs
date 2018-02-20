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
        public ActionResult AddCity()
        {
            return View("SaveCity", new City());
        }
        [HttpPost]
        public ActionResult AddCity(City city)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            if (cityDomainLogic.AddCity(city))
            {
                Success("City added successfully");
                ModelState.Clear();
                city = new City();
            }
            else
            {
                ErrorBlock("City already exists");
            }
            return View("SaveCity", city);
        }
        [HttpGet]
        public ActionResult EditCity(Guid id)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            City city = cityDomainLogic.GetCity(id);
            return View("SaveCity", city);
        }
        [HttpPost]
        public ActionResult EditCity(City city)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            if (cityDomainLogic.UpdateCity(city))
            {
                Success("City saved successfully");
                ModelState.Clear();
                city = new City();
            }
            else
            {
                ErrorBlock("City already exists");
            }
            return View("SaveCity", city);
        }
        public ActionResult DeleteCity(string cityGuid)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            return Json(cityDomainLogic.DeleteCityLogic(new Guid(cityGuid)), JsonRequestBehavior.AllowGet);
        }

    }
}