using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.AdminDomain.Enums
{
    public enum ImageType
    {
        User,
        Restaurant,
        Item
    }
    public enum MessageType
    {
        Success, Error, Info
    }
    public enum UserRoles
    {
        SuperAdmin = 1,
        Buyer = 2,
        Merchant = 3,
    }
}
