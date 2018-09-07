using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bringly.Domain.LookUps
{
    public class LookUpDomains : BaseClasses.DomainBase
    {
        /// <summary>
        /// 
        /// </summary>
        public LookUpDomains()
        {
            LookUpDomainId = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid LookUpDomainId { get; set; }

        [Required(ErrorMessage = "Please enter code")]
        [Index("IX_Unique_LookUpDomain_LookUpDomainCode", IsUnique = true)]
        public Enums.LookUps.LookUpDomainCode LookUpDomainCode { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        [Column(TypeName = "varchar")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string LookUpDomainDescription { get; set; }

        public virtual ICollection<LookUpDomainValues> LstLookUpDomainValue { get; set; }

        [ForeignKey("CreatedByUser")]
        public Guid FkCreatedBy { get; set; }

        #region 
        /// <summary>
        /// 
        /// </summary>
        bool isActive = true, isDeleted = false;

        [Required]
        public new bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        [Required]
        public new bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// 
        /// </summary>

        public string IsActive_YesNo
        {
            get
            {
                return isActive ? "Yes" : "No";
            }
        }

        public DateTime? DeletedDateDateTime
        {
            get; set;
        }

        public Guid? FkDeletedBy { get; set; }
        #endregion
    }
}
