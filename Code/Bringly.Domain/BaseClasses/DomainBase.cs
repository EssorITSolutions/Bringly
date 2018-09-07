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

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [Required]
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dateTime"></param>
        /// <returns></returns>
        public static string Dateformat(DateTime _dateTime)
        {
            return _dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dateTime"></param>
        /// <returns></returns>
        public static string Time24hr(DateTime _dateTime)
        {
            return _dateTime.ToString("H:mm");
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public Guid LoggedInUserId
        {
            get
            {
                return Guid.Empty;
            }
        }
        public bool IsActive { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } 
        public Guid? DeletedBy { get; set; }
    }
}
