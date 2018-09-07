using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bringly.Domain.LookUps
{
    public class LookUpDomainValues : BaseClasses.DomainBase
    {
        /// <summary>
        /// 
        /// </summary>
        public LookUpDomainValues()
        {
            LookUpDomainValueId = Guid.NewGuid();
            DisplayOrder = 0;
            CanEditLookUpDomainValue = CanEditLookUpDomainValueText = true;
            CanDeleteRecord = false;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid LookUpDomainValueId { get; set; }

        [ForeignKey("LookUpDomain")]
        public Guid FkLookUpDomainId { get; set; }

        public virtual LookUpDomains LookUpDomain { get; set; }

        string _lookUpDomainCode = "", _lookUpDomainValue = "";

        [StringLength(500, ErrorMessage = "Code cannot be longer than 500 characters")]
        [Column(TypeName = "varchar")]
        [Required]
        public string LookUpDomainCode
        {
            get
            {
                if (string.IsNullOrEmpty(_lookUpDomainCode) || string.IsNullOrWhiteSpace(_lookUpDomainCode))
                {
                    return LookUpDomainValueText;
                }
                return _lookUpDomainCode;
            }
            set
            {
                _lookUpDomainCode = value;
            }

        }

        [Required(ErrorMessage = "Please enter value")]
        public string LookUpDomainValue
        {
            get
            {
                if (string.IsNullOrEmpty(_lookUpDomainValue) || string.IsNullOrWhiteSpace(_lookUpDomainValue))
                {
                    return LookUpDomainValueText;
                }
                return _lookUpDomainValue;
            }
            set
            {
                _lookUpDomainValue = value;
            }
        }

        [Required(ErrorMessage = "Please enter text")]
        public string LookUpDomainValueText { get; set; }

        [Required(ErrorMessage = "Please enter value")]
        public bool CanEditLookUpDomainValue { get; set; }

        [Required(ErrorMessage = "Please enter text")]
        public bool CanEditLookUpDomainValueText { get; set; }

        [Required]
        public bool CanDeleteRecord { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        [ForeignKey("CreatedByUser")]
        public Guid FkCreatedBy { get; set; }
        //public virtual ApplicationUsers CreatedByUser { get; set; }

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
