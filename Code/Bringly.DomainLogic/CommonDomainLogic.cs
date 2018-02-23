using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Domain;
using Bringly.Data;
using Bringly.Domain.User;
using Bringly.DomainLogic.User;
using Bringly.Domain.Enums;

namespace Bringly.DomainLogic
{
    public class CommonDomainLogic : BaseClass.DomainLogicBase
    {
        public static string DefaultProfileImage = "profile.png";
        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Where(c => c.IsDeleted == false).Select(t => new City { CityGuid = t.CityGuid, CityName = t.CityName, CityUrlName = t.CityUrlName }).OrderBy(c => c.CityName).ToList();
        }
        public List<City> GetTopCities(City PreferredCity = null)
        {
            int takeTop = 6;
            Guid preferredCityGuid = Guid.Empty;
            if (PreferredCity != null)
            {
                takeTop = takeTop - 1;
                preferredCityGuid = PreferredCity.CityGuid;
            }

            List<City> Cities = bringlyEntities.tblCities.Where(c => c.IsDeleted == false && (preferredCityGuid == Guid.Empty || c.CityGuid != preferredCityGuid)).OrderByDescending(c => c.DateCreated).Select(t => new City { CityGuid = t.CityGuid, CityName = t.CityName, CityUrlName = t.CityUrlName }).Take(takeTop).ToList();
            if (PreferredCity != null)
            {
                Cities.Add(PreferredCity);
            }
            return Cities;
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
        public City GetPreferedCity()
        {
            UserDomainLogic _userDomainLogic = new UserDomainLogic();
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            City _City = new City();
            UserProfile _FindUser = _userDomainLogic.FindUser(UserVariables.LoggedInUserGuid);
            if (_FindUser == null && string.IsNullOrEmpty(_FindUser.PreferedCity))
            {
                _City = new City();
            }
            else
            {
                _City = commonDomainLogic.GetCityByGUID(new Guid(_FindUser.PreferedCity)).FirstOrDefault();
            }
            return _City;
        }
        public static string GetImagePath(ImageType? imagetype, string ImageName)
        {
            string retstring = "";
            switch (imagetype)
            {
                case ImageType.Item:
                    retstring = "/Upload/Item/" + ImageName;
                    break;
                case ImageType.User:
                    retstring = "/Upload/User/" + ImageName;
                    break;
                case ImageType.Restaurant:
                    retstring = "/Upload/Restaurant/" + ImageName;
                    break;
            }
            return retstring;
            //return "~/Upload" + imagetype + "/" + ImageName;
        }
        
    }
}
