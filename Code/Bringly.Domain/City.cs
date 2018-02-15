using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class ChooseCity
    {
        public List<City> Cities { get; set; }
        public City SelectedCity { get; set; }

    }

    public class City
    {
        public Guid CityGuid { get; set; }
        public string CityName { get; set; } 
        public string CityUrlName { get; set; }
    }

}
