using System.Web.Mvc;

namespace Bringly.Admin.Controllers.BaseClasses
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
            ViewBag.errorBlock = msg;
        }
        /// <summary>
        /// success message closed after 5000 millisecond
        /// </summary>
        /// <param name="msg"></param>
        protected void Success(string msg)
        {
            ViewBag.success = msg;
        }
        #endregion
    }
}