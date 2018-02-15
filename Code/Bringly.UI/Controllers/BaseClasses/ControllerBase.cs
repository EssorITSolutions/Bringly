using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Bringly.UI.Controllers.BaseClasses
{
    public class ControllerBase : Controller
    {
        #region Message Functions
        /// <summary>
        /// For Error message that stays until we close
        /// </summary>
        /// <param name="msg"></param>
        protected void ErrorBlock(string msg)
        {
            ViewBag.ErrorBlock = msg;
        }
        /// <summary>
        /// success message closed after 5000 millisecond
        /// </summary>
        /// <param name="msg"></param>
        protected void Success(string msg)
        {
            ViewBag.Success = msg;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("login", "usermembership");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ActionResult RedirectTo404()
        {
            return RedirectToAction("error404", "error");
        }

        #region Ajax Messages
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected JsonResult ReturnAjaxModelError()
        {
            return Json(new
            {
                Success = false,
                Message = string.Join("<br/>", ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray())
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected JsonResult ReturnJsonResult(object value)
        {
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected JsonResult ReturnAjaxErrorMessage(string errorMessage)
        {
            return Json(new
            {
                Success = false,
                Message = errorMessage
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        protected JsonResult ReturnAjaxSuccessMessage(string successMessage)
        {
            return Json(new
            {
                Success = true,
                Message = successMessage
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected JsonResult ReturnAjaxSuccessMessage(Dictionary<string, string> message, string mssage = "")
        {
            message.Add("Success", "true");
            message.Add("Message", mssage);
            return Json(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="mssage"></param>
        /// <returns></returns>
        protected JsonResult ReturnAjaxErrorMessage(Dictionary<string, string> message, string mssage = "")
        {
            message.Add("Success", "false");
            message.Add("Message", mssage);
            return Json(message);
        }
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, string> GetDictionaryForAjaxResult
        {
            get
            {
                return new Dictionary<string, string>();
            }
        }
        #endregion
    }
}