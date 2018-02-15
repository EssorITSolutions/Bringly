using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.Enums.User
{
    public enum UserRoles
    {
        [Display(Name = "System Admin")]
        SuperAdmin = 1,
        [Display(Name = "Caller")]
        Buyer = 2,
        [Display(Name = "Merchant")]
        Merchant = 3,
    }
}
