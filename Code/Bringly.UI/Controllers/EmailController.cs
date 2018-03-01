using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain;
using Bringly.DomainLogic;

namespace Bringly.UI.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Inbox(Guid EmailGuid)
        {
            int Currentpage = 0;
            TempData["CurrentPage"] = null;
            if (EmailGuid == Guid.Empty) {
                if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
                {
                    TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                    Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                    TempData.Keep();
                }
                
            }
             
            EmailDomainLogic email = new EmailDomainLogic();
            return View(email.GetInboxEmail(EmailGuid, Currentpage));
        }
        [HttpPost]
        public ActionResult Inbox(MyEmail MyEmail)
        {
            EmailDomainLogic email = new EmailDomainLogic();
           // email.SendEmail(MyEmail); compose email
            return View();
        }
  
        [HttpPost]
        public ActionResult Sent(List<Email> MyEmail)
        {
            return View();
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
            return Json(email.DeleteEmail(EmailGuid),JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSentEmailPartial()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
            TempData.Keep();
            return PartialView("_EmailList",email.GetSentEmail(Currentpage));
        }
        public ActionResult GetInboxEmailPartial()
        {
            EmailDomainLogic email = new EmailDomainLogic();
            int Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
            TempData.Keep();
            return PartialView("_EmailList", email.GetInboxEmail(Guid.Empty, Currentpage));
        }
        public ActionResult MarkAsRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            return Json(email.MarkAsRead(EmailGuid),JsonRequestBehavior.AllowGet);
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
        public ActionResult ComposeEmail()
        {
            return PartialView("_ComposeEmail",new ComposeEmail());
        }
        [HttpPost]
        public ActionResult ComposeEmail(ComposeEmail ComposeEmail)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            if (ModelState.IsValid)
            {
                MyEmail myEmail = new MyEmail();
                myEmail.EmailTo = ComposeEmail.EmailTo;
                myEmail.Subject = ComposeEmail.Subject;
                myEmail.Body = ComposeEmail.Body;
                email.SendEmail(myEmail);
                ViewBag.IspopUp = true;
            }
            return PartialView("_ComposeEmail", new ComposeEmail());
        }

        public ActionResult MarkNotificationRead(Guid[] EmailGuid)
        {
            EmailDomainLogic email = new EmailDomainLogic();
            email.MarkAsRead(EmailGuid);
            return PartialView("_NotificationMessage", email.GetNotificationEmail(Guid.Empty));
        }
    }
}