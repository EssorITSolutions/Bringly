using System;
using System.Collections.Generic;
using System.Text;
using Bringly.AdminDomain;
using Bringly.Data;
using System.Linq;
namespace Bringly.AdminDomainLogic
{
    public class CommonDomainLogic : BaseClass.DomainLogicBase
    {
        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Select(t => new City { CityGuid = t.CityGuid, CityName = t.CityName, CityUrlName = t.CityUrlName }).ToList();
        }
        public Guid FindCityGuid(string CityUrlName)
        {
            tblCity _city = bringlyEntities.tblCities.Where(x => x.CityUrlName == CityUrlName).FirstOrDefault();
            return _city != null ? _city.CityGuid : Guid.Empty;
        }
        public List<City> GetCityByGUID(Guid _cityguid)
        {
            return bringlyEntities.tblCities.Where(x => x.CityGuid == _cityguid).Select(t => new City { CityGuid = t.CityGuid, CityName = t.CityName, CityUrlName = t.CityUrlName }).ToList();
        }
    }
}
