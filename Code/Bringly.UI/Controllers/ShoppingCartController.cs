using Bringly.Domain;
using Bringly.DomainLogic;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class ShoppingCartController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// Get cart
        /// </summary>
        /// <returns>Shopping cart</returns>
        public ActionResult Cart()
        {
            ShoppingCart OrderItems = new ShoppingCart();
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return View(shoppingCartDomainLogic.fillCart());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShoppingCart"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cart(ShoppingCart ShoppingCart)
        {
            ShoppingCart OrderItems = new ShoppingCart();
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            shoppingCartDomainLogic.CartCheckout(ShoppingCart);
            return RedirectToAction("delivery", "orders", new { orderGuid = ShoppingCart.OrderGuid });// View("PaymentOption", deliveryOption);
            //return View(shoppingCartDomainLogic.fillCart());
        }
    }
}