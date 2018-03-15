using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain;
using Bringly.Domain.Enums;
using Bringly.Domain.User;
using Bringly.DomainLogic;
using Bringly.DomainLogic.User;

namespace Bringly.UI.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Inbox(Nullable<Guid> EmailGuid)
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
            return View(email.GetInboxEmail(Currentpage));
        }
        
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

        public ActionResult DeleteEmail(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.DeleteEmail(EmailGuid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Inbox()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"].ToType<int>(0);
            TempData.Keep();
            return PartialView("_EmailList", email.GetInboxEmail(Currentpage));
        }

        public ActionResult MarkAsRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.MarkAsRead(EmailGuid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MarkAsUnRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.MarkAsUnRead(EmailGuid), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnReadEmailCount()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.GetUnReadEmailCount(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MarkNotificationRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            if (EmailGuid != null && EmailGuid.Count() > 0)
            {
                email.MarkAsRead(EmailGuid);
            }
            return PartialView("_NotificationMessage", email.GetNotificationEmail(Guid.Empty));
        }
        public ActionResult ComposeEmail(Nullable<Guid> EmailGuid,Nullable<bool> Isreply)
        {
            ComposeEmail ComposeEmail = new ComposeEmail();
            UserDomainLogic UserDomainLogic = new UserDomainLogic();
            EmailDomainLogic EmailDomainLogic = new EmailDomainLogic();
    
            if (EmailGuid != null && EmailGuid != Guid.Empty)
            {
                ComposeEmail = EmailDomainLogic.GetEmailByEmailGuid(EmailGuid.HasValue?EmailGuid.Value:Guid.Empty);
                ComposeEmail.EmailMessage.Body = "<br><hr>"+ ComposeEmail.EmailMessage.Body;
                ComposeEmail.EmailMessage.Subject = (Isreply.HasValue ? (Isreply.Value?"Re: ":"Fwd: ") : "")+ ComposeEmail.EmailMessage.Subject;
               // ComposeEmail.EmailMessage.EmailTo = (Isreply.HasValue ? (Isreply.Value ? ComposeEmail.EmailMessage.EmailTo : "") : "");                
                ComposeEmail.Isemailreplyorforward = true;
            }
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
            { ComposeEmail.UserContactList = UserDomainLogic.GetAllMerchants(); }
            else { ComposeEmail.UserContactList = UserDomainLogic.GetAllBuyers();}
            
            return PartialView("_ComposeEmail", ComposeEmail);
        }
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
            UserDomainLogic UserDomainLogic = new UserDomainLogic();
            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
            { ComposeEmail.UserContactList = UserDomainLogic.GetAllMerchants(); }
            else { ComposeEmail.UserContactList = UserDomainLogic.GetAllBuyers(); }
            return PartialView("_ComposeEmail", ComposeEmail);
        }
        public ActionResult NoRecordFoundPartial()
        {
            return PartialView("_NoRecordFound","No message(s) found.");
        }


        //[HttpPost]
        //public ActionResult ComposeEmailDetail(Guid EmailGuid)
        //{

        //    return Json(ComposeEmail,JsonRequestBehavior.AllowGet);
        //}

    }
}