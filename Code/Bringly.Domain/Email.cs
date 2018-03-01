using Bringly.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class MyEmail : Paging
    {
        public Guid EmailGuid { get; set; }
        public Guid TemplateGuid { get; set; }
        [Required(ErrorMessage = "Please fill Subject.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please fill email address.")]
        public string EmailTo { get; set; }
        [Required(ErrorMessage = "Please fill email address.")]
        public string EmailFrom { get; set; }
        public string Body { get; set; }
        public int UnReadCount { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string TemplateType { get; set; }
        public List<Email> Emails { get; set; }
    }
    public class Email : BaseClasses.DomainBase
    {
        Regex regex = new Regex("\\<[^\\>]*\\>");
        public Guid EmailGuid { get; set; }
        public Guid TemplateGuid { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailFrom { get; set; }
        public bool Sent { get; set; }
        public bool Read { get; set; }
        public string PlainText { get {

                return regex.Replace(Body, String.Empty).Replace("&nbsp;", String.Empty).Substring(0, 100);
            }
        }
        public string TimeStamp
        {
            get
            {
                return String.Format("{0} days {1} hours {2} minutes", (DateTime.Now - DateCreated).Days, (DateTime.Now - DateCreated).Hours
, (DateTime.Now - DateCreated).Minutes) + " ago";
            }
        }
    }
    public class ComposeEmail
    {
        [Required(ErrorMessage = "Please fill Subject.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please fill email address.")]
        public string EmailTo { get; set; }
        [Required(ErrorMessage = "Please fill message.")]
        public string Body { get; set; }
    }



    //public class Sent : Paging
    //{
    //    public int MessageCount { get; set; }
    //    public List<Email> Emails { get; set; }
    //}
}
