//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Bringly.Domain.User
//{
//    public class ApplicationUserDetails : BaseClasses.DomainBase
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public ApplicationUserDetails()
//        {
//            ApplicationUserDetailGuid = Guid.NewGuid();           
//        }

//        [Key]
//        public Guid ApplicationUserDetailGuid { get; set; }

//        [ForeignKey("ApplicationUsers")]
//        public Guid ApplicationUserGuid { get; set; }

//        public virtual ApplicationUsers ApplicationUsers { get; set; }

//        [Required(ErrorMessage = "Please enter mobile number")]
//        [StringLength(256, ErrorMessage = "Mobile Number cannot be longer than 15 characters")]
//        [Column(TypeName = "varchar")]
//        public string MobileNumber { get; set; }


//        [Required(ErrorMessage = "Please enter address")]
//        [StringLength(256, ErrorMessage = "Address cannot be longer than 256 characters")]
//        [Column(TypeName = "varchar")]
//        public string Address { get; set; }


//        [Required(ErrorMessage = "Please enter city")]
//        [StringLength(20, ErrorMessage = "City cannot be longer than 20 characters")]
//        [Column(TypeName = "varchar")]
//        public string City { get; set; }

//        [Required(ErrorMessage = "Please enter postcode")]
//        [StringLength(10, ErrorMessage = "PostCode cannot be longer than 256 characters")]
//        [Column(TypeName = "varchar")]
//        public string PostCode { get; set; }

//        [Required]
//        public Enums.User.UserAddressType UserAddressType { get; set; }

//        bool isActive = true, isDeleted = false;

//        [Required]
//        public bool IsDeleted
//        {
//            get { return isDeleted; }
//            set { isDeleted = value; }
//        }
//        /// <summary>
//        /// 
//        /// </summary>

//        [Required]
//        public bool IsActive
//        {
//            get { return isActive; }
//            set { isActive = value; }
//        }
//    }
//}