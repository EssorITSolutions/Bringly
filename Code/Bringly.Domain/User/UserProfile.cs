using Bringly.Domain.Business;
using Bringly.Domain.Enums.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        
        [StringLength(11, MinimumLength =10, ErrorMessage = "Contact number must be atleast 10 characters long.")]
        [RegularExpression(@"^\d{1,}$", ErrorMessage = "Invalid contact number.")]
        public string MobileNumber { get; set; }

        public string SocialMediaUniqueId { get; set; }

        public string UserSocialMediaData { get; set; }

        public string UserRegistrationType { get; set; }

        public string ProfileImage { get; set; }

        public Guid? BusinessTypeGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessType> BusinessTypeList { get; set; }

        public string CompanyorIndividual { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UserAddress> UserAddresses { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public List<City> Cities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserRoles UserRole { get; set; }

        [RegularExpression(@"^\d{10,11}$",ErrorMessage = "CVR Number must be atleast 10 characters long.")]
        public string CVRNumber { get; set; }
        public string RoleName { get; set; }
        public Guid RoleGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Role> RolesList { get; set; }

        public List<Country> Countries { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        public Guid RoleGuid { get; set; }
        public string RoleName { get; set; }
    }

   /// <summary>
   /// 
   /// </summary>
    public class SocialLogin
    {
        public string EmailAddress { get; set; }
        public bool IsGoogleLogin { get; set; }
        public string FullName { get; set; }
        public string GoogleProviderUserId { get; set; }
        public string FacebookProviderUserId { get; set; }
        public string GoogleUserProfileImageUrl { get; set; }
        public string FacebookUserProfileImageUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string UserPassword { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Contact
    {
        public Guid UserGuid { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserRegistration
    {
        // still working
        public Guid UserGuid { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(100, ErrorMessage = "email address cannot be longer than 100 characters.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter valid email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter mobile number.")]
        [RegularExpression(@"^\d{1,}$", ErrorMessage = "Invalid contact number.")]
        public string MobileNumber { get; set; }
        public string UserRegistrationType { get; set; }
        public string ProfileImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UserAddress> UserAddresses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserAddress BillingAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserAddress ShippingAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<City> Cities { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Country> Countries { get; set; }
        public Guid CityGuid { get; set; }
        public string UserRole { get; set; }
        public string CompanyorIndividual { get; set; }
        public Guid? BusinessTypeGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessType> BusinessTypeList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManageUserProfiles : Paging
    {
        public List<UserProfile> UserProfiles { get; set; }
    }

    //public class Dashboard
    //{
    //    public int OrderCompleted { get; set; }
    //    public int OrderInProgress { get; set; }
    //    public int OrderCancelled { get; set; }
    //    public int OrderRejected { get; set; }
    //    public List<Email> EmailList { get; set; }
    //    public List<RestaurantReview> ReviewList { get; set; }
    //    public List<BusinessObject> FavouriteList { get; set; }
    //    public string TotalSales { get; set; }
    //}
}
