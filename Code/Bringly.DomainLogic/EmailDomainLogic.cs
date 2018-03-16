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
        public bool SendEmail(ComposeEmail ComposeEmail)
        {
            EmailDomain EmailDomain = new EmailDomain();
            tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
            tblTemplate template = new tblTemplate();
            Restaurant tblRestaurant = new Restaurant();
            string image = "<img src = " + CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Default, "") + ">";
            string UserImageName = userfrom.ImageName;
            int count = 0;
            if (ComposeEmail.EmailToGuid != null && ComposeEmail.EmailToGuid.Count() > 0)
            {
                foreach (string usertoguid in ComposeEmail.EmailToGuid)
                {
                    tblUser userto = bringlyEntities.tblUsers.Where(x => x.UserGuid == new Guid(usertoguid)).ToList().FirstOrDefault();
                    EmailDomain.EmailTo = userto.EmailAddress;
                    template = !string.IsNullOrEmpty(ComposeEmail.EmailMessage.TemplateType) ? bringlyEntities.tblTemplates.Where(x => x.TemplateType == ComposeEmail.EmailMessage.TemplateType).ToList().FirstOrDefault() : new tblTemplate();
                    if (template != null && template.TemplateGuid != null && template.TemplateGuid != Guid.Empty)
                    {
                        EmailDomain.EmailFrom = userfrom.EmailAddress;
                        EmailDomain.EmailSubject = ComposeEmail.EmailMessage.Subject;
                        if (!ComposeEmail.Isemailreplyorforward)
                        {
                            tblRestaurant = bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid).
                                            Select(s => new Restaurant { RestaurantImage = s.RestaurantImage, RestaurantName = s.RestaurantName }).ToList().FirstOrDefault();
                            EmailDomain.EmailBody = template.Body;
                            EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{ToName}", userto.FullName).Replace("{Description}", ComposeEmail.EmailMessage.Body)
                                .Replace("{FromName}", userfrom.FullName);                            
                        }
                        else
                        {
                            EmailDomain.EmailBody = ComposeEmail.EmailMessage.Body;
                        }

                        string emailresponse = "";//EmailSender.sendEmail(EmailDomain);

                        tblEmail tblEmail = new tblEmail();
                        tblEmail.EmailGuid = Guid.NewGuid();
                        tblEmail.EmailFrom = EmailDomain.EmailFrom;
                        tblEmail.Subject = EmailDomain.EmailSubject;
                        tblEmail.Body = EmailDomain.EmailBody;
                        tblEmail.Sent = (emailresponse == "") ? true : false;
                        tblEmail.TemplateGuid = template.TemplateGuid;
                        tblEmail.DateCreated = DateTime.Now;
                        tblEmail.CreatedByGuid = UserVariables.LoggedInUserGuid;
                        bringlyEntities.tblEmails.Add(tblEmail);
                        if (tblEmail.Sent)
                        {
                            tblEmailTo tblEmailTo = new tblEmailTo();

                            tblEmailTo.EmailToGuid = Guid.NewGuid();
                            tblEmailTo.EmailGuid = tblEmail.EmailGuid;
                            tblEmailTo.EmailTo = EmailDomain.EmailTo;
                            tblEmailTo.UserGuid = new Guid(usertoguid);// new Guid(usertoguid);
                            bringlyEntities.tblEmailToes.Add(tblEmailTo);
                        }
                        bringlyEntities.SaveChanges();
                        count = count + ((emailresponse == "") ? 0 : 1);
                    }
                }
            }
            else { count++; }

            if (count > 0)
            {
                return false;
            }
            else { return true; }            
        }

        public bool SendReviewEmail(ComposeEmail ComposeEmail)
        {
            EmailDomain EmailDomain = new EmailDomain();
            tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
            tblTemplate template = new tblTemplate();
            Restaurant tblRestaurant = new Restaurant();
            string image = "<img src = " + CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Default, "") + ">";
            string UserImageName = userfrom.ImageName;
            int count = 0;
            if (ComposeEmail.EmailToGuid != null && ComposeEmail.EmailToGuid.Count() > 0)
            {
                foreach (string usertoguid in ComposeEmail.EmailToGuid)
                {
                    tblUser userto = bringlyEntities.tblUsers.Where(x => x.UserGuid == new Guid(usertoguid)).ToList().FirstOrDefault();
                    EmailDomain.EmailTo = userto.EmailAddress;
                    template = !string.IsNullOrEmpty(ComposeEmail.EmailMessage.TemplateType) ? bringlyEntities.tblTemplates.Where(x => x.TemplateType == ComposeEmail.EmailMessage.TemplateType).ToList().FirstOrDefault() : new tblTemplate();
                    if (template != null && template.TemplateGuid != null && template.TemplateGuid != Guid.Empty)
                    {
                        EmailDomain.EmailFrom = ComposeEmail.EmailMessage.EmailFrom;
                        EmailDomain.EmailSubject = ComposeEmail.EmailMessage.Subject;
                        if (!ComposeEmail.Isemailreplyorforward)
                        {
                            tblRestaurant = bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid).
                                            Select(s => new Restaurant { RestaurantImage = s.RestaurantImage, RestaurantName = s.RestaurantName }).ToList().FirstOrDefault();
                            EmailDomain.EmailBody = template.Body;
                            EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{ToName}", userto.FullName).Replace("{Description}", ComposeEmail.EmailMessage.Body)
                                ;

                            if (ComposeEmail.EmailMessage.TemplateType == (Enum.GetName(typeof(TemplateType), TemplateType.Review)))
                            { EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{RedirecttoReviewList}", "" + CommonDomainLogic.GetCurrentDomain + "\\User\\MerchantReview").Replace("{FromName}", userfrom.FullName); }
                            else {
                                EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{FromName}", bringlyEntities.tblUsers.Where(x=>x.EmailAddress==template.EmailFrom).ToList().FirstOrDefault().FullName);
                            }
                        }
                        else { EmailDomain.EmailBody = ComposeEmail.EmailMessage.Body; }
                        string emailresponse = "";//EmailSender.sendEmail(EmailDomain);
                        tblEmail tblEmail = new tblEmail();
                        tblEmail.EmailGuid = Guid.NewGuid();
                        tblEmail.EmailFrom = EmailDomain.EmailFrom;
                        tblEmail.Subject = EmailDomain.EmailSubject;
                        tblEmail.Body = EmailDomain.EmailBody;
                        tblEmail.Sent = (emailresponse == "") ? true : false;
                        tblEmail.TemplateGuid = template.TemplateGuid;
                        tblEmail.DateCreated = DateTime.Now;
                        tblEmail.CreatedByGuid = ComposeEmail.CreatedGuid;
                        bringlyEntities.tblEmails.Add(tblEmail);
                        if (tblEmail.Sent)
                        {
                            tblEmailTo tblEmailTo = new tblEmailTo();

                            tblEmailTo.EmailToGuid = Guid.NewGuid();
                            tblEmailTo.EmailGuid = tblEmail.EmailGuid;
                            tblEmailTo.EmailTo = EmailDomain.EmailTo;
                            tblEmailTo.UserGuid = userto.UserGuid;
                            bringlyEntities.tblEmailToes.Add(tblEmailTo);
                        }
                        bringlyEntities.SaveChanges();
                        count = count + ((emailresponse == "") ? 0 : 1);
                    }
                }
            }
            else { count++; }

            if (count > 0)
            {
                return false;
            }
            else { return true; }
        }

        public MyEmail GetSentEmail(int LatestPage=0)
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;
            Email.SortBy = SortBy;


            string restaurantimagepath = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == x.tblUser.UserGuid).FirstOrDefault().RestaurantImage);
            Email.Emails = bringlyEntities.tblEmails.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted==false && x.Sent==true).
                Select(em=>new Email { EmailGuid=em.EmailGuid,TemplateGuid=em.TemplateGuid,Subject=em.Subject,Body=em.Body,EmailFrom=em.EmailFrom,DateCreated=em.DateCreated
                ,FromName =em.tblUser.FullName,ToName= em.tblEmailToes.Where(x => x.UserGuid == x.tblUser.UserGuid).ToList().FirstOrDefault().tblUser.FullName
                ,EmailToList=em.tblEmailToes.Where(x => x.UserGuid == x.tblUser.UserGuid).ToList().Select(t=>new EmailTo {UserGuid=t.UserGuid, Name = t.tblUser.FullName }).ToList()
                ,UserImage = em.tblUser.ImageName
                }).OrderByDescending(x => x.DateCreated).ToList();
            Email.Emails.ForEach(z => z.RestaurantImage = restaurantimagepath);
            
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            Email.TotalRecords = Email.Emails.Count;
            int Skip = 0;
            int Take = PageSize;
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
                List<tblEmailTo> EmailGuidToBeDelete= bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.EmailGuid)).ToList();
                foreach (tblEmailTo email in EmailGuidToBeDelete)
                {                   
                    email.IsDeleted = true;                    
                }
                bringlyEntities.SaveChanges();
                return true;                   
        }        

        public MyEmail GetInboxEmail(int LatestPage=0)
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            Email.SortBy = SortBy;
            string restaurantimagepath = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == x.tblUser.UserGuid).FirstOrDefault().RestaurantImage) ;
            Email.Emails = bringlyEntities.tblEmailToes.Where(x => x.EmailGuid==x.tblEmail.EmailGuid && x.IsDeleted == false && x.UserGuid== UserVariables.LoggedInUserGuid).
                Select(em => new Email { EmailGuid = em.tblEmail.EmailGuid, TemplateGuid = em.tblEmail.TemplateGuid, Subject = em.tblEmail.Subject, Body = em.tblEmail.Body, EmailFrom = em.tblEmail.EmailFrom
                , DateCreated = em.tblEmail.DateCreated
                , FromName = bringlyEntities.tblUsers.Where(zx=>zx.UserGuid==em.tblEmail.CreatedByGuid).ToList().FirstOrDefault().FullName
                ,Read=em.Read,ToName = em.tblUser.FullName
                 ,
                    UserImage = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.CreatedByGuid).ToList().FirstOrDefault().ImageName
                }).OrderByDescending(x => x.DateCreated).ToList().OrderByDescending(x => x.Read == false).ToList();
            Email.Emails.ForEach(z => z.RestaurantImage = restaurantimagepath);
            Email.UnReadCount= bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            
            Email.TotalRecords = Email.Emails.Count;
            int Skip = 0;
            int Take = PageSize;
            if (Email.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((Email.CurrentPage * Email.PageSize) - Email.PageSize);

            Email.Emails = Email.Emails.Skip(Skip).Take(Take).ToList(); 
            return Email;
        }

        public int MarkAsRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.EmailGuid) && x.UserGuid==UserVariables.LoggedInUserGuid).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = true;
            }
            bringlyEntities.SaveChanges();
            int count= bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            return count ;
        }

        public int MarkAsUnRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.EmailGuid) && x.UserGuid == UserVariables.LoggedInUserGuid).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = false;
            }
            bringlyEntities.SaveChanges();
            int count = bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            return count;
        }

        public int GetUnReadEmailCount()
        {
            return bringlyEntities.tblEmailToes.Where(x => x.UserGuid== UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid==x.tblEmail.EmailGuid)
                .ToList().Count;
        }

        public MyEmail GetNotificationEmail(Guid EmailGuid)
        {
            MyEmail Email = new MyEmail();
            Email.Emails = bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid).
                Select(em => new Email
                {
                    EmailGuid = em.tblEmail.EmailGuid,
                    TemplateGuid = em.tblEmail.TemplateGuid,
                    Subject = em.tblEmail.Subject,                    
                    DateCreated = em.tblEmail.DateCreated,
                    FromName = em.tblEmail.tblUser.FullName,
                    Read = em.Read
                })
                .OrderByDescending(x => x.DateCreated).OrderByDescending(x => x.Read == false).Take(2).ToList();
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            string orderStatus = Enum.GetName(typeof(OrderStatus), OrderStatus.Incomplete);
            Email.CartCount = bringlyEntities.tblOrderItems.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.OrderGuid==x.tblOrder.OrderGuid && x.tblOrder.OrderStatus == orderStatus)
              .ToList().Sum(x => x.Quantity);
            return Email;
        }

        public ComposeEmail GetEmailByEmailGuid(Guid EmailGuid)
        {
            ComposeEmail myemail = new ComposeEmail();
            List<tblEmailTo> emailto = bringlyEntities.tblEmailToes.Where(x => x.EmailGuid == EmailGuid).ToList();//.Select(up => new tblEmailTo { EmailTo = up.EmailTo});
            myemail.EmailMessage = bringlyEntities.tblEmails.Where(x => x.EmailGuid == EmailGuid).Select(em=> new Email { EmailGuid=em.EmailGuid ,TemplateGuid=em.TemplateGuid ,Subject= em.Subject,Body=em.Body
                ,EmailFrom=em.EmailFrom,DateCreated=em.DateCreated,CreatedByGuid=em.CreatedByGuid,ToName = bringlyEntities.tblUsers.Where(x=>x.UserGuid== em.CreatedByGuid).ToList().FirstOrDefault().FullName})
                .ToList().FirstOrDefault();                        
          //  myemail.EmailMessage.EmailToGuid = bringlyEntities.tblEmails.Where(x => x.EmailGuid == EmailGuid).Select(x => x.EmailFrom).ToList().FirstOrDefault();
            myemail.EmailMessage.EmailFrom= string.Join(",", emailto.Select(x => x.EmailTo));
            return myemail;
        }
      
    }
}
