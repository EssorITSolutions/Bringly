using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain.User
{
    public class UserAddress : BaseClasses.DomainBase
    {
        public Guid UserAddressGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select city")]
        public Guid CityGuid { get; set; }
        public string PostCode { get; set; }
        public string AddressType { get; set; }
        public string CityName { get; set; }

    }
}
