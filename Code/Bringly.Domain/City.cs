using System;
using System.Collections.Generic;

namespace Bringly.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class ChooseCity
    {
        public List<City> Cities { get; set; }
        public City SelectedCity { get; set; }
        public List<City> TopCities { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class City
    {
        public Guid CityGuid { get; set; }
        public string CityName { get; set; } 
        public string CityUrlName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Country {
        public Guid CountryGuid { get; set; }
        public string CountryName { get; set; }
        public string CountriDisplayName { get; set; }
    }
}
