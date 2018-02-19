using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.AdminDomain
{
    public class City : BaseClasses.DomainBase
    {
        public Guid CityGuid { get; set; }
        public string CityName { get; set; }
        public string CityUrlName { get; set; }
    }

}
