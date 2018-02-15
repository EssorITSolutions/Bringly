using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.Domain;
using Bringly.Domain.User;
using Bringly.DomainLogic;
using Bringly.DomainLogic.User;

namespace Bringly.UI.Controllers
{
    public class MenuController : Controller
    {
        [ChildActionOnlyAttribute]
        public PartialViewResult ChoosePreferedCity()
        {
            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            UserDomainLogic _userDomainLogic = new UserDomainLogic();            
            ChooseCity chooseCity = new ChooseCity();
            UserProfile _FindUser = _userDomainLogic.FindUser(UserVariables.LoggedInUserId);
            chooseCity.Cities = commonDomainLogic.GetCities();
            chooseCity.SelectedCity = commonDomainLogic.GetPreferedCity();
            return PartialView("_chooseCity", chooseCity);
        }
        public PartialViewResult TopMenu()
        {
            return PartialView("_topUserMenu");
        }
    }
}