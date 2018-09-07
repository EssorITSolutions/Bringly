using Bringly.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class MyBusiness : Paging
    {
        public Guid BusinessGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessObject> BusinessObjects { get; set; }
        public string CityName { get; set; }
        public string BusinessName { get; set; }
        public Guid CityGuid { get; set; }
        public Guid SaloonTimeGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SaloonTimeMaster> SaloonSlotList { get; set; }
        //public int TotalRecords { get; set; }
        //public int PageSize { get; set; }
        //public int CurrentPage { get; set; }
        public Guid ReservationGuid { get; set; }
        public Guid RoomGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<RoomMaster> RoomList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomProperty
    {
        public Guid CustomPropertyGuid { get; set; }
        public Guid LocationGuid { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BusinessObject : BaseClasses.DomainBase
    {
        public Guid BusinessGuid { get; set; }

        [Required(ErrorMessage = "Please enter business name.")]
        public string BusinessName { get; set; }

        public string BusinessTypeName { get; set; }

        public Guid BusinessTypeGuid { get; set; }

        [Required(ErrorMessage = "Please enter p-Number.")]
        public string PNumber { get; set; }

        [StringLength(11, MinimumLength = 10, ErrorMessage = "Contact number must be atleast 10 characters long.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Invalid contact number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(100, ErrorMessage = "email address cannot be longer than 100 characters.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter address.")]
        public string Address { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PlaceId { get; set; }
       
        public Guid CityGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<City> CityList { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string PinCode { get; set; }

        public string BusinessImage { get; set; }

        public Guid CreatedByGuid { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Guid? DeletedByGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessType> BusinessTypeList { get; set; }

        public Guid? ManagerGuid { get; set; } = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<Contact> UserList { get; set; }

        public string Description { get; set; }

        public string OrderTiming { get; set; }

        public string PickUpTiming { get; set; }

        public string ServiceCharge { get; set; }

        public string ServiceTax { get; set; }

        public string FlatRate { get; set; }

        public string RateAfterKm { get; set; }

        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "CVR Number must be atleast 10 characters long.")]
        public string CVRNumber { get; set; }

        public string CustomProperty { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public List<Manager> Managers { get; set; }

        //[Required(ErrorMessage ="Title is required.")]
        public string AboutUsTitle { get; set; }
        //[Required(ErrorMessage = "Description is required.")]
        public string AboutUsDescription { get; set; }
        public Guid? AboutUsPageGuid { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class RoomMaster
    {
        public Guid RoomGuid { get; set; }
        public string RoomName { get; set; }
        public int RoomSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SaloonTimeMaster
    {
        public Guid SaloonTimeGuid { get; set; }
        public string SlotTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BusinessType
    {
        public Guid BusinessTypeGuid { get; set; }
        public string BusinessTypeName { get; set; }
    }
}
