using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Domain.Enums.User;
namespace Bringly.Domain.User
{
    public class UserProfile : BaseClasses.DomainBase
    {
        public Guid UserGuid { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(100, ErrorMessage = "email address cannot be longer than 100 characters.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter valid email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string FullName { get; set; }
        public string PreferedCity { get; set; }
        public string MobileNumber { get; set; }
        public string SocialMediaUniqueId { get; set; }
        public string UserSocialMediaData { get; set; }
        public string UserRegistrationType { get; set; }
        public string ProfileImage { get; set; }
        public List<UserAddress> UserAddresses { get; set; }
        public UserAddress BillingAddress
        {
            get
            {
                if (UserAddresses != null && UserAddresses.Where(x => x.AddressType == UserAddressType.Billing.ToString()).Count() > 0)
                {
                    return UserAddresses.Where(x => x.AddressType == UserAddressType.Billing.ToString()).First();
                }
                else
                {
                    return new UserAddress();
                }
            }
        }
        public UserAddress ShippingAddress
        {
            get
            {
                if (UserAddresses != null && UserAddresses.Where(x => x.AddressType == UserAddressType.Shipping.ToString()).Count() > 0)
                {
                    return UserAddresses.Where(x => x.AddressType == UserAddressType.Shipping.ToString()).First();
                }
                else
                {
                    return new UserAddress();
                }
            }
        }
        public List<City> Cities { get; set; }
        public UserRoles UserRole { get; set; }

    }

    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string UserPassword { get; set; }
    }
}
