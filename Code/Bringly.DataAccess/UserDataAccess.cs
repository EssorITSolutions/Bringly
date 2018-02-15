using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Domain.User;
using System.Data.Entity;
namespace Bringly.DataAccess
{
    public class UserDataAccess : BaseClasses.DataAccessBase

    {
        public void GetUsers(Guid userGuid)
        {
            Users userResult = new Users();


            //var usr = _bringlyDbContext.tblUsers.Where(x => x.UserGuid == userGuid).Include(x => x.tblUsersAddresses);
            //if (tbluser != null)
            //{
            //    userResult.UserGuid = u.UserGuid;
            //    userResult.EmailAddress = u.EmailAddress;
            //    userResult.pho

            //}

        }
    }
}
