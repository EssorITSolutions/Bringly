
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bringly.Domain.BaseClasses
{
    public abstract class DomainBase
    {
        [NotMapped]
        [JsonIgnore]
        public bool ClosePopUp { get; set; }

        DateTime _dateCreated = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string DateCreated_ToLongDateString
        {
            get
            {
                return _dateCreated.ToLongDateString();
            }
        }

        [JsonIgnore]
        [Required]
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        [JsonIgnore]
        public Guid LoggedInUserId
        {
            get
            {
                //if (UserVariables.IsAuthenticated)
                //    return UserVariables.LoggedInUserId;
                //else
                return Guid.Empty;
            }
        }
        bool _isActive = true;
        public bool IsActive { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        bool _isDeleted = false;
        public bool IsDeleted { get; set; } 
        public Guid? DeletedBy { get; set; }
    }
}
