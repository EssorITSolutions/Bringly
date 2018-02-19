using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.AdminDomain
{
    public class City : BaseClasses.DomainBase
    {
        public Guid CityGuid { get; set; }
        [Required(ErrorMessage = "Please enter city")]
        public string CityName { get; set; }
        public string CityUrlName { get; set; }
    }

}
