using System;
using System.Web.Mvc;
using Bringly.Domain;
using Bringly.Domain.Common;
using Bringly.Domain.Business;
using Bringly.DomainLogic.Business;
using Bringly.Domain.Enums;
using Bringly.DomainLogic;
using System.Collections.Generic;
using Bringly.DomainLogic.User;
using System.Linq;

namespace Bringly.UI.Controllers
{
    public class BusinessController : BaseClasses.AuthoriseUserControllerBase
    {
        #region Manage Business Type

        public ActionResult ManageBusinessType()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return View(businessDomainLogic.GetBusinessTypes());
        }
        public ActionResult AddBusinessType()
        {
            return View("SaveBusinessType", new BusinessType());
        }
        [HttpPost]
        public ActionResult AddBusinessType(BusinessType businessType)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            Message message = new Message();
            if (businessDomainLogic.AddBusinessType(businessType))
            {
                ModelState.Clear();
                businessType = new BusinessType();
                message.MessageText = "Business Type added successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Business Type already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;
        }
        [HttpGet]
        public ActionResult EditBusinessType(Guid id)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            BusinessType businessType = businessDomainLogic.GetBusinessType(id);
            return PartialView("SaveBusinessType", businessType);
        }
        [HttpPost]
        public ActionResult EditBusinessType(BusinessType businessType)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            Message message = new Message();
            if (businessDomainLogic.UpdateBusinessType(businessType))
            {
                ModelState.Clear();
                businessType = new BusinessType();
                message.MessageText = "Business Type updated successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Business Type already exists.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult DeleteBusinessType(string BusinessTypeGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            Message message = new Message();
            if (businessDomainLogic.DeleteBusinessType(new Guid(BusinessTypeGuid)))
            {
                ModelState.Clear();
                message.MessageText = "Business Type has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Business Type deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Manage Business Locations

        public ActionResult AddLocation()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return View(businessDomainLogic.Newbusiness());
        }
        [HttpPost]
        public ActionResult AddLocation(BusinessObject businessObject)
        {

            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.AddLocation(businessObject),JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyLocation(Guid businessGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            BusinessObject businessObject = new BusinessObject();
            businessObject = businessDomainLogic.GetLocationByGuid(businessGuid);
            return View(businessObject);
        }
        // tblbusiness update
        //[HttpPost]
        //public ActionResult UpdateBusinessProfile(BusinessObject BusinessObject)
        //{
        //    BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
        //    return Json(businessDomainLogic.UpdateLocationProfile(BusinessObject), JsonRequestBehavior.AllowGet);
        //}

        // tbllocation update
        [HttpPost]
        public ActionResult UpdateLocation(BusinessObject BusinessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.UpdateLocation(BusinessObject), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LocationList()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return View(businessDomainLogic.GetAllLocations());
        }
        [HttpPost]
        public ActionResult DeleteLocation(Guid BusinessGuid)
        {
            Message message = new Message();
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            if (businessDomainLogic.DeleteLocation(BusinessGuid))
            {
                ModelState.Clear();
                message.MessageText = "Location has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Location deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound", "No location(s) found.");
        }
        public ActionResult CommonNoRecordFoundPartial(string message)
        {
            return PartialView("_NoRecordFound", message);
        }
        #endregion Manage Business

        #region Location Custom Property

        public ActionResult AddNewProperty()
        {
            return PartialView("_multipleBusinessProperty");
        }
        public ActionResult AddCustomField(List<CustomProperty> CustomFiledList)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.AddCustomProperty(CustomFiledList), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCustomField(List<CustomProperty> CustomFiledList)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.UpdateCustomProperty(CustomFiledList), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCustomField(Guid CustomFieldGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.DeleteCustomProperty(CustomFieldGuid), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Location User

        public ActionResult LocationListUser(string id,Nullable<Guid> BusinessTypeGuid, Nullable<Guid> LocationGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            CommonDomainLogic _commonDomainLogic = new CommonDomainLogic();
            ChooseCity chooseCity = new ChooseCity();
            if (!string.IsNullOrEmpty(id))
            {
                Guid _cityguid = _commonDomainLogic.FindCityGuid(id);
                UserDomainLogic _userDomainLogic = new UserDomainLogic();
                _userDomainLogic.UpdatePreferedCity(_cityguid);
                chooseCity.Cities = _commonDomainLogic.GetCityByGUID(_cityguid);
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }
            else
            {
                UserDomainLogic userDomainLogic = new UserDomainLogic();
                string guid = userDomainLogic.FindUser(UserVariables.LoggedInUserGuid).PreferedCity;
                if (!string.IsNullOrEmpty(guid))
                {
                    chooseCity.Cities = _commonDomainLogic.GetCityByGUID(new Guid(guid));
                }
                else {
                    chooseCity.Cities = _commonDomainLogic.GetCities();
                }
                
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }
            int Currentpage = 0;
            TempData["CurrentPage"] = null;
            if (LocationGuid ==null || LocationGuid == Guid.Empty)
            {
                if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
                {
                    TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                    Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                    TempData.Keep();
                }
            }
            return View(businessDomainLogic.GetBusinessByCity(chooseCity.SelectedCity, BusinessTypeGuid, Currentpage));
        }
        [HttpPost]
        public ActionResult LocationListUser(string id, Nullable<Guid> BusinessTypeGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"].ToType<int>(0);
            TempData.Keep();
            return PartialView("_EmailList", email.GetInboxEmail(Currentpage));
        }
        [ChildActionOnly]
        public ActionResult BusinessHeader()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return PartialView("_filterBusinessHeader", businessDomainLogic.GetBusinessHeader());
        }
        public ActionResult GetBusinessInfo(Guid guid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return PartialView("_GetBusinessInfo", businessDomainLogic.GetLocationByGuid(guid));
        }

        #endregion

        #region Saloon/Spa
        [HttpPost]
        public JsonResult MakeSaloonAppointment(BusinessObject businessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.MakeUpdateAppointment(businessObject));
        }
        [HttpPost]
        public JsonResult IsSaloonBooked(BusinessObject businessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.IsSaloonBooked(businessObject));
        }
        public ActionResult UserAppointments(Nullable<Guid> guid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            int Currentpage = 0;
            TempData["CurrentPage"] = null;
            if (guid == null || guid == Guid.Empty)
            {
                if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
                {
                    TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                    Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                    TempData.Keep();
                }
            }
            return View(businessDomainLogic.GetAppointmentByUserGuid(guid.HasValue?guid.Value:Guid.Empty, Currentpage));
        }
        public ActionResult DeleteSaloonAppointment(Guid SaloonAppointmentGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            Message message = new Message();
            if (businessDomainLogic.DeleteSaloonAppointment(SaloonAppointmentGuid))
            {
                ModelState.Clear();
                message.MessageText = "Appointment has been deleted successfully.";
                message.MessageType = MessageType.Success;
            }
            else
            {
                message.MessageText = "Appointment deletion failed.";
                message.MessageType = MessageType.Error;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}