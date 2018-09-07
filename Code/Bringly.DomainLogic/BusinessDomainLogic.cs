using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Business;
using Bringly.DomainLogic.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Bringly.DomainLogic.Business
{
    public class BusinessDomainLogic : BaseClass.DomainLogicBase
    {
        #region Business Type

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BusinessType> GetBusinessTypes()
        {
            return bringlyEntities.tblBusinessTypes.Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName = c.BusinessTypeName }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SaloonTimeMaster> GetAllSaloonSlots()
        {
            return bringlyEntities.tblSaloonTimeMasters.Select(c => new SaloonTimeMaster { SaloonTimeGuid = c.SaloonTimeGuid, SlotTime = c.SlotTime }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BusinessObject GetBusinessHeader()
        {
            BusinessObject businessObject = new BusinessObject();
            businessObject.BusinessTypeList = bringlyEntities.tblBusinessTypes.Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName = c.BusinessTypeName }).ToList();
            tblUser tblUsers = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.FK_PreferedCity == x.tblCity.CityGuid).FirstOrDefault();
            if (tblUsers != null)
            {
                businessObject.CityName = bringlyEntities.tblCities.Where(x => x.CityGuid == tblUsers.FK_PreferedCity).FirstOrDefault().CityUrlName;
            }
            else
            {
                businessObject.CityName = bringlyEntities.tblCities.FirstOrDefault().CityUrlName;
            }
            return businessObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessTypeGuid"></param>
        /// <returns></returns>
        public BusinessType GetBusinessType(Guid BusinessTypeGuid)
        {
            return bringlyEntities.tblBusinessTypes
                .Where(c => c.BusinessTypeGuid == BusinessTypeGuid)
                .Select(c => new BusinessType
                {
                    BusinessTypeGuid = c.BusinessTypeGuid,
                    BusinessTypeName = c.BusinessTypeName
                }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public bool AddBusinessType(BusinessType businessType)
        {
            if (!IsBusinessTypeExists(businessType))
            {
                bringlyEntities.tblBusinessTypes.Add(new tblBusinessType { BusinessTypeGuid = Guid.NewGuid(), BusinessTypeName = businessType.BusinessTypeName });
                bringlyEntities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public bool UpdateBusinessType(BusinessType businessType)
        {
            if (!IsBusinessTypeExists(businessType))
            {
                tblBusinessType businessTypeObject = bringlyEntities.tblBusinessTypes
                    .Where(x => x.BusinessTypeGuid == businessType.BusinessTypeGuid).FirstOrDefault();

                businessTypeObject.BusinessTypeName = businessType.BusinessTypeName;
                bringlyEntities.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public bool IsBusinessTypeExists(BusinessType businessType)
        {
            bool businessTypeexists = false;
            tblBusinessType businessTypeObject = bringlyEntities.tblBusinessTypes
                .Where(x => x.BusinessTypeName == businessType.BusinessTypeName && x.BusinessTypeGuid != businessType.BusinessTypeGuid)
                .FirstOrDefault();

            if (businessTypeObject != null && !string.IsNullOrEmpty(businessTypeObject.BusinessTypeName))
            {
                businessTypeexists = true;
            }

            return businessTypeexists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypeGuid"></param>
        /// <returns></returns>
        public bool DeleteBusinessType(Guid businessTypeGuid)
        {
            tblBusinessType businessType = bringlyEntities.tblBusinessTypes
                .Where(c => c.BusinessTypeGuid == businessTypeGuid)
                .FirstOrDefault();

            bringlyEntities.Entry(businessType).State = EntityState.Deleted;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BusinessObject Newbusiness()
        {
            BusinessObject BusinessObject = new BusinessObject();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            BusinessObject.BusinessTypeList = GetBusinessTypes();
            BusinessObject.CityList = commonDomainLogic.GetCities();

            BusinessObject.CVRNumber = bringlyEntities.tblUsers
                .Where(x => x.UserGuid == UserVariables.LoggedInUserGuid)
                .FirstOrDefault().CVRNumber;

            BusinessObject.Managers = bringlyEntities.tblManagers
                .Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false)
                .Select(x => new Manager
                {
                    BranchGuid = x.BranchGuid,
                    BusinessGuid = x.BusinessGuid,
                    CreatedByGuid = x.CreatedByGuid,
                    DateCreated = x.DateCreated,
                    ManagerGuid = x.ManagerGuid,
                    Name = bringlyEntities.tblUsers.Where(z => z.UserGuid == x.UserGuid).Select(z => z.FullName).FirstOrDefault()
                }).ToList();

            return BusinessObject;
        }

        #endregion        

        #region Business Locations

        // Get Location by Guid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessGuid"></param>
        /// <returns></returns>
        public BusinessObject GetLocationByGuid(Guid businessGuid)
        {
            BusinessObject business = new BusinessObject();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            List<City> list = commonDomainLogic.GetCities();
            business = bringlyEntities.tblBranches.Where(x => x.BranchGuid == businessGuid && x.IsDeleted == false).Select(r =>
                new BusinessObject
                {
                    BusinessImage = r.BranchImage,
                    BusinessGuid = r.BranchGuid,
                    BusinessName = r.BranchName,
                    CityGuid = r.FK_CityGuid,
                    BusinessTypeGuid = r.FK_BusinessTypeGuid,
                    PNumber = r.PNumber,
                    Phone = r.Phone,
                    PinCode = r.PinCode,
                    CreatedByGuid = r.FK_CreatedByGuid,
                    ManagerGuid = r.ManagerUserGuid,
                    Address = r.Address,
                    Email = r.Email,
                    OrderTiming = r.OrderTiming,
                    PickUpTiming = r.PickUpTiming,
                    ServiceCharge = r.ServiceCharge,
                    ServiceTax = r.ServiceTax,
                    FlatRate = r.FlatRate,
                    RateAfterKm = r.RateAfterKm,
                    Description = r.Description,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    PlaceId = r.PlaceId,
                    CountryName = r.Country,
                    AboutUsDescription = r.tblMerchantAboutUsPages.Where(x => x.FK_BranchGuid == r.BranchGuid).Select(x => x.Description).FirstOrDefault(),
                    AboutUsTitle = r.tblMerchantAboutUsPages.Where(x => x.FK_BranchGuid == r.BranchGuid).Select(x => x.Title).FirstOrDefault(),
                    AboutUsPageGuid = r.tblMerchantAboutUsPages.Where(x => x.FK_BranchGuid == r.BranchGuid).Select(x => x.AboutUsPageGuid).FirstOrDefault()
                }).FirstOrDefault();

            business.CityList = list;
            business.BusinessTypeName = bringlyEntities.tblBusinessTypes
                .Where(c => c.BusinessTypeGuid == business.BusinessTypeGuid)
                .FirstOrDefault()
                .BusinessTypeName;

            business.CustomPropertyList = bringlyEntities.tblCustomProperties
                .Where(x => x.LocationGuid == businessGuid)
                .Select(z => new CustomProperty
                {
                    CustomPropertyGuid = z.CustomPropertyGuid,
                    LocationGuid = z.LocationGuid,
                    Field = z.Field,
                    Value = z.Value,
                })
                .ToList();

            business.CityName = business.CityGuid != Guid.Empty ? bringlyEntities.tblCities.Where(x => x.CityGuid == business.CityGuid).FirstOrDefault().CityName : "";
            business.CVRNumber = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault().CVRNumber;

            business.Managers = bringlyEntities.tblManagers.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false).Select(x => new Manager
            {
                BranchGuid = x.BranchGuid,
                BusinessGuid = x.BusinessGuid,
                CreatedByGuid = x.CreatedByGuid,
                DateCreated = x.DateCreated,
                ManagerGuid = x.ManagerGuid,
                Name = bringlyEntities.tblUsers.Where(z => z.UserGuid == x.UserGuid).Select(z => z.FullName).FirstOrDefault()
            }).ToList();

            return business;
        }

        // Business Update
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public string UpdateLocationProfile(BusinessObject BusinessObject)
        {
            tblBusiness business = bringlyEntities.tblBusinesses
                .Where(x => x.BusinessGuid == BusinessObject.BusinessGuid)
                .FirstOrDefault();

            business.BusinessName = BusinessObject.BusinessName;
            business.CityGuid = BusinessObject.CityGuid;
            business.PNumber = BusinessObject.PNumber;
            business.Phone = BusinessObject.Phone;
            business.PinCode = BusinessObject.PinCode;
            business.ModifiedBy = UserVariables.LoggedInUserGuid;
            business.ModifiedDate = DateTime.Now;
            business.Address = BusinessObject.Address;
            business.Email = BusinessObject.Email;

            business.OrderTiming = BusinessObject.OrderTiming;
            business.PickUpTiming = BusinessObject.PickUpTiming;
            business.ServiceCharge = BusinessObject.ServiceCharge;
            business.ServiceTax = BusinessObject.ServiceTax;
            business.FlatRate = BusinessObject.FlatRate;
            business.RateAfterKm = BusinessObject.RateAfterKm;
            business.Description = BusinessObject.Description;

            bringlyEntities.SaveChanges();

            string cityname = bringlyEntities.tblCities.Where(x => x.CityGuid == BusinessObject.CityGuid).FirstOrDefault().CityName;
            return cityname;
        }

        // Location Update
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public string UpdateLocation(BusinessObject BusinessObject)
        {

            tblBranch tblBranch = bringlyEntities.tblBranches
                .Where(x => x.BranchGuid == BusinessObject.BusinessGuid)
                .FirstOrDefault();

            tblBranch.BranchName = BusinessObject.BusinessName;
            tblBranch.FK_CityGuid = BusinessObject.CityGuid;
            tblBranch.PNumber = BusinessObject.PNumber;
            tblBranch.Phone = BusinessObject.Phone;
            tblBranch.PinCode = BusinessObject.PinCode;
            tblBranch.ModifiedBy = UserVariables.LoggedInUserGuid;
            tblBranch.ModifiedDate = DateTime.Now;
            tblBranch.Address = BusinessObject.Address;
            tblBranch.Email = BusinessObject.Email;
            tblBranch.ManagerUserGuid = BusinessObject.ManagerGuid ?? Guid.Empty;
            tblBranch.OrderTiming = BusinessObject.OrderTiming;
            tblBranch.PickUpTiming = BusinessObject.PickUpTiming;
            tblBranch.ServiceCharge = BusinessObject.ServiceCharge;
            tblBranch.ServiceTax = BusinessObject.ServiceTax;
            tblBranch.FlatRate = BusinessObject.FlatRate;
            tblBranch.RateAfterKm = BusinessObject.RateAfterKm;
            tblBranch.Description = BusinessObject.Description;
            tblBranch.Latitude = BusinessObject.Latitude;
            tblBranch.Longitude = BusinessObject.Longitude;
            tblBranch.PlaceId = BusinessObject.PlaceId;
            tblBranch.Country = BusinessObject.CountryName;
            bringlyEntities.SaveChanges();

            string cityname = bringlyEntities.tblCities.Where(x => x.CityGuid == BusinessObject.CityGuid).FirstOrDefault().CityName;
            return cityname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public bool UpdateAboutUsPageMerchant(BusinessObject businessObject)
        {
            tblMerchantAboutUsPage tblMerchantAboutUsPage = bringlyEntities.tblMerchantAboutUsPages
                .Where(x => x.FK_BranchGuid == businessObject.BusinessGuid && x.AboutUsPageGuid == businessObject.AboutUsPageGuid)
                .FirstOrDefault();

            if (tblMerchantAboutUsPage == null)
            {
                tblBranch tblBranch = bringlyEntities.tblBranches
                    .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.BranchGuid == businessObject.BusinessGuid)
                    .FirstOrDefault();

                tblMerchantAboutUsPage = new tblMerchantAboutUsPage();
                tblMerchantAboutUsPage.AboutUsPageGuid = Guid.NewGuid();
                tblMerchantAboutUsPage.FK_BranchGuid = tblBranch.BranchGuid;
                tblMerchantAboutUsPage.CreatedByGuid = UserVariables.LoggedInUserGuid;
                tblMerchantAboutUsPage.DateCreated = DateTime.Now;
                tblMerchantAboutUsPage.Description = businessObject.AboutUsDescription;
                tblMerchantAboutUsPage.IsActive = true;
                tblMerchantAboutUsPage.IsDelete = false;
                tblMerchantAboutUsPage.Title = businessObject.AboutUsTitle;
                bringlyEntities.tblMerchantAboutUsPages.Add(tblMerchantAboutUsPage);
            }
            else
            {
                tblMerchantAboutUsPage.Title = businessObject.AboutUsTitle;
                tblMerchantAboutUsPage.Description = businessObject.AboutUsDescription;
            }
            bringlyEntities.SaveChanges();

            return true;
        }

        // Add Location
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public Guid AddLocation(BusinessObject BusinessObject)
        {
            tblBusiness business = bringlyEntities.tblBusinesses
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid)
                .FirstOrDefault();

            tblBranch tblBranch = new tblBranch();

            tblBranch.BranchGuid = Guid.NewGuid();
            tblBranch.FK_BusinessTypeGuid = business.FK_BusinessTypeGuid;
            tblBranch.BranchName = BusinessObject.BusinessName;
            tblBranch.FK_CityGuid = BusinessObject.CityGuid;
            tblBranch.PNumber = BusinessObject.PNumber;
            tblBranch.Phone = BusinessObject.Phone;
            tblBranch.PinCode = BusinessObject.PinCode;
            tblBranch.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
            tblBranch.DateCreated = DateTime.Now;
            tblBranch.Address = BusinessObject.Address;
            tblBranch.Email = BusinessObject.Email;
            tblBranch.ManagerUserGuid = (BusinessObject.ManagerGuid == null || (!BusinessObject.ManagerGuid.HasValue || BusinessObject.ManagerGuid.Value == Guid.Empty)) ? UserVariables.LoggedInUserGuid : BusinessObject.ManagerGuid.Value;
            tblBranch.OrderTiming = BusinessObject.OrderTiming;
            tblBranch.PickUpTiming = BusinessObject.PickUpTiming;
            tblBranch.ServiceCharge = BusinessObject.ServiceCharge;
            tblBranch.ServiceTax = BusinessObject.ServiceTax;
            tblBranch.FlatRate = BusinessObject.FlatRate;
            tblBranch.RateAfterKm = BusinessObject.RateAfterKm;
            tblBranch.Description = BusinessObject.Description;
            tblBranch.IsDeleted = false;
            tblBranch.Latitude = BusinessObject.Latitude;
            tblBranch.Longitude = BusinessObject.Longitude;
            tblBranch.PlaceId = BusinessObject.PlaceId;
            tblBranch.Country = BusinessObject.CountryName;
            bringlyEntities.tblBranches.Add(tblBranch);
            bringlyEntities.SaveChanges();

            return tblBranch.BranchGuid;
        }

        // Get all Location
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BusinessObject> GetAllLocations()
        {
            List<BusinessObject> locationlist = new List<BusinessObject>();
            locationlist = bringlyEntities.tblBranches
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false)
                .Select(z => new BusinessObject
                {
                    BusinessGuid = z.BranchGuid,
                    BusinessName = z.BranchName,
                    CityGuid = z.FK_CityGuid,
                    PNumber = z.PNumber,
                    Phone = z.Phone,
                    PinCode = z.PinCode,
                    Address = z.Address,
                    Email = z.Email,
                    OrderTiming = z.OrderTiming,
                    PickUpTiming = z.PickUpTiming,
                    ServiceCharge = z.ServiceCharge,
                    ServiceTax = z.ServiceTax,
                    FlatRate = z.FlatRate,
                    RateAfterKm = z.RateAfterKm,
                    Description = z.Description,
                    ManagerGuid = z.ManagerUserGuid
                }).ToList();

            return locationlist;
        }

        // Delete Location
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessGuid"></param>
        /// <returns></returns>
        public bool DeleteLocation(Guid businessGuid)
        {
            tblBranch tblLocation = bringlyEntities.tblBranches
                .Where(x => x.BranchGuid == businessGuid && x.IsDeleted == false)
                .FirstOrDefault();

            tblLocation.IsDeleted = true;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_city"></param>
        /// <param name="BusinessTypeGuid"></param>
        /// <param name="LatestPage"></param>
        /// <returns></returns>
        public MyBusiness GetBusinessByCity(City _city, Nullable<Guid> BusinessTypeGuid, int LatestPage = 0)
        {
            MyBusiness _businessSearch = new MyBusiness();
            _businessSearch.PageSize = PageSizeBusiness;
            _businessSearch.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;
            _businessSearch.SortBy = SortBy;

            if (BusinessTypeGuid == null || BusinessTypeGuid == Guid.Empty)
            {
                BusinessTypeGuid = Guid.Empty;
                BusinessTypeGuid = Guid.Parse("AB97A903-B8F3-4772-AAF8-4CDCEA630054");
            }

            _businessSearch.BusinessObjects = bringlyEntities.tblBranches
                .Where(x => x.IsDeleted == false && x.FK_BusinessTypeGuid == BusinessTypeGuid)
                .Select(r => new BusinessObject
                {
                    BusinessImage = r.BranchImage,
                    BusinessGuid = r.BranchGuid,
                    BusinessName = r.BranchName,
                    CityGuid = r.FK_CityGuid,
                    CityName = _city.CityName,
                    IsFavorite = false,
                    Address = r.Address
                }).ToList();

            _businessSearch.BusinessName = bringlyEntities.tblBusinessTypes
                .Where(x => x.BusinessTypeGuid == BusinessTypeGuid.Value)
                .FirstOrDefault()
                .BusinessTypeName;

            if (_city.CityGuid != null)
            {
                _businessSearch.BusinessObjects = _businessSearch.BusinessObjects
                    .Where(s => s.CityGuid == _city.CityGuid)
                    .ToList();
            }
            if (_businessSearch.BusinessName.Trim().ToLower() == "restaurant")
            {
                _businessSearch.SaloonSlotList = GetAllSaloonSlots();
            }
            UserDomainLogic userDomainLogic = new UserDomainLogic();
            List<BusinessObject> favouriteLocations = userDomainLogic.FavouriteLocations();//favourite part left
            _businessSearch.CityGuid = _city.CityGuid;
            _businessSearch.CityName = _city.CityName;
            List<Guid> businessGuid = favouriteLocations.Select(c => c.BusinessGuid).ToList();//favourite part left
            foreach (BusinessObject busines in _businessSearch.BusinessObjects)
            {
                if (businessGuid.Contains(busines.BusinessGuid))
                {
                    busines.IsFavorite = true;
                }
            }
            _businessSearch.TotalRecords = _businessSearch.BusinessObjects.Count;
            int Skip = 0;
            int Take = PageSize;
            if (_businessSearch.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((_businessSearch.CurrentPage * _businessSearch.PageSize) - _businessSearch.PageSize);

            _businessSearch.BusinessObjects = _businessSearch.BusinessObjects.Skip(Skip).Take(Take).ToList();

            return _businessSearch;
        }

        #endregion

        #region Location Custom Property

        // Add Custom Property
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddCustomProperty(List<CustomProperty> list)
        {
            List<tblCustomProperty> customPropertyList = new List<tblCustomProperty>();
            tblCustomProperty customProperty = new tblCustomProperty();
            foreach (var itm in list)
            {
                customProperty = new tblCustomProperty();
                customProperty.LocationGuid = itm.LocationGuid;
                customProperty.CustomPropertyGuid = Guid.NewGuid();
                customProperty.Field = itm.Field;
                customProperty.Value = itm.Value;
                customPropertyList.Add(customProperty);
            }
            bringlyEntities.tblCustomProperties.AddRange(customPropertyList);
            bringlyEntities.SaveChanges();

            return true;
        }

        // Update Custom Property
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpdateCustomProperty(List<CustomProperty> list)
        {
            List<tblCustomProperty> customPropertyList = new List<tblCustomProperty>();
            List<tblCustomProperty> customPropertyListNew = new List<tblCustomProperty>();
            tblCustomProperty customProperty = new tblCustomProperty();
            foreach (var itm in list)
            {
                if (itm.CustomPropertyGuid != Guid.Empty)
                {
                    customProperty = bringlyEntities.tblCustomProperties.Where(x => x.CustomPropertyGuid == itm.CustomPropertyGuid).FirstOrDefault();
                    customProperty.Field = itm.Field;
                    customProperty.Value = itm.Value;
                    bringlyEntities.SaveChanges();
                }
                else
                {
                    customProperty = new tblCustomProperty();
                    customProperty.LocationGuid = itm.LocationGuid;
                    customProperty.CustomPropertyGuid = Guid.NewGuid();
                    customProperty.Field = itm.Field;
                    customProperty.Value = itm.Value;
                    customPropertyListNew.Add(customProperty);
                }
            }

            bringlyEntities.tblCustomProperties.AddRange(customPropertyListNew);
            bringlyEntities.SaveChanges();

            return true;
        }

        // Delete Custom Property
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customFieldGuid"></param>
        /// <returns></returns>
        public bool DeleteCustomProperty(Guid customFieldGuid)
        {
            tblCustomProperty customProperty = bringlyEntities.tblCustomProperties.Where(x => x.CustomPropertyGuid == customFieldGuid).FirstOrDefault();
            bringlyEntities.Entry(customProperty).State = EntityState.Deleted;
            bringlyEntities.SaveChanges();
            return true;
        }

        #endregion

        #region Saloon/ Spa

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public bool MakeUpdateAppointment(BusinessObject businessObject)
        {
            if (businessObject.SaloonAppointmentGuid != null && businessObject.SaloonAppointmentGuid != Guid.Empty)
            {
                try
                {
                    tblSaloonAppointment tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.SaloonAppointmentGuid == businessObject.SaloonAppointmentGuid).FirstOrDefault();
                    tblSaloonAppointment.SaloonTime = businessObject.SaloonTime;
                    tblSaloonAppointment.FK_SaloonTimeGuid = businessObject.SaloonTimeGuid;
                    tblSaloonAppointment.AppointmentDate = businessObject.AppointmentDate;
                    tblSaloonAppointment.FK_ModifiedBy = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.ModifiedDate = DateTime.Now;
                    tblSaloonAppointment.IsApproved = businessObject.IsApproved;
                    bringlyEntities.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    tblSaloonAppointment tblSaloonAppointment = new tblSaloonAppointment();
                    tblSaloonAppointment.SaloonAppointmentGuid = Guid.NewGuid();
                    tblSaloonAppointment.FK_BusinessGuid = businessObject.BusinessGuid;
                    tblSaloonAppointment.FK_UserGuid = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.IsApproved = false;
                    tblSaloonAppointment.SaloonTime = businessObject.SaloonTime;
                    tblSaloonAppointment.FK_SaloonTimeGuid = businessObject.SaloonTimeGuid;
                    tblSaloonAppointment.AppointmentDate = businessObject.AppointmentDate;
                    tblSaloonAppointment.DateCreated = DateTime.Now;
                    tblSaloonAppointment.IsDeleted = false;
                    bringlyEntities.tblSaloonAppointments.Add(tblSaloonAppointment);
                    bringlyEntities.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SaloonAppointmentGuid"></param>
        /// <returns></returns>
        public bool DeleteSaloonAppointment(Guid SaloonAppointmentGuid)
        {
            tblSaloonAppointment tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.SaloonAppointmentGuid == SaloonAppointmentGuid).FirstOrDefault();
            tblSaloonAppointment.IsDeleted = true;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public bool IsSaloonBooked(BusinessObject businessObject)
        {
            tblSaloonAppointment tblSaloonAppointment = new tblSaloonAppointment();
            if (businessObject.SaloonAppointmentGuid != null && businessObject.SaloonAppointmentGuid != Guid.Empty)
            {
                tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false
                && x.FK_BusinessGuid == businessObject.BusinessGuid && x.FK_SaloonTimeGuid == businessObject.SaloonTimeGuid
                && x.AppointmentDate == businessObject.AppointmentDate
                && x.SaloonAppointmentGuid != businessObject.SaloonAppointmentGuid).FirstOrDefault();
            }
            else
            {
                tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false
                && x.FK_SaloonTimeGuid == businessObject.SaloonTimeGuid && x.AppointmentDate == businessObject.AppointmentDate
                && x.FK_BusinessGuid == businessObject.BusinessGuid).FirstOrDefault();
            }

            if (tblSaloonAppointment == null || tblSaloonAppointment.SaloonAppointmentGuid == null
                || tblSaloonAppointment.SaloonAppointmentGuid == Guid.Empty)
            { return true; }
            else { return false; }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="LatestPage"></param>
        /// <returns></returns>
        public MyBusiness GetAppointmentByUserGuid(Guid guid, int LatestPage = 0)
        {
            MyBusiness MyBusiness = new MyBusiness();
            MyBusiness.PageSize = PageSize;
            MyBusiness.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            MyBusiness.SortBy = SortBy;
            if (guid != null && guid != Guid.Empty)
            {
                MyBusiness.BusinessObjects = bringlyEntities.tblSaloonAppointments
                    .Where(x => x.SaloonAppointmentGuid == guid && x.IsDeleted == false && x.FK_UserGuid == UserVariables.LoggedInUserGuid)
                .Select(c => new BusinessObject
                {
                    SaloonAppointmentGuid = c.SaloonAppointmentGuid,
                    AppointmentDate = c.AppointmentDate,
                    UserGuid = c.FK_UserGuid,
                    IsApproved = c.IsApproved,
                    BusinessGuid = c.FK_BusinessGuid,
                    BusinessTypeGuid = bringlyEntities.tblBranches.Where(x => x.BranchGuid == c.FK_BusinessGuid).FirstOrDefault()
                        .FK_BusinessTypeGuid,
                    SaloonTimeGuid = c.FK_SaloonTimeGuid,
                    SaloonTime = c.SaloonTime,
                    BusinessName = bringlyEntities.tblBranches.Where(z => z.BranchGuid == c.FK_BusinessGuid).FirstOrDefault().BranchName
                }).ToList();
            }
            else
            {
                MyBusiness.BusinessObjects = bringlyEntities.tblSaloonAppointments
                    .Where(x => x.IsDeleted == false && x.FK_UserGuid == UserVariables.LoggedInUserGuid)
                    .Select(c => new BusinessObject
                    {
                        SaloonAppointmentGuid = c.SaloonAppointmentGuid,
                        AppointmentDate = c.AppointmentDate,
                        UserGuid = c.FK_UserGuid,
                        IsApproved = c.IsApproved,
                        BusinessGuid = c.FK_BusinessGuid,
                        SaloonTimeGuid = c.FK_SaloonTimeGuid,
                        BusinessTypeGuid = bringlyEntities.tblBranches.Where(x => x.BranchGuid == c.FK_BusinessGuid).FirstOrDefault().FK_BusinessTypeGuid,
                        SaloonTime = c.SaloonTime,
                        BusinessName = bringlyEntities.tblBranches.Where(z => z.BranchGuid == c.FK_BusinessGuid).FirstOrDefault()
                        .BranchName
                    }).ToList();
            }

            MyBusiness.SaloonSlotList = GetAllSaloonSlots();
            MyBusiness.TotalRecords = MyBusiness.BusinessObjects.Count;
            int Skip = 0;
            int Take = PageSize;
            if (MyBusiness.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((MyBusiness.CurrentPage * MyBusiness.PageSize) - MyBusiness.PageSize);

            MyBusiness.BusinessObjects = MyBusiness.BusinessObjects.Skip(Skip).Take(Take).ToList();

            return MyBusiness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public List<CustomSelectListItem> GetBranchList(Guid merchantGuid)
        {
            var data = bringlyEntities.tblBranches.Where(b => b.IsDeleted == false && b.FK_CreatedByGuid == merchantGuid)
                .Select(b => new CustomSelectListItem
                {
                    IsSelected = false,
                    Text = b.BranchName,
                    Value = b.BranchGuid.ToString()
                }).ToList();
            if (data == null)
                return new List<CustomSelectListItem>();
            return data;
        }

        #endregion

        #region Hotel Booking

        #endregion
    }
}