using System;
using System.ComponentModel.DataAnnotations;

namespace Bringly.Domain.Log
{
    public class DataChangeLog : BaseClasses.DomainBase
    {
        public DataChangeLog()
        {
            DataChangeLogId = Guid.NewGuid();
        }
        [Key]
        public Guid DataChangeLogId { get; set; }

        [Required]
        public string OriginalValue { get; set; }

        [Required]
        public string NewValue { get; set; }

        [Required]
        public string RecordId { get; set; }

        [StringLength(256)]
        [Required]
        public string EventType { get; set; }

        [StringLength(256)]
        [Required]
        public string Model { get; set; }

        public Guid? FkCreatedBy { get; set; }


        #region 
        /// <summary>
        /// 
        /// </summary>
        bool isActive = true, isDeleted = false;

        [Required]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        [Required]
        public bool IsActive
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
