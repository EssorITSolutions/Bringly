using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.Enums.User
{
    /// <summary>
    /// User roles for the system
    /// </summary>
    public enum UserRoles
    {
        [Display(Name = "System Admin")]
        SuperAdmin = 1,
        [Display(Name = "Buyer")]
        Buyer = 2,
        [Display(Name = "Merchant")]
        Merchant = 3,

        [Display(Name = "Manager")]
        Manager = 4,
    }
}
