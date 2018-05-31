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
        /// 
        #region User Role
        public List<UserRole> GetRoles()
        {
            return bringlyEntities.tblRoles.Where(c => c.CreatedByGuid == UserVariables.LoggedInUserGuid).Select(c => new UserRole { RoleGuid = c.RoleGuid, RoleName = c.RoleName, DateCreated = c.DateCreated }).ToList();
        }
        public UserRole GetRole(Guid roleGuid)
        {
            return bringlyEntities.tblRoles.Where(c => c.RoleGuid == roleGuid).Select(c => new UserRole { RoleGuid = c.RoleGuid, RoleName = c.RoleName, DateCreated = c.DateCreated }).FirstOrDefault();
        }
        public bool AddRole(UserRole role)
        {
            if (!IsRolexists(role))
            {
                bringlyEntities.tblRoles.Add(new tblRole { RoleGuid = Guid.NewGuid(), RoleName = role.RoleName, DateCreated = DateTime.Now, CreatedByGuid = UserVariables.LoggedInUserGuid });
                bringlyEntities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateRole(UserRole role)
        {
            if (!IsRolexists(role))
            {
                tblRole roleObject = bringlyEntities.tblRoles.Where(x => x.RoleGuid == role.RoleGuid).FirstOrDefault();
                roleObject.RoleName = role.RoleName;
                roleObject.ModifiedBy = UserVariables.LoggedInUserGuid;
                roleObject.ModifiedDate = DateTime.Now;
                bringlyEntities.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsRolexists(UserRole role)
        {
            bool roleExists = false;
            tblRole roleObject = bringlyEntities.tblRoles.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.RoleName == role.RoleName&& x.RoleGuid != role.RoleGuid).FirstOrDefault();
            if (roleObject != null && !string.IsNullOrEmpty(roleObject.RoleName))
            {
                roleExists = true;
            }
            return roleExists;
        }
        public bool DeleteRoleLogic(Guid roleGuid)
        {
            tblRole role = bringlyEntities.tblRoles.Where(c => c.RoleGuid == roleGuid).FirstOrDefault();
            bringlyEntities.Entry(role).State = System.Data.Entity.EntityState.Deleted;
            bringlyEntities.SaveChanges();
            return true;
        }


        #endregion




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
