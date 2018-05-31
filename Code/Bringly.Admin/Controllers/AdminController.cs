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

        #region Manage City
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
        #endregion
        #region Manage Role
        public ActionResult ManageRoles()
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            return View(userDomainLogic.GetRoles());
        }
        public ActionResult AddRole()
        {
            return View("SaveRole", new UserRole());
        }
        [HttpPost]
        public ActionResult AddRole(UserRole role)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = new Message();
            if (userDomainLogic.AddRole(role))
            {
                ModelState.Clear();
                role = new UserRole();
                message.MessageText = "Role added successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Role already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;
        }

        [HttpGet]
        public ActionResult EditRole(Guid id)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            UserRole role = userDomainLogic.GetRole(id);
            return PartialView("SaveRole", role);
        }
        [HttpPost]
        public ActionResult EditRole(UserRole role)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = new Message();
            if (userDomainLogic.UpdateRole(role))
            {
                ModelState.Clear();
                role = new UserRole();
                message.MessageText = "Role updated successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Role already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult DeleteRole(string roleGuid)
        {
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            Message message = new Message();
            if (userDomainLogic.DeleteRoleLogic(new Guid(roleGuid)))
            {
                ModelState.Clear();
                message.MessageText = "Role has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Role deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }



        #endregion




        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}