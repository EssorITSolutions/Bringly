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
using System.Configuration;

namespace Bringly.UI.Controllers
{
    public class BusinessController : BaseClasses.AuthoriseUserControllerBase
    {
        #region Manage Business Type

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageBusinessType()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return View(businessDomainLogic.GetBusinessTypes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBusinessType()
        {
            return View("SaveBusinessType", new BusinessType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBusinessType(Guid id)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            BusinessType businessType = businessDomainLogic.GetBusinessType(id);
            return PartialView("SaveBusinessType", businessType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessTypeGuid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddLocation()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            ViewBag.GoolgeMapKey = ConfigurationManager.AppSettings["GoogleMapKey"];
            var model = businessDomainLogic.Newbusiness();
            model.CityList.Add(new Domain.City { CityGuid = Guid.Empty, CityName = "Other", CityUrlName = "Other" });
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddLocation(BusinessObject businessObject)
        {

            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();

            if (!string.IsNullOrEmpty(businessObject.CityName) && businessObject.CityGuid == Guid.Empty)
            {
                CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
                Bringly.Domain.City city = new Domain.City
                {
                    CityGuid = Guid.NewGuid(),
                    CityName = businessObject.CityName,
                    CityUrlName = businessObject.CityName
                };
                businessObject.CityGuid = commonDomainLogic.AddCity(city);
            }

            businessDomainLogic.AddLocation(businessObject);
            TempData["Success"] = "Saved successfully";
            return RedirectToAction("LocationList", "Business");
            //return Json(businessDomainLogic.AddLocation(businessObject), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessGuid"></param>
        /// <returns></returns>
        public ActionResult MyLocation(Guid businessGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            BusinessObject businessObject = new BusinessObject();
            businessObject = businessDomainLogic.GetLocationByGuid(businessGuid);
            businessObject.CityList.Add(new Domain.City { CityGuid = Guid.Empty, CityName = "Other", CityUrlName = "Other" });
            ViewBag.GoolgeMapKey = ConfigurationManager.AppSettings["GoogleMapKey"];
            return View(businessObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateLocation(BusinessObject BusinessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();

            if (!string.IsNullOrEmpty(BusinessObject.CityName) && BusinessObject.CityGuid == Guid.Empty)
            {
                Bringly.Domain.City city = new Domain.City
                {
                    CityGuid = Guid.NewGuid(),
                    CityName = BusinessObject.CityName,
                    CityUrlName = BusinessObject.CityName
                };

                BusinessObject.CityGuid = commonDomainLogic.AddCity(city);
            }

            return Json(businessDomainLogic.UpdateLocation(BusinessObject), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ActionName("aboutus")]
        public ActionResult UpdateAboutUsPage(BusinessObject BusinessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            businessDomainLogic.UpdateAboutUsPageMerchant(BusinessObject);
            return RedirectToAction("LocationList", new { isSucess = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSucess"></param>
        /// <returns></returns>
        public ActionResult LocationList(bool isSucess = false)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            if (TempData["Success"] != null)
                Success(TempData["Success"].ToString());
            if (isSucess)
                Success("Saved successfully");

            return View(businessDomainLogic.GetAllLocations());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessGuid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound", "No branch(s) found.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult CommonNoRecordFoundPartial(string message)
        {
            return PartialView("_NoRecordFound", message);
        }

        #endregion Manage Business

        #region Location Custom Property

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewProperty()
        {
            return PartialView("_multipleBusinessProperty");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomFiledList"></param>
        /// <returns></returns>
        public ActionResult AddCustomField(List<CustomProperty> CustomFiledList)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.AddCustomProperty(CustomFiledList), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomFiledList"></param>
        /// <returns></returns>
        public ActionResult UpdateCustomField(List<CustomProperty> CustomFiledList)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.UpdateCustomProperty(CustomFiledList), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomFieldGuid"></param>
        /// <returns></returns>
        public ActionResult DeleteCustomField(Guid CustomFieldGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.DeleteCustomProperty(CustomFieldGuid), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Location User

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BusinessTypeGuid"></param>
        /// <param name="LocationGuid"></param>
        /// <returns></returns>
        public ActionResult LocationListUser(string id, Nullable<Guid> BusinessTypeGuid, Nullable<Guid> LocationGuid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            CommonDomainLogic _commonDomainLogic = new CommonDomainLogic();
            ChooseCity chooseCity = new ChooseCity();
            TempData["SelectedBusinessTypeGuid"] = BusinessTypeGuid;
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
                else
                {
                    chooseCity.Cities = _commonDomainLogic.GetCities();
                }

                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }

            int Currentpage = 0;
            TempData["CurrentPage"] = null;
            if (LocationGuid == null || LocationGuid == Guid.Empty)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BusinessTypeGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LocationListUser(string id, Nullable<Guid> BusinessTypeGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"].ToType<int>(0);
            TempData.Keep();
            return PartialView("_EmailList", email.GetInboxEmail(Currentpage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult BusinessHeader()
        {
            Guid selectedBusinessTypeGuid = Guid.Empty;
            if (TempData["SelectedBusinessTypeGuid"] != null)
            {
                ViewBag.selectedBusinessTypeGuid = (Guid)(TempData["SelectedBusinessTypeGuid"]);
            }
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();

            return PartialView("_filterBusinessHeader", businessDomainLogic.GetBusinessHeader());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult GetBusinessInfo(Guid guid)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return PartialView("_GetBusinessInfo", businessDomainLogic.GetLocationByGuid(guid));
        }

        #endregion

        #region Saloon/Spa

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MakeSaloonAppointment(BusinessObject businessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.MakeUpdateAppointment(businessObject));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsSaloonBooked(BusinessObject businessObject)
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            return Json(businessDomainLogic.IsSaloonBooked(businessObject));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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
            return View(businessDomainLogic.GetAppointmentByUserGuid(guid.HasValue ? guid.Value : Guid.Empty, Currentpage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SaloonAppointmentGuid"></param>
        /// <returns></returns>
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