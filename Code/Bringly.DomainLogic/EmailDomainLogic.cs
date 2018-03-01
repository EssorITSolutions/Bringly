using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Bringly.Data;
using Bringly.Domain.User;
using Bringly.Domain.Enums.User;
using Bringly.Domain;
using System.IO;
using System.Web;
using System.Web.Security;
using Bringly.Domain.Common;
using Utilities.EmailSender;
using Utilities.EmailSender.Domain;
using Bringly.Domain.Enums;
using System.Text.RegularExpressions;

namespace Bringly.DomainLogic
{
    public class EmailDomainLogic : BaseClass.DomainLogicBase
    {
        public bool SendEmail(MyEmail MyEmail)
        {
            EmailDomain EmailDomain = new EmailDomain();
            if (MyEmail.EmailTo.Contains(","))
            {
                ListOfMultipleEmailTo ListOfMultipleEmailTo = new ListOfMultipleEmailTo();
                foreach (string str in MyEmail.EmailTo.Split(','))
                {
                    ListOfMultipleEmailTo.Add(str, str);
                }
                EmailDomain.ListOfMultpleEmailTo = ListOfMultipleEmailTo;
            }
            else
            {
                EmailDomain.EmailTo = MyEmail.EmailTo;
            }
            List<tblTemplate> t = !string.IsNullOrEmpty(MyEmail.TemplateType) ? bringlyEntities.tblTemplates.Where(x => x.TemplateType == MyEmail.TemplateType).ToList() : bringlyEntities.tblTemplates.ToList();
            EmailDomain.EmailBody = (t != null && t.Count > 0) ? t.FirstOrDefault().Body : "";
            EmailDomain.EmailFrom = (t != null && t.Count > 0) ? t.FirstOrDefault().EmailFrom : "noreply@essorsolutions.com";
            EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{UserName}", "Sir/ Madam").Replace("{Description}", MyEmail.Body);// message body
            EmailDomain.EmailSubject = !string.IsNullOrEmpty(MyEmail.Subject) ? MyEmail.Subject : (t != null && t.Count > 0) ? t.FirstOrDefault().Subject : "Order Subject" ;
            string emailresponse=EmailSender.sendEmail(EmailDomain);
            if (emailresponse=="")
            {
                tblEmail tblEmail = new tblEmail();
                tblEmail = new tblEmail();
                tblEmail.EmailGuid = Guid.NewGuid();
                tblEmail.EmailFrom = EmailDomain.EmailFrom;
                tblEmail.Subject = EmailDomain.EmailSubject;
                tblEmail.Body = EmailDomain.EmailBody;
                tblEmail.Sent = true;
                tblEmail.TemplateGuid= (t != null && t.Count > 0) ? t.FirstOrDefault().TemplateGuid : Guid.NewGuid();
                tblEmail.DateCreated = DateTime.Now;
                tblEmail.CreatedByGuid = UserVariables.LoggedInUserGuid;
                bringlyEntities.tblEmails.Add(tblEmail);//.SaveChanges();
                //bringlyEntities.SaveChanges();
                tblEmailTo tblEmailTo = new tblEmailTo();
                if (EmailDomain.ListOfMultpleEmailTo != null && EmailDomain.ListOfMultpleEmailTo.Count > 0)
                {
                    foreach (KeyValuePair<string, string> to in EmailDomain.ListOfMultpleEmailTo)
                    {
                        tblEmailTo = new tblEmailTo();
                        tblEmailTo.EmailToGuid = Guid.NewGuid();
                        tblEmailTo.EmailGuid = tblEmail.EmailGuid;
                        tblEmailTo.EmailTo = to.Value;
                        bringlyEntities.tblEmailToes.Add(tblEmailTo);
                        //bringlyEntities.SaveChanges();
                    }
                }
                else {
                    tblEmailTo = new tblEmailTo();
                    tblEmailTo.EmailToGuid = Guid.NewGuid();
                    tblEmailTo.EmailGuid = tblEmail.EmailGuid;
                    tblEmailTo.EmailTo = EmailDomain.EmailTo;
                    bringlyEntities.tblEmailToes.Add(tblEmailTo);
                    //bringlyEntities.SaveChanges();
                }
                bringlyEntities.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }

        public MyEmail GetSentEmail(int LatestPage=0)
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;
            Email.SortBy = SortBy;
            Email.Emails = bringlyEntities.tblEmails.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted==false && x.Sent==true).
                Select(em=>new Email { EmailGuid=em.EmailGuid,TemplateGuid=em.TemplateGuid,Subject=em.Subject,Body=em.Body,EmailFrom=em.EmailFrom, Sent=em.Sent,DateCreated=em.DateCreated,UserName=em.tblUser.FullName })
                .OrderByDescending(x => x.DateCreated).ToList();
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read == false)
                .ToList().Count;
            Email.TotalRecords = Email.Emails.Count;
            int Skip = 0;
            int Take = 5;
            if (Email.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((Email.CurrentPage * Email.PageSize) - Email.PageSize);
            if (Email.TotalRecords == 0 && Skip>0) {
                Skip = Skip - 1;                
            }
            Email.Emails = Email.Emails.Skip(Skip).Take(Take).ToList();
            return Email;
        }

