using Bringly.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain.Business
{

    public class MyBusiness : Paging
    {      
        public Guid BusinessGuid { get; set; }
        public List<BusinessObject> BusinessObjects { get; set; }
        public string CityName { get; set; }
        public string BusinessName { get; set; }
        public Guid CityGuid { get; set; }
        public Guid SaloonTimeGuid { get; set; }
        public List<SaloonTimeMaster> SaloonSlotList { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public Guid ReservationGuid { get; set; }
        public Guid RoomGuid { get; set; }
        public List<RoomMaster> RoomList { get; set; }
    }

    public class CustomProperty
    {
        public Guid CustomPropertyGuid { get; set; }
        public Guid LocationGuid{get;set;}
        public string Field { get; set; }
        public string Value { get; set; }
        
    }

    public class BusinessObject : BaseClasses.DomainBase
    {
        public Guid BusinessGuid { get; set; }
        [Required(ErrorMessage = "Please enter business name.")]
        public string BusinessName { get; set; }
        public string BusinessTypeName { get; set; }
        public Guid BusinessTypeGuid { get; set; }
        [Required(ErrorMessage = "Please enter p-Number.")]
        public string PNumber { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter email id.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter address.")]
        public string Address { get; set; }
        public Guid CityGuid { get; set; }
        public List<City> CityList { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string BusinessImage { get; set; }
        public Guid CreatedByGuid { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedByGuid { get; set; }
        public List<BusinessType> BusinessTypeList { get; set; }
        public Guid? ManagerGuid { get; set; } = Guid.Empty;
        public List<Contact> UserList { get; set; }

        public string Description { get; set; }
        public string OrderTiming { get; set; }
        public string PickUpTiming { get; set; }
        public string ServiceCharge { get; set; }
        public string ServiceTax { get; set; }
        public string FlatRate { get; set; }
        public string RateAfterKm { get; set; }
        public string CVRNumber { get; set; }
        public string CustomProperty { get; set; }
        public List<CustomProperty> CustomPropertyList { get; set; }
        public bool IsFavorite { get; set; }

        public Guid SaloonAppointmentGuid { get; set; }
        public Guid SaloonTimeGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string SaloonTime { get; set; }
        public string AppointmentDate { get; set; }
        public bool IsApproved { get; set; }

        public Guid ReservationGuid { get; set; }
        public Guid RoomGuid { get; set; }
        public int NoOfGuest { get; set; }
        public string ReservationStartDate { get; set; }
        public string ReservationEndDate { get; set; }


    }
    public class RoomMaster
    {
        public Guid RoomGuid { get; set; }
        public string RoomName { get; set; }
        public int RoomSize { get; set; }
    }
    public class SaloonTimeMaster
    {
        public Guid SaloonTimeGuid { get; set; }
        public string SlotTime { get; set; }
    }
    public class BusinessType
    {
        public Guid BusinessTypeGuid { get; set; }
        public string BusinessTypeName { get; set; }
    }
}
