using Bringly.DomainLogic;
using System;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class WalletController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// Get wallet histry for the logged in user.
        /// </summary>
        /// <returns>List of wallet histpry</returns>
        [HttpGet]
        [ActionName("getwallethistory")]
        public ActionResult GetWalletHistory()
        {
            WalletDomainLogic walletDomainLogic = new WalletDomainLogic();
            int Currentpage = 0;
            TempData["CurrentPage"] = null;

            if (Request.Url.AbsoluteUri.Contains("?") && Request.Url.AbsoluteUri.Contains("page"))
            {
                TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                TempData.Keep();
            }

            var model = walletDomainLogic.GetWalletHistory(UserVariables.LoggedInUserGuid, Currentpage);
            return View("WalletHistory", model);
        }
    }
}