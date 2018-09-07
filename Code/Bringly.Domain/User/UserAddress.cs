using System;
using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.User
{
    public class UserAddress : BaseClasses.DomainBase
    {
        public Guid UserAddressGuid { get; set; }
        public Guid UserGuid { get; set; }
        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select city")]
        public Guid CityGuid { get; set; }

        [Required(ErrorMessage = "Please enter post code")]
        public string PostCode { get; set; }
        public string AddressType { get; set; }
        public string CityName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PlaceId { get; set; }

        //[Required(ErrorMessage = "Please Select Country")]
        public string Country { get; set; }
        public Guid? CountryGuid { get; set; }
    }
}
