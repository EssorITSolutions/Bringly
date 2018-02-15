//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Bringly.Domain.User
//{
//    public class ApplicationUsers : BaseClasses.DomainBase
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public ApplicationUsers()
//        {
//            ApplicationUserGuid = Guid.NewGuid();
//            UserRegistrationType = Enums.User.UserRegistrationType.Direct;
//        }

//        [Key]
//        public Guid ApplicationUserGuid{ get; set; }

//        [Required(ErrorMessage = "Please enter email address")]
//        [Index("IX_Unique_ApplicationUsers_EmailAddress", IsUnique = true)]
//        [StringLength(256, ErrorMessage = "Email Address cannot be longer than 256 characters")]
//        [Column(TypeName = "varchar")]
//        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter valid email address")]
//        public string EmailAddress { get; set; }

//        [Required(ErrorMessage = "Please enter password")]
//        [StringLength(256, ErrorMessage = "Password cannot be longer than 256 characters")]
//        [Column(TypeName = "varchar")]
//        public string UserPassword { get; set; }

//        //[Required]
//        //[ForeignKey("UserRoleId")]
//        //public short FkUserRoleId { get; set; }

//        [StringLength(500)]
//        [Column(TypeName = "varchar")]
//        public string SocialMediaUniqueId { get; set; }

//        string _fullName = "";

//        [StringLength(256, ErrorMessage = "First name cannot be longer than 256 characters")]
//        [Required(ErrorMessage = "Please enter your name")]
//        [Column(TypeName = "varchar")]
//        public string FullName
//        {
//            get
//            {
//                return _fullName.ToFirstLetterCapitalize();
//            }
//            set
//            {
//                _fullName = value;
//            }
//        }

//        [Column(TypeName = "varchar")]
//        public string ProfilePic
//        { get; set; }


//        //[StringLength(256, ErrorMessage = "Last name cannot be longer than 256 characters")]
//        //[Required(ErrorMessage = "Please enter lastname")]
//        //[Column(TypeName = "varchar")]
//        //public string LastName
//        //{
//        //    get
//        //    {
//        //        return _lastName.ToFirstLetterCapitalize();
//        //    }
//        //    set
//        //    {
//        //        _lastName = value;
//        //    }
//        //}

//        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
//        //public string FullName
//        //{
//        //    get
//        //    {
//        //        return FirstName + " " + LastName;
//        //    }
//        //    private set { /* needed for EF */ }
//        //}

//        //[StringLength(256, ErrorMessage = "Email address cannot be longer than 256 characters")]
//        //[Required(ErrorMessage = "Please enter emailaddress")]
//        //[Column(TypeName = "varchar")]
//        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter valid email address")]
//        //public string EmailAddress { get; set; }

//        [Column(TypeName = "varchar")]
//        public string UserSocialMediaData { get; set; }

//        [Required]
//        public Enums.User.UserRegistrationType UserRegistrationType { get; set; }

//        public virtual ICollection<ApplicationUserAssignedRoles> LstApplicationUserAssignedRoles { get; set; }


//        [ForeignKey("CreatedByUser")]
//        public Guid? FkCreatedBy { get; set; }
//        public virtual ApplicationUsers CreatedByUser { get; set; }        

//        #region 
//        /// <summary>
//        /// 
//        /// </summary>
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

//        /// <summary>
//        /// 
//        /// </summary>

//        public string IsActive_YesNo
//        {
//            get
//            {
//                return isActive ? "Yes" : "No";
//            }
//        }

//        public DateTime? DeletedDateTime
//        {
//            get; set;
//        }


//        public Guid? FkDeletedBy { get; set; }
//        #endregion
//    }
//}
