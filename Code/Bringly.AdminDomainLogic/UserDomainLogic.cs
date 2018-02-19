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
  
    }
}
