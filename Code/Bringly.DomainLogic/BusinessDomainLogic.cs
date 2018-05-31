using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Bringly.Data;

using Bringly.Domain.Business;
using Bringly.Domain;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;

namespace Bringly.DomainLogic.Business
{
    public class BusinessDomainLogic : BaseClass.DomainLogicBase
    {
        #region Business Type

        public List<BusinessType> GetBusinessTypes()
        {
            return bringlyEntities.tblBusinessTypes.Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName= c.BusinessTypeName}).ToList();
        }
        public List<SaloonTimeMaster> GetAllSaloonSlots()
        {
            return bringlyEntities.tblSaloonTimeMasters.Select(c => new SaloonTimeMaster { SaloonTimeGuid = c.SaloonTimeGuid, SlotTime = c.SlotTime }).ToList();
        }
        public BusinessObject GetBusinessHeader()
        {
            BusinessObject businessObject = new BusinessObject();
            businessObject.BusinessTypeList = bringlyEntities.tblBusinessTypes.Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName = c.BusinessTypeName }).ToList();
            tblUser tblUsers = bringlyEntities.tblUsers.Where(x=>x.UserGuid==UserVariables.LoggedInUserGuid && x.PreferedCity==x.tblCity.CityGuid).FirstOrDefault();
            if (tblUsers != null)
            { businessObject.CityName = bringlyEntities.tblCities.Where(x => x.CityGuid == tblUsers.PreferedCity).FirstOrDefault().CityUrlName; }
            else
            { businessObject.CityName = bringlyEntities.tblCities.FirstOrDefault().CityUrlName; }
            //businessObject.CityName= bringlyEntities.tblBusinessTypes.Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName = c.BusinessTypeName }).ToList()
            return businessObject;
        }
        public BusinessType GetBusinessType(Guid BusinessTypeGuid)
        {
            return bringlyEntities.tblBusinessTypes.Where(c => c.BusinessTypeGuid == BusinessTypeGuid).Select(c => new BusinessType { BusinessTypeGuid = c.BusinessTypeGuid, BusinessTypeName = c.BusinessTypeName}).FirstOrDefault();
        }
        public bool AddBusinessType(BusinessType businessType)
        {
            if (!IsBusinessTypeExists(businessType))
            {
                bringlyEntities.tblBusinessTypes.Add(new tblBusinessType { BusinessTypeGuid = Guid.NewGuid(), BusinessTypeName = businessType.BusinessTypeName});
                bringlyEntities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool UpdateBusinessType(BusinessType businessType)
        {
            if (!IsBusinessTypeExists(businessType))
            {
                tblBusinessType businessTypeObject = bringlyEntities.tblBusinessTypes.Where(x => x.BusinessTypeGuid == businessType.BusinessTypeGuid).FirstOrDefault();
                businessTypeObject.BusinessTypeName = businessType.BusinessTypeName;
                bringlyEntities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsBusinessTypeExists(BusinessType businessType)
        {
            bool businessTypeexists = false;
            tblBusinessType businessTypeObject = bringlyEntities.tblBusinessTypes.Where(x => x.BusinessTypeName == businessType.BusinessTypeName && x.BusinessTypeGuid != businessType.BusinessTypeGuid).FirstOrDefault();
            if (businessTypeObject != null && !string.IsNullOrEmpty(businessTypeObject.BusinessTypeName))
            {
                businessTypeexists = true;
            }
            return businessTypeexists;
        }
        public bool DeleteBusinessType(Guid businessTypeGuid)
        {
            tblBusinessType businessType = bringlyEntities.tblBusinessTypes.Where(c => c.BusinessTypeGuid == businessTypeGuid).FirstOrDefault();
            bringlyEntities.Entry(businessType).State = EntityState.Deleted;
            bringlyEntities.SaveChanges();
            return true;
        }
        public BusinessObject Newbusiness()
        {
            BusinessObject BusinessObject = new BusinessObject();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            BusinessObject.BusinessTypeList = GetBusinessTypes();
            BusinessObject.CityList = commonDomainLogic.GetCities();
            BusinessObject.CVRNumber = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault().CVRNumber;
            BusinessObject.UserList = bringlyEntities.tblUsers.Where(x => x.ParentGuid == UserVariables.LoggedInUserGuid).Select(z=> new Contact {UserGuid=z.UserGuid,FullName=z.FullName }).ToList();
            return BusinessObject;
        }

        #endregion        

        #region Business Locations

        // Get Location by Guid
        public BusinessObject GetLocationByGuid(Guid businessGuid)
        {
            BusinessObject business = new BusinessObject();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            List<City> list = commonDomainLogic.GetCities();
            business = bringlyEntities.tblLocations.Where(x => x.LocationGuid == businessGuid && x.IsDeleted == false).Select(r => new BusinessObject
            {
                BusinessImage = r.LocationImage,
                BusinessGuid = r.LocationGuid,
                BusinessName = r.LocationName,
                CityGuid = r.CityGuid,
                BusinessTypeGuid = r.BusinessTypeGuid,
                PNumber = r.PNumber,
                Phone = r.Phone,
                PinCode = r.PinCode,
                CreatedByGuid = r.CreatedByGuid,
                ManagerGuid = r.ManagerUserGuid,
                Address = r.Address,
                Email = r.Email,
                OrderTiming = r.OrderTiming,
                PickUpTiming = r.PickUpTiming,
                ServiceCharge = r.ServiceCharge,
                ServiceTax = r.ServiceTax,
                FlatRate = r.FlatRate,
                RateAfterKm = r.RateAfterKm,
                Description = r.Description
            }).FirstOrDefault();
            business.CityList = list;
            business.BusinessTypeName = bringlyEntities.tblBusinessTypes.Where(c => c.BusinessTypeGuid == business.BusinessTypeGuid).FirstOrDefault().BusinessTypeName;
            business.CustomPropertyList = bringlyEntities.tblCustomProperties.Where(x => x.LocationGuid == businessGuid)
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
            return business;
        }
        // Business Update
        public string UpdateLocationProfile(BusinessObject BusinessObject)
        {
            tblBusiness business = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == BusinessObject.BusinessGuid).FirstOrDefault();
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
        public string UpdateLocation(BusinessObject BusinessObject)
        {
            tblLocation business = bringlyEntities.tblLocations.Where(x => x.LocationGuid == BusinessObject.BusinessGuid).FirstOrDefault();
            business.LocationName = BusinessObject.BusinessName;
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
        // Add Location
        public Guid AddLocation(BusinessObject BusinessObject)
        {
            tblBusiness business = bringlyEntities.tblBusinesses.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            tblLocation tblLocation = new tblLocation();

            tblLocation.LocationGuid = Guid.NewGuid();
            tblLocation.BusinessTypeGuid = business.BusinessTypeGuid;
            tblLocation.LocationName = BusinessObject.BusinessName;
            tblLocation.CityGuid = BusinessObject.CityGuid;
            tblLocation.PNumber = BusinessObject.PNumber;
            tblLocation.Phone = BusinessObject.Phone;
            tblLocation.PinCode = BusinessObject.PinCode;
            tblLocation.CreatedByGuid = UserVariables.LoggedInUserGuid;
            tblLocation.DateCreated = DateTime.Now;
            tblLocation.Address = BusinessObject.Address;
            tblLocation.Email = BusinessObject.Email;
            tblLocation.ManagerUserGuid = (BusinessObject.ManagerGuid==null || (!BusinessObject.ManagerGuid.HasValue || BusinessObject.ManagerGuid.Value== Guid.Empty))? UserVariables.LoggedInUserGuid: BusinessObject.ManagerGuid.Value;
            tblLocation.OrderTiming = BusinessObject.OrderTiming;
            tblLocation.PickUpTiming = BusinessObject.PickUpTiming;
            tblLocation.ServiceCharge = BusinessObject.ServiceCharge;
            tblLocation.ServiceTax = BusinessObject.ServiceTax;
            tblLocation.FlatRate = BusinessObject.FlatRate;
            tblLocation.RateAfterKm = BusinessObject.RateAfterKm;
            tblLocation.Description = BusinessObject.Description;
            tblLocation.IsDeleted = false;
            bringlyEntities.tblLocations.Add(tblLocation);
            bringlyEntities.SaveChanges();

            return tblLocation.LocationGuid;
        }       

        // Get all Location
        public List<BusinessObject> GetAllLocations()
        {
            List<BusinessObject> locationlist = new List<BusinessObject>();
            locationlist = bringlyEntities.tblLocations.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false).Select(z => new BusinessObject
            {
                BusinessGuid = z.LocationGuid,
                BusinessName = z.LocationName,
                CityGuid = z.CityGuid,
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
                ManagerGuid=z.ManagerUserGuid
            }).ToList();
            return locationlist;
        }
        // Delete Location
        public bool DeleteLocation(Guid businessGuid)
        {
            tblLocation tblLocation = bringlyEntities.tblLocations.Where(x => x.LocationGuid == businessGuid && x.IsDeleted == false).FirstOrDefault();
            tblLocation.IsDeleted = true;
            bringlyEntities.SaveChanges();
            return true;
        }
        public MyBusiness GetBusinessByCity(City _city, Nullable<Guid> BusinessTypeGuid, int LatestPage = 0)
        {
            MyBusiness _businessSearch = new MyBusiness();
            _businessSearch.PageSize = PageSizeBusiness;    
            _businessSearch.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            _businessSearch.SortBy = SortBy;

            if (BusinessTypeGuid !=null && BusinessTypeGuid != Guid.Empty)
            {
                //_businessSearch.BusinessTypeGuid = BusinessTypeGuid.Value;
                _businessSearch.BusinessObjects = bringlyEntities.tblLocations.Where(x => x.IsDeleted == false && x.BusinessTypeGuid== BusinessTypeGuid).Select(r => 
                new BusinessObject {
                    BusinessImage = r.LocationImage, BusinessGuid = r.LocationGuid, BusinessName = r.LocationName, CityGuid = r.CityGuid, CityName = _city.CityName,
                    IsFavorite = false }).ToList();
                _businessSearch.BusinessName = bringlyEntities.tblBusinessTypes.Where(x => x.BusinessTypeGuid == BusinessTypeGuid.Value).FirstOrDefault().BusinessTypeName;
            }
            else {
                //_businessSearch.BusinessTypeGuid=bringlyEntities.tblBusinessTypes.FirstOrDefault().BusinessTypeGuid;
                _businessSearch.BusinessObjects = bringlyEntities.tblLocations.Where(x => x.IsDeleted == false).Select(r => new BusinessObject { BusinessImage = r.LocationImage, BusinessGuid = r.LocationGuid, BusinessName = r.LocationName, CityGuid = r.CityGuid, CityName = _city.CityName, IsFavorite = false }).ToList();
                _businessSearch.BusinessName = "Restaurant";
            }
            
            if (_city.CityGuid != null)
            {
                _businessSearch.BusinessObjects = _businessSearch.BusinessObjects.Where(s => s.CityGuid == _city.CityGuid).ToList();
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

        //public BusinessObject GetLocationByBusinessGuid(Guid businessGuid)
        //{
        //    BusinessObject business = new BusinessObject();
        //    CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
        //    List<City> list = commonDomainLogic.GetCities();
        //    business = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == businessGuid).Select(r => new BusinessObject
        //    {
        //        BusinessImage = r.BusinessImage
        //        ,                BusinessGuid = r.BusinessGuid
        //        ,                BusinessName = r.BusinessName
        //        ,                CityGuid = r.CityGuid
        //        ,                BusinessTypeGuid = r.BusinessTypeGuid
        //        ,                PNumber = r.PNumber
        //        ,                Phone = r.Phone
        //        ,                PinCode = r.PinCode
        //        ,                CreatedByGuid = r.CreatedByGuid
        //        ,                ManagerGuid = r.ManagerUserGuid
        //        ,                Address = r.Address
        //        ,                Email = r.Email
        //        ,                OrderTiming = r.OrderTiming
        //        ,                PickUpTiming = r.PickUpTiming
        //        ,                ServiceCharge = r.ServiceCharge
        //        ,                ServiceTax = r.ServiceTax
        //        ,                FlatRate = r.FlatRate
        //        ,                RateAfterKm = r.RateAfterKm
        //        ,                Description = r.Description
        //    }).FirstOrDefault();
        //    business.CityList = list;
        //    business.CityName = business.CityGuid != Guid.Empty ? bringlyEntities.tblCities.Where(x => x.CityGuid == business.CityGuid).FirstOrDefault().CityName : "";
        //    business.CVRNumber = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault().CVRNumber;
        //    return business;
        //}

        #endregion

        #region Location Custom Property

        // Add Custom Property
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
            //foreach (var itm in list.Where(x=>x.CustomPropertyGuid!=Guid.Empty))
            //{
            //    customProperty = bringlyEntities.tblCustomProperties.Where(x => x.CustomPropertyGuid == itm.CustomPropertyGuid).FirstOrDefault();
            //    customProperty.Field = itm.Field;
            //    customProperty.Value = itm.Value;
            //    bringlyEntities.SaveChanges();
            //}
            ////bringlyEntities.tblCustomProperties.AddRange(customPropertyList);
            ////customPropertyList = new List<tblCustomProperty>();
            //foreach (var itm in list.Where(x => x.CustomPropertyGuid == Guid.Empty))
            //{
            //    customProperty = new tblCustomProperty();
            //    customProperty.LocationGuid = itm.LocationGuid;
            //    customProperty.CustomPropertyGuid = Guid.NewGuid();
            //    customProperty.Field = itm.Field;
            //    customProperty.Value = itm.Value;
            //    customPropertyListNew.Add(customProperty);
            //}
            bringlyEntities.tblCustomProperties.AddRange(customPropertyListNew);
            bringlyEntities.SaveChanges();
           
            return true;
        }
        // Delete Custom Property
        public bool DeleteCustomProperty(Guid customFieldGuid)
        {
            tblCustomProperty customProperty = bringlyEntities.tblCustomProperties.Where(x => x.CustomPropertyGuid == customFieldGuid).FirstOrDefault();
            bringlyEntities.Entry(customProperty).State = EntityState.Deleted;
            bringlyEntities.SaveChanges();
            return true;
        }

        #endregion

        #region Saloon/ Spa

        public bool MakeUpdateAppointment(BusinessObject businessObject)
        {
            if (businessObject.SaloonAppointmentGuid != null && businessObject.SaloonAppointmentGuid != Guid.Empty)
            {
                try
                {
                    tblSaloonAppointment tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.SaloonAppointmentGuid == businessObject.SaloonAppointmentGuid).FirstOrDefault();
                    tblSaloonAppointment.SaloonTime = businessObject.SaloonTime;
                    tblSaloonAppointment.SaloonTimeGuid = businessObject.SaloonTimeGuid;
                    tblSaloonAppointment.AppointmentDate = businessObject.AppointmentDate;
                    tblSaloonAppointment.ModifiedBy = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.ModifiedDate = DateTime.Now;
                    tblSaloonAppointment.IsApproved = businessObject.IsApproved;
                    bringlyEntities.SaveChanges();
                }
                catch (Exception ex)
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
                    tblSaloonAppointment.BusinessGuid = businessObject.BusinessGuid;
                    tblSaloonAppointment.UserGuid = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.CreatedByGuid = UserVariables.LoggedInUserGuid;
                    tblSaloonAppointment.IsApproved = false;
                    tblSaloonAppointment.SaloonTime = businessObject.SaloonTime;
                    tblSaloonAppointment.SaloonTimeGuid = businessObject.SaloonTimeGuid;
                    tblSaloonAppointment.AppointmentDate = businessObject.AppointmentDate;
                    tblSaloonAppointment.DateCreated = DateTime.Now;
                    tblSaloonAppointment.IsDeleted = false;
                    bringlyEntities.tblSaloonAppointments.Add(tblSaloonAppointment);
                    bringlyEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        public bool DeleteSaloonAppointment(Guid SaloonAppointmentGuid)
        {
            tblSaloonAppointment tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.SaloonAppointmentGuid == SaloonAppointmentGuid).FirstOrDefault();
            tblSaloonAppointment.IsDeleted = true;
            bringlyEntities.SaveChanges();
            return true;
        }
        public bool IsSaloonBooked(BusinessObject businessObject)
        {
            tblSaloonAppointment tblSaloonAppointment = new tblSaloonAppointment();
            if (businessObject.SaloonAppointmentGuid != null && businessObject.SaloonAppointmentGuid != Guid.Empty)
            {
                tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.BusinessGuid == businessObject.BusinessGuid && x.SaloonTimeGuid == businessObject.SaloonTimeGuid && x.AppointmentDate == businessObject.AppointmentDate && x.SaloonAppointmentGuid!= businessObject.SaloonAppointmentGuid).FirstOrDefault();
            }
            else
            {
                tblSaloonAppointment = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.SaloonTimeGuid == businessObject.SaloonTimeGuid && x.AppointmentDate == businessObject.AppointmentDate && x.BusinessGuid==businessObject.BusinessGuid).FirstOrDefault();
            }
            
            if (tblSaloonAppointment == null || tblSaloonAppointment.SaloonAppointmentGuid == null || tblSaloonAppointment.SaloonAppointmentGuid == Guid.Empty)
            {return true; }
            else { return false; }
            
        }

        public MyBusiness GetAppointmentByUserGuid(Guid guid,int LatestPage=0)
        {
            MyBusiness MyBusiness = new MyBusiness();
            MyBusiness.PageSize = PageSize;
            MyBusiness.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            MyBusiness.SortBy = SortBy;
            if (guid != null && guid != Guid.Empty)
            {
                MyBusiness.BusinessObjects = bringlyEntities.tblSaloonAppointments.Where(x => x.SaloonAppointmentGuid == guid && x.IsDeleted == false && x.UserGuid==UserVariables.LoggedInUserGuid).Select(
                    c => new BusinessObject
                    {
                        SaloonAppointmentGuid=c.SaloonAppointmentGuid,
                        AppointmentDate=c.AppointmentDate,
                        UserGuid=c.UserGuid,
                        IsApproved=c.IsApproved,
                        BusinessGuid=c.BusinessGuid,
                        BusinessTypeGuid = bringlyEntities.tblLocations.Where(x=>x.LocationGuid==c.BusinessGuid).FirstOrDefault().BusinessTypeGuid,
                        SaloonTimeGuid =c.SaloonTimeGuid,
                        SaloonTime=c.SaloonTime,
                        BusinessName = bringlyEntities.tblLocations.Where(z => z.LocationGuid == c.BusinessGuid).FirstOrDefault().LocationName
                    }).ToList();
            }
            else {
                MyBusiness.BusinessObjects = bringlyEntities.tblSaloonAppointments.Where(x => x.IsDeleted == false && x.UserGuid == UserVariables.LoggedInUserGuid).Select(
                                  c => new BusinessObject
                                  {
                                      SaloonAppointmentGuid = c.SaloonAppointmentGuid,
                                      AppointmentDate = c.AppointmentDate,
                                      UserGuid = c.UserGuid,
                                      IsApproved = c.IsApproved,
                                      BusinessGuid = c.BusinessGuid,
                                      SaloonTimeGuid = c.SaloonTimeGuid,
                                      BusinessTypeGuid = bringlyEntities.tblLocations.Where(x => x.LocationGuid == c.BusinessGuid).FirstOrDefault().BusinessTypeGuid,
                                      SaloonTime = c.SaloonTime,
                                      BusinessName=bringlyEntities.tblLocations.Where(z=>z.LocationGuid==c.BusinessGuid).FirstOrDefault().LocationName
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

        #endregion

        #region Hotel Booking

        //public List<RoomMaster> GetAllRooms()
        //{
        //    return bringlyEntities.tblRoomMasters.Select(c => new RoomMaster { RoomGuid = c.RoomGuid, RoomName = c.RoomName,RoomSize=c.RoomSize }).ToList();
        //}


        #endregion
    }
}