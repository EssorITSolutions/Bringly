using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bringly.DomainLogic;
using Bringly.Domain;
using Bringly.DomainLogic.User;
using Bringly.Domain.User;

namespace Bringly.UI.Controllers
{
    public class ShoppingCartController : BaseClasses.AuthoriseUserControllerBase
    {
        // GET: ShoppingCart
        public ActionResult Cart()
        {
            ShoppingCart OrderItems = new ShoppingCart();
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return View(shoppingCartDomainLogic.GetItemsIncart());
        }
        [HttpPost]
        public ActionResult Cart(ShoppingCart ShoppingCart)
        {
            ShoppingCart OrderItems = new ShoppingCart();
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            shoppingCartDomainLogic.CartCheckout(ShoppingCart);
            return View(shoppingCartDomainLogic.GetItemsIncart());
        }
    }
}