        public bool DeleteEmail(Guid[] EmailGuidArray)
        {                
                List<tblEmail> EmailGuidToBeDelete= bringlyEntities.tblEmails.Where(x => EmailGuidArray.Contains(x.EmailGuid)).ToList();
                foreach (tblEmail email in EmailGuidToBeDelete)
                {                   
                    email.IsDeleted = true;                    
                }
                bringlyEntities.SaveChanges();
                return true;                   
        }        

        public MyEmail GetInboxEmail(Guid EmailGuid,int LatestPage=0)
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            Email.SortBy = SortBy;
            Regex regex = new Regex("\\<[^\\>]*\\>"); 
            

            Email.Emails = bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false).
                Select(em => new Email { EmailGuid = em.tblEmail.EmailGuid, TemplateGuid = em.tblEmail.TemplateGuid, Subject = em.tblEmail.Subject, Body = em.tblEmail.Body, EmailFrom = em.tblEmail.EmailFrom
                , Sent = em.tblEmail.Sent, DateCreated = em.tblEmail.DateCreated, UserName = em.tblEmail.tblUser.FullName,Read=em.Read })
                .OrderByDescending(x => x.DateCreated).OrderByDescending(x => x.Read==false).ToList();
            if (EmailGuid != Guid.Empty) {
                Email.Emails = Email.Emails.Where(x => x.EmailGuid == EmailGuid).ToList();
                Guid[] guidArrayList=new Guid[1];
                guidArrayList[0]=EmailGuid;
               // MarkAsRead(guidArrayList);
            }
            Email.UnReadCount= bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read==false)
                .ToList().Count;
            
            Email.TotalRecords = Email.Emails.Count;
            int Skip = 0;
            int Take = 5;
            if (Email.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((Email.CurrentPage * Email.PageSize) - Email.PageSize);
            if (Email.TotalRecords == 0 && Skip > 0)
            {
                Skip = Skip - 1;
            }
            Email.Emails = Email.Emails.Skip(Skip).Take(Take).ToList();
            
            
            return Email;
        }

        public int MarkAsRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.EmailGuid)).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = true;
            }
            bringlyEntities.SaveChanges();
            int count= bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read == false)
                .ToList().Count;
            return count ;
        }

        public int MarkAsUnRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.EmailGuid)).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = false;
            }
            bringlyEntities.SaveChanges();
            int count = bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read == false)
                .ToList().Count;
            return count;
        }

        public int GetUnReadEmailCount()
        {
            return bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read == false)
                .ToList().Count;
        }

        public MyEmail GetNotificationEmail(Guid EmailGuid)
        {
            MyEmail Email = new MyEmail();
            Email.Emails = bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false).
                Select(em => new Email
                {
                    EmailGuid = em.tblEmail.EmailGuid,
                    TemplateGuid = em.tblEmail.TemplateGuid,
                    Subject = em.tblEmail.Subject,
                    Body = em.tblEmail.Body,
                    EmailFrom = em.tblEmail.EmailFrom
                ,
                    Sent = em.tblEmail.Sent,
                    DateCreated = em.tblEmail.DateCreated,
                    UserName = em.tblEmail.tblUser.FullName,
                    Read = em.Read
                })
                .OrderByDescending(x => x.DateCreated).OrderByDescending(x => x.Read == false).Take(2).ToList();
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.tblEmail.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblEmail.IsDeleted == false && x.tblEmail.Sent == false && x.Read == false)
              .ToList().Count;
            return Email;
        }
    }
}
