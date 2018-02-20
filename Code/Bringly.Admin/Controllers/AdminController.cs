using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.AdminDomain;
using Bringly.AdminDomainLogic;
using Bringly.AdminDomain.Common;
using Bringly.AdminDomain.Enums;
using System.Web.Security;

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
            Message message = new Message();            
            if (cityDomainLogic.AddCity(city))
            {
                ModelState.Clear();
                city = new City();
                message.MessageText = "City added successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "City already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;            
        }
        [HttpGet]
        public ActionResult EditCity(Guid id)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            City city = cityDomainLogic.GetCity(id);
            return PartialView("SaveCity", city);
        }
        [HttpPost]
        public ActionResult EditCity(City city)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            Message message = new Message();
            if (cityDomainLogic.UpdateCity(city))
            {
                ModelState.Clear();
                city = new City();
                message.MessageText = "City updated successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "City already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult DeleteCity(string cityGuid)
        {
            CityDomainLogic cityDomainLogic = new CityDomainLogic();
            Message message = new Message();
            if (cityDomainLogic.DeleteCityLogic(new Guid(cityGuid)))
            {
                ModelState.Clear();
                message.MessageText = "City has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else {
                message.MessageText = "City deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}