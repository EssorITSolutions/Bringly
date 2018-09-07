using Bringly.DomainLogic;
using System.Web.Mvc;
using System.IO;

namespace Bringly.UI.Controllers
{
    public class CMSController : BaseClasses.AnonymousUserControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actioName"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Index(string actioName)
        {
            if (string.IsNullOrEmpty(actioName))
                return RedirectTo404();

            CMSDomainLogic cMSDomainLogic = new CMSDomainLogic();

            var model = cMSDomainLogic.GetCMSPageByAlias(actioName);

            if (model == null)
                return RedirectTo404();
            model.PageDescription = System.Net.WebUtility.HtmlDecode(model.PageDescription);

            return View("Index", model);
        }
    }
}