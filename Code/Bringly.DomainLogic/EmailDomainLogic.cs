using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Business;
using Bringly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Utilities;
using Utilities.EmailSender;
using Utilities.EmailSender.Domain;
using Utilities.Helper;

namespace Bringly.DomainLogic
{
    public class EmailDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComposeEmail"></param>
        /// <returns></returns>
        public bool SendEmail(ComposeEmail ComposeEmail)
        {
            EmailDomain EmailDomain = new EmailDomain();
            tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
            tblEmailTemplate template = new tblEmailTemplate();
            BusinessObject tblBusiness = new BusinessObject();
            string image = "<img src = " + CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Default, "") + ">";
            string UserImageName = userfrom.ImageName;
            int count = 0;
            if (ComposeEmail.EmailToGuid != null && ComposeEmail.EmailToGuid.Count() > 0)
            {
                foreach (string usertoguid in ComposeEmail.EmailToGuid)
                {
                    tblUser userto = bringlyEntities.tblUsers.Where(x => x.UserGuid == new Guid(usertoguid)).ToList().FirstOrDefault();
                    EmailDomain.EmailTo = userto.EmailAddress;
                    template = !string.IsNullOrEmpty(ComposeEmail.EmailMessage.TemplateType) ? bringlyEntities.tblEmailTemplates
                        .Where(x => x.TemplateType == ComposeEmail.EmailMessage.TemplateType).ToList().FirstOrDefault() : new tblEmailTemplate();
                    if (template != null && template.TemplateGuid != null && template.TemplateGuid != Guid.Empty)
                    {
                        EmailDomain.EmailFrom = userfrom.EmailAddress;
                        EmailDomain.EmailSubject = ComposeEmail.EmailMessage.Subject;
                        if (!ComposeEmail.Isemailreplyorforward)
                        {
                            tblBusiness = bringlyEntities.tblBusinesses.Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).
                                            Select(s => new BusinessObject { BusinessImage = s.BusinessImage, BusinessName = s.BusinessName }).ToList().FirstOrDefault();
                            EmailDomain.EmailBody = template.Body;
                            EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{ToName}", userto.FullName).Replace("{Description}", ComposeEmail.EmailMessage.Body)
                                .Replace("{FromName}", userfrom.FullName);
                        }
                        else
                        {
                            EmailDomain.EmailBody = ComposeEmail.EmailMessage.Body;
                        }

                        string emailSendResult = EmailSender.sendEmail(EmailDomain);
                        if (!string.IsNullOrEmpty(emailSendResult))
                        {
                            ErrorLog.LogError(emailSendResult, "send email Error");
                        }

                        tblEmail tblEmail = new tblEmail();
                        tblEmail.EmailGuid = Guid.NewGuid();
                        tblEmail.EmailFrom = EmailDomain.EmailFrom;
                        tblEmail.Subject = EmailDomain.EmailSubject;
                        tblEmail.Body = EmailDomain.EmailBody;
                        tblEmail.Sent = (emailSendResult == "") ? true : false;
                        tblEmail.FK_TemplateGuid = template.TemplateGuid;
                        tblEmail.DateCreated = DateTime.Now;
                        tblEmail.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
                        bringlyEntities.tblEmails.Add(tblEmail);
                        if (tblEmail.Sent)
                        {
                            tblEmailTo tblEmailTo = new tblEmailTo();

                            tblEmailTo.EmailToGuid = Guid.NewGuid();
                            tblEmailTo.FK_EmailGuid = tblEmail.EmailGuid;
                            tblEmailTo.EmailTo = EmailDomain.EmailTo;
                            tblEmailTo.FK_UserGuid = new Guid(usertoguid);// new Guid(usertoguid);
                            bringlyEntities.tblEmailToes.Add(tblEmailTo);
                        }
                        bringlyEntities.SaveChanges();
                        count = count + ((emailSendResult == "") ? 0 : 1);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComposeEmail"></param>
        /// <returns></returns>
        public bool SendReviewEmail(ComposeEmail ComposeEmail)
        {
            EmailDomain EmailDomain = new EmailDomain();
            tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
            tblEmailTemplate template = new tblEmailTemplate();
            BusinessObject tblBusiness = new BusinessObject();
            string image = "<img src = " + CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Default, "") + ">";
            string UserImageName = userfrom.ImageName;
            int count = 0;
            if (ComposeEmail.EmailToGuid != null && ComposeEmail.EmailToGuid.Count() > 0)
            {
                foreach (string usertoguid in ComposeEmail.EmailToGuid)
                {
                    tblUser userto = bringlyEntities.tblUsers.Where(x => x.UserGuid == new Guid(usertoguid)).FirstOrDefault();
                    if (userto != null)
                    {
                        EmailDomain.EmailTo = userto.EmailAddress;
                        template = !string.IsNullOrEmpty(ComposeEmail.EmailMessage.TemplateType) ? bringlyEntities.tblEmailTemplates
                            .Where(x => x.TemplateType == ComposeEmail.EmailMessage.TemplateType).ToList().FirstOrDefault() : new tblEmailTemplate();
                        if (template != null && template.TemplateGuid != null && template.TemplateGuid != Guid.Empty)
                        {
                            EmailDomain.EmailFrom = ComposeEmail.EmailMessage.EmailFrom;
                            EmailDomain.EmailSubject = ComposeEmail.EmailMessage.Subject;
                            if (!ComposeEmail.Isemailreplyorforward)
                            {
                                tblBusiness = bringlyEntities.tblBusinesses.Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).
                                                Select(s => new BusinessObject { BusinessImage = s.BusinessImage, BusinessName = s.BusinessName }).FirstOrDefault();
                                EmailDomain.EmailBody = template.Body;
                                EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{ToName}", userto.FullName).Replace("{Description}", ComposeEmail.EmailMessage.Body)
                                    ;

                                if (ComposeEmail.EmailMessage.TemplateType == (Enum.GetName(typeof(TemplateType), TemplateType.Review)))
                                { EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{RedirecttoReviewList}", "" + CommonDomainLogic.GetCurrentDomain + "\\User\\MerchantReview").Replace("{FromName}", userfrom.FullName); }
                                else
                                {
                                    EmailDomain.EmailBody = EmailDomain.EmailBody.Replace("{FromName}", bringlyEntities.tblUsers.Where(x => x.UserGuid == ComposeEmail.CreatedGuid).FirstOrDefault().FullName);
                                }
                            }
                            else { EmailDomain.EmailBody = ComposeEmail.EmailMessage.Body; }

                            string emailresponse = EmailSender.sendEmail(EmailDomain);
                            if (!string.IsNullOrEmpty(emailresponse))
                            {
                                ErrorLog.LogError(emailresponse, "send email Error");
                            }

                            tblEmail tblEmail = new tblEmail();
                            tblEmail.EmailGuid = Guid.NewGuid();
                            tblEmail.EmailFrom = EmailDomain.EmailFrom;
                            tblEmail.Subject = EmailDomain.EmailSubject;
                            tblEmail.Body = EmailDomain.EmailBody;
                            tblEmail.Sent = (emailresponse == "") ? true : false;
                            tblEmail.FK_TemplateGuid = template.TemplateGuid;
                            tblEmail.DateCreated = DateTime.Now;
                            tblEmail.FK_CreatedByGuid = ComposeEmail.CreatedGuid;
                            bringlyEntities.tblEmails.Add(tblEmail);
                            if (tblEmail.Sent)
                            {
                                tblEmailTo tblEmailTo = new tblEmailTo();

                                tblEmailTo.EmailToGuid = Guid.NewGuid();
                                tblEmailTo.FK_EmailGuid = tblEmail.EmailGuid;
                                tblEmailTo.EmailTo = EmailDomain.EmailTo;
                                tblEmailTo.FK_UserGuid = userto.UserGuid;
                                bringlyEntities.tblEmailToes.Add(tblEmailTo);
                            }
                            bringlyEntities.SaveChanges();
                            count = count + ((emailresponse == "") ? 0 : 1);
                        }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LatestPage"></param>
        /// <returns></returns>
        public MyEmail GetSentEmail(int LatestPage = 0)
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;
            Email.SortBy = SortBy;
            string restaurantimagepath = string.Empty;
            var restaurant = bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == x.tblUser.UserGuid).FirstOrDefault();
            if (restaurant != null)
                restaurantimagepath = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, restaurant.RestaurantImage);

            Email.Emails = bringlyEntities.tblEmails.Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.Sent == true).
                Select(em => new Email
                {
                    EmailGuid = em.EmailGuid,
                    TemplateGuid = em.FK_TemplateGuid,
                    Subject = em.Subject,
                    Body = em.Body,
                    EmailFrom = em.EmailFrom,
                    DateCreated = em.DateCreated
                ,
                    FromName = em.tblUser.FullName,
                    ToName = em.tblEmailToes.Where(x => x.FK_UserGuid == x.tblUser.UserGuid).ToList().FirstOrDefault().tblUser.FullName
                ,
                    EmailToList = em.tblEmailToes.Where(x => x.FK_UserGuid == x.tblUser.UserGuid).ToList().Select(t => new EmailTo { UserGuid = t.FK_UserGuid, Name = t.tblUser.FullName }).ToList()
                ,
                    UserImage = em.tblUser.ImageName
                }).OrderByDescending(x => x.DateCreated).ToList();
            Email.Emails.ForEach(z => z.RestaurantImage = restaurantimagepath);

            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            Email.TotalRecords = Email.Emails.Count;
            int Skip = 0;
            int Take = PageSize;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuidArray"></param>
        /// <returns></returns>
        public bool DeleteEmail(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidToBeDelete = bringlyEntities.tblEmailToes.Where(x => EmailGuidArray.Contains(x.FK_EmailGuid)).ToList();
            foreach (tblEmailTo email in EmailGuidToBeDelete)
            {
                email.IsDeleted = true;
            }
            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LatestPage"></param>
        /// <param name="searchQuery"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public MyEmail GetInboxEmail(int LatestPage = 0, string searchQuery = "", string sortBy = "lto")
        {
            MyEmail Email = new MyEmail();
            Email.PageSize = PageSize;
            Email.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage
            Email.SortBy = SortBy;
            string restaurantimagepath = string.Empty;
            var restaurant = bringlyEntities.tblRestaurants.Where(x => x.CreatedByGuid == x.tblUser.UserGuid).FirstOrDefault();
            var temp = bringlyEntities.tblBusinesses.Where(x => x.FK_CreatedByGuid == x.tblUser.UserGuid).FirstOrDefault();
            if (restaurant != null)
                restaurantimagepath = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, restaurant.RestaurantImage);
            else
            {
                restaurantimagepath = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(null, null);
            }
            var data = bringlyEntities.tblEmailToes
                .Where(x => x.FK_EmailGuid == x.tblEmail.EmailGuid && x.IsDeleted == false && x.FK_UserGuid == UserVariables.LoggedInUserGuid);

            if (!string.IsNullOrEmpty(searchQuery))
                data = data.Where(x => x.tblEmail.Subject.Contains(searchQuery));

            if (sortBy != null && sortBy.Equals("otl"))
            {
                Email.Emails = data.Select(em => new Email
                {
                    EmailGuid = em.tblEmail.EmailGuid,
                    TemplateGuid = em.tblEmail.FK_TemplateGuid,
                    Subject = em.tblEmail.Subject,
                    Body = em.tblEmail.Body,
                    EmailFrom = em.tblEmail.EmailFrom,
                    DateCreated = em.tblEmail.DateCreated,
                    FromName = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().FullName,
                    Read = em.Read,
                    ToName = em.tblUser.FullName,
                    UserImage = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().ImageName
                })
                .OrderBy(x => x.DateCreated)
                .ToList()
                .OrderBy(x => x.Read == false)
                .ToList();
            }

            else
            {
                Email.Emails = data
                    .Select(em => new Email
                    {
                        EmailGuid = em.tblEmail.EmailGuid,
                        TemplateGuid = em.tblEmail.FK_TemplateGuid,
                        Subject = em.tblEmail.Subject,
                        Body = em.tblEmail.Body,
                        EmailFrom = em.tblEmail.EmailFrom,
                        DateCreated = em.tblEmail.DateCreated,
                        FromName = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().FullName,
                        Read = em.Read,
                        ToName = em.tblUser.FullName,
                        UserImage = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().ImageName
                    })
                    .OrderByDescending(x => x.DateCreated)
                    .ToList()
                    .OrderByDescending(x => x.Read == false)
                    .ToList();
            }

            Email.Emails.ForEach(z => z.RestaurantImage = restaurantimagepath);
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailGuid"></param>
        /// <returns></returns>
        public Email GetEmailDetailsByGuid(Guid emailGuid)
        {
            var data = bringlyEntities.tblEmailToes
              .Where(x => x.FK_EmailGuid == x.tblEmail.EmailGuid && x.IsDeleted == false
              && x.FK_UserGuid == UserVariables.LoggedInUserGuid && x.FK_EmailGuid == emailGuid).Select(em => new Email
              {
                  EmailGuid = em.tblEmail.EmailGuid,
                  TemplateGuid = em.tblEmail.FK_TemplateGuid,
                  Subject = em.tblEmail.Subject,
                  Body = em.tblEmail.Body,
                  EmailFrom = em.tblEmail.EmailFrom,
                  DateCreated = em.tblEmail.DateCreated,
                  FromName = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().FullName,
                  Read = em.Read,
                  ToName = em.tblUser.FullName,
                  UserImage = bringlyEntities.tblUsers.Where(zx => zx.UserGuid == em.tblEmail.FK_CreatedByGuid).FirstOrDefault().ImageName
              }).FirstOrDefault();

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuidArray"></param>
        /// <returns></returns>
        public int MarkAsRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes
                .Where(x => EmailGuidArray.Contains(x.FK_EmailGuid) && x.FK_UserGuid == UserVariables.LoggedInUserGuid).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = true;
            }
            bringlyEntities.SaveChanges();
            int count = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuidArray"></param>
        /// <returns></returns>
        public int MarkAsUnRead(Guid[] EmailGuidArray)
        {
            List<tblEmailTo> EmailGuidMarkAsRead = bringlyEntities.tblEmailToes
                .Where(x => EmailGuidArray.Contains(x.FK_EmailGuid) && x.FK_UserGuid == UserVariables.LoggedInUserGuid).ToList();
            foreach (tblEmailTo email in EmailGuidMarkAsRead)
            {
                email.Read = false;
            }
            bringlyEntities.SaveChanges();
            int count = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid).Count();
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetUnReadEmailCount()
        {
            return bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public MyEmail GetNotificationEmail(Guid EmailGuid)
        {
            MyEmail Email = new MyEmail();
            Email.Emails = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid).
                Select(em => new Email
                {
                    EmailGuid = em.tblEmail.EmailGuid,
                    TemplateGuid = em.tblEmail.FK_TemplateGuid,
                    Subject = em.tblEmail.Subject,
                    DateCreated = em.tblEmail.DateCreated,
                    FromName = em.tblEmail.tblUser.FullName,
                    Read = em.Read
                })
                .OrderByDescending(x => x.DateCreated).OrderByDescending(x => x.Read == false).Take(2).ToList();
            Email.UnReadCount = bringlyEntities.tblEmailToes.Where(x => x.FK_UserGuid == UserVariables.LoggedInUserGuid
            && x.IsDeleted == false && x.tblEmail.Sent == true && x.Read == false && x.FK_EmailGuid == x.tblEmail.EmailGuid)
                .ToList().Count;
            int orderStatusId = (int)OrderStatus.Incomplete;
            Email.CartCount = bringlyEntities.tblOrderItems
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.FK_OrderGuid == x.tblOrder.OrderGuid
                && x.tblOrder.FK_OrderStatusId == orderStatusId)
              .ToList().Sum(x => x.Quantity);
            return Email;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public ComposeEmail GetEmailByEmailGuid(Guid EmailGuid)
        {
            ComposeEmail myemail = new ComposeEmail();
            List<tblEmailTo> emailto = bringlyEntities.tblEmailToes.Where(x => x.FK_EmailGuid == EmailGuid).ToList();//.Select(up => new tblEmailTo { EmailTo = up.EmailTo});
            myemail.EmailMessage = bringlyEntities.tblEmails.Where(x => x.EmailGuid == EmailGuid).Select(em => new Email
            {
                EmailGuid = em.EmailGuid,
                TemplateGuid = em.FK_TemplateGuid,
                Subject = em.Subject,
                Body = em.Body
                ,
                EmailFrom = em.EmailFrom,
                DateCreated = em.DateCreated,
                CreatedByGuid = em.FK_CreatedByGuid,
                ToName = bringlyEntities.tblUsers.Where(x => x.UserGuid == em.FK_CreatedByGuid).FirstOrDefault().FullName
            })
                .ToList().FirstOrDefault();
            //  myemail.EmailMessage.EmailToGuid = bringlyEntities.tblEmails.Where(x => x.EmailGuid == EmailGuid).Select(x => x.EmailFrom).ToList().FirstOrDefault();
            myemail.EmailMessage.EmailFrom = string.Join(",", emailto.Select(x => x.EmailTo));
            return myemail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool SendUserVerificationConfirmationEmail(Guid guid)
        {
            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == guid && usr.IsDeleted == false && usr.IsVerified != true).FirstOrDefault();
            if (user == null || user.EmailAddress == null)
                return false;
            string currentDomainName = string.Empty;
            Utilities.Helper.UtilityHelper.GetHostedDomainName(out currentDomainName);
            var confirmUrl = currentDomainName + "/home/verification/" + user.UserGuid;
            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = user.EmailAddress;
            emailDomain.EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            tblEmailTemplate Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "UserVerification").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", user.FullName).Replace("{hostUrl}", currentDomainName).Replace("{confirmUrl}", confirmUrl).Replace("{logoUrl}", currentDomainName + "/Templates/images/bringlylogoemail.png");
            string emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool SendThankUForVerificationEmail(Guid guid)
        {
            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == guid && usr.IsDeleted == false && usr.IsVerified == true).FirstOrDefault();
            if (user == null || user.EmailAddress == null)
                return false;

            string currentDomainName = string.Empty;
            Utilities.Helper.UtilityHelper.GetHostedDomainName(out currentDomainName);
            string imgSrc = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.User, "bringlylogoemail.png");
            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = user.EmailAddress;
            emailDomain.EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            tblEmailTemplate Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "ThankUForVerification").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", user.FullName).Replace("{hostUrl}", currentDomainName).Replace("{logoUrl}", currentDomainName + "/Templates/images/bringlylogoemail.png");
            string emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool SendThankUForSocialSignUpEmail(Guid guid)
        {
            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == guid && usr.IsDeleted == false).FirstOrDefault();
            if (user == null || user.EmailAddress == null)
                return false;

            string currentDomainName = string.Empty;
            Utilities.Helper.UtilityHelper.GetHostedDomainName(out currentDomainName);
            string imgSrc = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.User, "bringlylogoemail.png");
            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = user.EmailAddress;
            emailDomain.EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            tblEmailTemplate Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "ThankYouSocialSignUp").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", user.FullName).Replace("{hostUrl}", currentDomainName).Replace("{logoUrl}", currentDomainName + "/Templates/images/bringlylogoemail.png");
            string emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool SendUserPassword(string emailAddress)
        {
            bool isPasswordSent = false;
            var user = bringlyEntities.tblUsers.Where(usr => usr.IsDeleted == false && usr.IsVerified == true && usr.EmailAddress == emailAddress).FirstOrDefault();
            if (user == null)
                return isPasswordSent;

            string password = EncryptionHelper.DecryptText(user.Password, user.EmailAddress);

            if (string.IsNullOrEmpty(password))
                return false;

            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = user.EmailAddress;
            emailDomain.EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            tblEmailTemplate Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "ForgetPassword").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", user.FullName).Replace("{username}", user.EmailAddress).Replace("{password}", password);
            string emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="composeEmail"></param>
        /// <returns></returns>
        public bool SendContactUsMessage(ComposeEmail composeEmail)
        {
            var admin = bringlyEntities.tblUsers
                .Where(usr => usr.IsActive == true && usr.IsDeleted == false && usr.UserRegistrationType == 3)
                .Select(usr => usr)
                .FirstOrDefault();

            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = admin.EmailAddress;
            emailDomain.EmailFrom = composeEmail.EmailMessage.EmailFrom;
            tblEmailTemplate Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "ContactUsAdmin").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = composeEmail.EmailMessage.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", admin.FullName).Replace("{message}", composeEmail.EmailMessage.Body).Replace("{EmailFrom}",composeEmail.EmailMessage.FromName);
            string emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }
            tblEmail tblEmail = new tblEmail();
            tblEmail.EmailGuid = Guid.NewGuid();
            tblEmail.EmailFrom = composeEmail.EmailMessage.EmailFrom;
            tblEmail.Subject = composeEmail.EmailMessage.Subject;
            tblEmail.Body = composeEmail.EmailMessage.Body;
            tblEmail.Sent = (emailSendResult == "") ? true : false;
            tblEmail.FK_TemplateGuid = Template.TemplateGuid;
            tblEmail.DateCreated = DateTime.Now;
            // tblEmail.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
            bringlyEntities.tblEmails.Add(tblEmail);
            if (tblEmail.Sent)
            {
                tblEmailTo tblEmailTo = new tblEmailTo();

                tblEmailTo.EmailToGuid = Guid.NewGuid();
                tblEmailTo.FK_EmailGuid = tblEmail.EmailGuid;
                tblEmailTo.EmailTo = emailDomain.EmailTo;
               // tblEmailTo.FK_UserGuid = admin.UserGuid;
                bringlyEntities.tblEmailToes.Add(tblEmailTo);
            }
            bringlyEntities.SaveChanges();

            emailDomain = new EmailDomain();
            emailDomain.EmailTo = composeEmail.EmailMessage.EmailFrom;
            emailDomain.EmailFrom = admin.EmailAddress;
            Template = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == "ContactUsThankYou").FirstOrDefault();

            if (Template == null)
                return false;

            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", composeEmail.EmailMessage.FromName);
            emailSendResult = EmailSender.sendEmail(emailDomain);
            if (!string.IsNullOrEmpty(emailSendResult))
            {
                ErrorLog.LogError(emailSendResult, "send email Error");
                return false;
            }

            tblEmail = new tblEmail();
            tblEmail.EmailGuid = Guid.NewGuid();
            tblEmail.EmailFrom = admin.EmailAddress;
            tblEmail.Subject = emailDomain.EmailSubject;
            tblEmail.Body = emailDomain.EmailBody;
            tblEmail.Sent = (emailSendResult == "") ? true : false;
            tblEmail.FK_TemplateGuid = Template.TemplateGuid;
            tblEmail.DateCreated = DateTime.Now;
            // tblEmail.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
            bringlyEntities.tblEmails.Add(tblEmail);
            if (tblEmail.Sent)
            {
                tblEmailTo tblEmailTo = new tblEmailTo();

                tblEmailTo.EmailToGuid = Guid.NewGuid();
                tblEmailTo.FK_EmailGuid = tblEmail.EmailGuid;
                tblEmailTo.EmailTo = emailDomain.EmailTo;
                //  tblEmailTo.FK_UserGuid = admin.UserGuid;
                bringlyEntities.tblEmailToes.Add(tblEmailTo);
            }
            bringlyEntities.SaveChanges();

            return true;
        }

    }
}
