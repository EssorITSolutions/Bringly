using Bringly.Domain.Common;
using Bringly.Domain.User;
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
        public int UnReadCount { get; set; }
        public int CartCount { get; set; }
        public string TemplateType { get; set; }
        public List<Email> Emails { get; set; }
        public bool Isemailreplyorforward { get; set; }
    }
    public class Email : BaseClasses.DomainBase
    {
        Regex regex = new Regex("\\<[^\\>]*\\>");
        public Guid EmailGuid { get; set; }
        public Guid TemplateGuid { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public List<EmailTo> EmailToList { get; set; }
        [MinLength(1)]
        public string[] EmailToGuid { get; set; }
        public string TemplateType { get; set; }
        [Required(ErrorMessage = "Please enter subject.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please enter message.")]
        
        public string Body { get; set; }
        public string EmailFrom { get; set; }
        public bool Read { get; set; }
        public string RestaurantImage { get; set; }
        public string UserImage { get; set; }
        //public string PlainText { get {
        //        string PlainTextBody = regex.Replace(Body, String.Empty).Replace("&nbsp;", String.Empty).Replace("\r\n", String.Empty);
        //        return PlainTextBody.Trim().Substring(0, PlainTextBody.Trim().Length>100?100: PlainTextBody.Trim().Length);
        //    }
        //}
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
        public Guid EmailGuid { get; set; }
        [MinLength(1)]
        public string[] EmailToGuid { get; set; }
        public string EmailTo { get; set; }
        public Email EmailMessage { get; set; }
        public List<Contact> UserContactList { get; set; }
        public bool Isemailreplyorforward { get; set; }
        public string UserName { get; set; }
        public Guid CreatedGuid { get; set; }
    }
    public class EmailTo
    {
        public Guid UserGuid { get; set; }
        public string Name { get; set; }
    }    
}
