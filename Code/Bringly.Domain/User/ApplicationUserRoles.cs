//using Newtonsoft.Json;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Bringly.Domain.User
//{
//    public class ApplicationUserRoles : BaseClasses.DomainBase
//    {
//        [Key]
//        public short UserRoleId { get; set; }

//        [Required]
//        [Index("IX_Unique_ApplicationUserRoles_UserRoleName", IsUnique = true)]
//        [StringLength(256)]
//        [Column(TypeName = "varchar")]
//        public string UserRoleName { get; set; }

//        [Required]
//        [Index("IX_Unique_ApplicationUserRoles_UserRoleCode", IsUnique = true)]
//        public Enums.User.UserRoles UserRoleCode { get; set; }

//        [Required]
//        [StringLength(2000)]
//        [Column(TypeName = "varchar")]
//        public string UserRoleDescription { get; set; }

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

//        public DateTime? DeletedDateDateTime
//        {
//            get; set;
//        }


//        public Guid? FkDeletedBy { get; set; }
//        #endregion
//    }
//}
