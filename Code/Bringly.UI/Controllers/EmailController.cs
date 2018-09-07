using Bringly.Domain;
using Bringly.Domain.Enums;
using Bringly.DomainLogic;
using Bringly.DomainLogic.User;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class EmailController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public EmailController()
        {

        }
       
        // GET: Email
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <param name="EmailSearchQuery"></param>
        /// <param name="EmailSortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Inbox(Nullable<Guid> EmailGuid, string EmailSearchQuery, string EmailSortOrder)
        {
            int Currentpage = 0;
            TempData["CurrentPage"] = null;
            if (EmailGuid == Guid.Empty)
            {
                if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
                {
                    TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                    Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                    TempData.Keep();
                }
            }
            EmailDomainLogic email = new EmailDomainLogic();
            return View(email.GetInboxEmail(Currentpage, EmailSearchQuery, EmailSortOrder));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Sent()
        {
            TempData["CurrentPage"] = null;
            if (Request.Url.AbsoluteUri.Contains('?'))
            {
                TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('=')[1].Split('&')[0];
            }
            EmailDomainLogic email = new EmailDomainLogic();
            MyEmail myemail = new MyEmail();
            int Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
            TempData.Keep();
            myemail = email.GetSentEmail(Currentpage);
            return View(myemail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public ActionResult DeleteEmail(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.DeleteEmail(EmailGuid), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Inbox()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"].ToType<int>(0);
            TempData.Keep();
            return PartialView("_EmailList", email.GetInboxEmail(Currentpage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public ActionResult MarkAsRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.MarkAsRead(EmailGuid), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public ActionResult MarkAsUnRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.MarkAsUnRead(EmailGuid), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUnReadEmailCount()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.GetUnReadEmailCount(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <returns></returns>
        public ActionResult MarkNotificationRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            if (EmailGuid != null && EmailGuid.Count() > 0)
            {
                email.MarkAsRead(EmailGuid);
            }

            return PartialView("_NotificationMessage", email.GetNotificationEmail(Guid.Empty));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmailGuid"></param>
        /// <param name="Isreply"></param>
        /// <returns></returns>
        public ActionResult ComposeEmail(Nullable<Guid> EmailGuid, Nullable<bool> Isreply)
        {
            ComposeEmail ComposeEmail = new ComposeEmail();
            UserDomainLogic UserDomainLogic = new UserDomainLogic();
            EmailDomainLogic EmailDomainLogic = new EmailDomainLogic();
            ViewBag.PopupTitle = "Send Message";
            if (EmailGuid != null && EmailGuid != Guid.Empty)
            {
                ComposeEmail = EmailDomainLogic.GetEmailByEmailGuid(EmailGuid.HasValue ? EmailGuid.Value : Guid.Empty);
                ComposeEmail.EmailMessage.Body = "<br><hr>" + ComposeEmail.EmailMessage.Body;
                ComposeEmail.EmailMessage.Subject = (Isreply.HasValue ? (Isreply.Value ? "Re: " : "Fwd: ") : "") + ComposeEmail.EmailMessage.Subject;
                // ComposeEmail.EmailMessage.EmailTo = (Isreply.HasValue ? (Isreply.Value ? ComposeEmail.EmailMessage.EmailTo : "") : "");                
                ComposeEmail.Isemailreplyorforward = true;
            }
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
            { ComposeEmail.UserContactList = UserDomainLogic.GetAllMerchants(); }
            else { ComposeEmail.UserContactList = UserDomainLogic.GetAllBuyers(); }

            return PartialView("_ComposeEmail", ComposeEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComposeEmail"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ComposeEmail(ComposeEmail ComposeEmail)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            if (ModelState.IsValid)
            {
                ComposeEmail.EmailMessage.TemplateType = Enum.GetName(typeof(TemplateType), TemplateType.Mail);
                ViewBag.Ismessagesent = email.SendEmail(ComposeEmail);
                ViewBag.IspopUp = true;
            }

            ViewBag.PopupTitle = "Send Message";
            UserDomainLogic UserDomainLogic = new UserDomainLogic();
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
            { ComposeEmail.UserContactList = UserDomainLogic.GetAllMerchants(); }
            else { ComposeEmail.UserContactList = UserDomainLogic.GetAllBuyers(); }
            return PartialView("_ComposeEmail", ComposeEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound", "No message(s) found.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getemail")]
        public ActionResult GetEmailByGuid(Guid emailGuid)
        {
            EmailDomainLogic emailDomainLogic = new EmailDomainLogic();
            ViewBag.PopupTitle = "Message";
            var model = emailDomainLogic.GetEmailDetailsByGuid(emailGuid);
            return View("_GetEmailPartial", model);
        }
    }
}