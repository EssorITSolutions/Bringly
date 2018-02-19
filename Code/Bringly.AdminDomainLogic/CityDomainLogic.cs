using System;
using Bringly.AdminDomain;
using Bringly.AdminDomain.Common;
using Bringly.Data;
using System.Linq;
using Bringly.AdminDomain.Enums;
using System.Web.Security;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bringly.AdminDomainLogic
{
    public class CityDomainLogic : BaseClass.DomainLogicBase
    {
        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Where(c => c.IsDeleted == false).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).ToList();
        }
        public City GetCity(Guid cityGuid)
        {
            return bringlyEntities.tblCities.Where(c => c.CityGuid == cityGuid).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).FirstOrDefault();
        }
        public bool AddCity(City city)
        {

            tblCity cityObject = new tblCity();
            string cityUrlName = Regex.Replace(city.CityName, @"[^0-9a-zA-Z]+", "-").Replace("--", "-");
            cityObject = bringlyEntities.tblCities.Add(new tblCity { CityGuid = Guid.NewGuid(), CityName = city.CityName, CityUrlName = cityUrlName });
            bringlyEntities.SaveChanges();
            return true;

        }
        public bool UpdateCity(City city)
        {
            tblCity cityObject = bringlyEntities.tblCities.Where(x => x.CityGuid == city.CityGuid).FirstOrDefault();
            cityObject.CityName = city.CityName;
            bringlyEntities.SaveChanges(); 
            return true;
        }

        public bool IsCityExists(City city)
        {
            bool cityexists = false;
            tblCity cityObject = bringlyEntities.tblCities.Where(x => x.IsDeleted == false && x.CityName == city.CityName && x.CityGuid != city.CityGuid).FirstOrDefault();
            if (cityObject != null && !string.IsNullOrEmpty(cityObject.CityName))
            {
                cityexists = true;
            }
            return cityexists;
        }
        public bool DeleteCityLogic(Guid cityGuid)
        {
            tblCity city = bringlyEntities.tblCities.Where(c => c.CityGuid == cityGuid).FirstOrDefault();
            city.IsDeleted = true;
            bringlyEntities.SaveChanges();
            return true;
        }
    }
}
