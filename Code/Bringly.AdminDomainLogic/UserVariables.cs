using Bringly.AdminDomain.Enums; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bringly.AdminDomainLogic
{
    public class UserVariables
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        static object GetValue(int index, char split = '}')
        {
            string userData = HttpContext.Current.User.Identity.Name;
            string[] st = userData.Split(split);
            if (st.Length <= 0) throw new System.InvalidOperationException("Out of index");
            return st[index];
        }
        /// <summary>
        /// Login User ID -- 0
        /// </summary>
        public static Guid LoggedInUserGuid
        {
            get
            {
                return Guid.Parse(GetValue(0).ToString());
            }
        }
        /// <summary>
        /// Is LoggedIn User Super Admin -- 2
        /// </summary>
        public static bool IsSuperAdmin
        {
            get
            {
                return false;
                //return GetValue(2).ToType<bool>();
            }
        }
        public static UserRoles UserRole
        {
            get
            {
                return ((UserRoles)Convert.ToInt32(GetValue(2)));
            }
        }
        public static string UserName
        {
            get
            {
                return Convert.ToString(GetValue(3));
            }
        }
        /// <summary>
        /// Check current user is login or not
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                if (HttpContext.Current == null) return false;
                if (HttpContext.Current.User == null) return false;
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
    }
}
