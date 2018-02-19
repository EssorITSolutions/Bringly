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
    public class UserDomainLogic : BaseClass.DomainLogicBase
    {
        public Message UserLogin(UserLogin userLogin)
        {
            Message message = new Message();
            tblUser user = bringlyEntities.tblUsers.Where(u => u.EmailAddress == userLogin.UserName && u.Password == userLogin.UserPassword && u.IsDeleted == false && u.UserRegistrationType == 1).FirstOrDefault();
            if (user != null && user.IsActive)
            {
                AuthencationTicket(user);
                message.MessageType = AdminDomain.Enums.MessageType.Success;
            }
            else if (user != null && user.IsActive == false)
            {
                message.MessageType = AdminDomain.Enums.MessageType.Error;
                message.MessageText = "Your account has been deactivated, Please contact administrator";
            }
            else
            {
                message.MessageType = AdminDomain.Enums.MessageType.Error;
                message.MessageText = "Wrong username or password";
            }
            return message;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        private void AuthencationTicket(tblUser user)
        {
            string userData =
                user.UserGuid + "}" //0
                + user.EmailAddress + "}" //1
                + (user.UserRegistrationType).ToString() + "}"//2  
                + user.FullName;
            FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, userData, DateTime.Now, DateTime.Now.AddHours(5), false, "Bringly", FormsAuthentication.FormsCookiePath);
            string st = FormsAuthentication.Encrypt(tkt);
            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, st) { HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(ck);
        }
        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Where(c => c.IsDeleted == false).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).ToList();
        }
        public City GetCity(Guid cityGuid)
        {
            return bringlyEntities.tblCities.Where(c => c.CityGuid == cityGuid).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).FirstOrDefault();
        }
        public bool AddUpdateCity(string cityName,Guid cityGuid)
        {
            tblCity cityObject = bringlyEntities.tblCities.Where(x => x.CityGuid == cityGuid).FirstOrDefault();
            if (cityObject != null && !string.IsNullOrEmpty(cityObject.CityName))
            {
                cityObject.CityName = cityName;
                bringlyEntities.SaveChanges();
            }
            else {
                cityObject = new tblCity();            
                cityObject = bringlyEntities.tblCities.Add(new tblCity { CityGuid = Guid.NewGuid(), CityName = cityName, CityUrlName = Regex.Replace(cityName, @"[^0-9a-zA-Z]+", "") });
                bringlyEntities.SaveChanges();                
            }
            return true;
        }
        public bool IsCityExists(string cityName,Guid guid)
        {
            bool cityexists = false;
            string cityurl = Regex.Replace(cityName, @"[^0-9a-zA-Z]+", "");
            tblCity cityObject = bringlyEntities.tblCities.Where(x => x.CityUrlName== cityurl && x.CityGuid!= guid).FirstOrDefault();
            if (cityObject != null && !string.IsNullOrEmpty(cityObject.CityName))
            {
                cityexists = true;
            }
            return cityexists;
        }
        public bool DeleteCityLogic(Guid cityGuid)
        {
            tblCity city = bringlyEntities.tblCities.Where(c => c.CityGuid == cityGuid).FirstOrDefault();            
            bringlyEntities.tblCities.Remove(city);
            return true;
        }
    }
}
