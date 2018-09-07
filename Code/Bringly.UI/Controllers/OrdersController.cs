using Bringly.Domain;
using Bringly.Domain.Enums;
using Bringly.DomainLogic;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class OrdersController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// Get buyer orders
        /// </summary>
        /// <param name="sortBy">The column name to sort the orders</param>
        /// <param name="sortOrder">Sort order(asc, desc)</param>
        /// <param name="orderStatus">Order status to sort the orders</param>
        /// <returns>Orders for the given user guid</returns>
        public ActionResult MyOrders(string sortBy, string sortOrder, OrderStatus? orderStatus, string activeTab)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            MyOrder myOrder = new MyOrder();
            ViewBag.ActiveTab = activeTab;

            int Currentpage = 0;
            TempData["CurrentPage"] = null;

            if (Request.Url.AbsoluteUri.Contains("?") && Request.Url.AbsoluteUri.Contains("page"))
            {
                var queryStringArray = Request.Url.AbsoluteUri.Split('&');//[4].Split('=')[1].Split('&')[0];
                var pageString = queryStringArray[queryStringArray.Length - 1];
                var pageSize = pageString.Split('=')[1];
                TempData["CurrentPage"] = pageSize;
                Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                TempData.Keep();
            }

            if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Buyer)
                myOrder = orderDomainLogic.GetBuyerOrders(UserVariables.LoggedInUserGuid, orderStatus, sortBy, sortOrder, activeTab, Currentpage);

            else if (UserVariables.UserRole == Domain.Enums.User.UserRoles.Merchant)
                return View("MyOrders", myOrder);

            return View("MyOrders", myOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestQuery"></param>
        /// <param name="activeTab"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMerchantMyOrders(RequestQuery requestQuery, string activeTab)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            var loggedInGuid = UserVariables.LoggedInUserGuid;
            var data =
               await Task.Run(() =>
               {
                   return orderDomainLogic.GetMerchantOrders(loggedInGuid, requestQuery, activeTab);
               });

            return Json(new { myOrderData = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get order details 
        /// </summary>
        /// <param name="orderGuid">OrderGuid to get orde details</param>
        /// <returns>order details for given order guid</returns>
        [ActionName("details")]
        public ActionResult OrderDetail(Guid orderGuid)
        {
            Order order = new Order();
            if (orderGuid != Guid.Empty)
            {
                OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
                order = orderDomainLogic.GetOrderDetails(orderGuid);
            }
            return View("OrderDetailBuyer", order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("updateorderstatus")]
        public ActionResult UpdateOrderStatus(Guid orderGuid, OrderStatus orderStatus)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            return Json(orderDomainLogic.UpdateOrderStatus(orderGuid, orderStatus), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("delivery")]
        public ActionResult DeliveryOption(Guid orderGuid)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            //Guid.TryParse("B176BAD2-2BC8-4648-86E2-0392703B5CE2", out orderGuid);
            var model = orderDomainLogic.GetDeliveryOption(orderGuid);
            return View("Delivery", model);
        }

        [HttpPost]
        [ActionName("delivery")]
        public ActionResult SaveDeliveryOption(DeliveryOption deliveryOption)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            if (deliveryOption.DeliveryType == OrderType.SelfDelivery && string.IsNullOrEmpty(deliveryOption.ShippingAddress.Country))
            {
                ModelState.AddModelError("ShippingAddress.Country", "Select Country.");
                var model = orderDomainLogic.GetDeliveryOption(deliveryOption.OrderGuid);
                deliveryOption.CountryList = model.CountryList;
                deliveryOption.CityList = model.CityList;
                return View("Delivery", deliveryOption);
            }
           
            var result = orderDomainLogic.SaveDeliveryOption(deliveryOption);
            return RedirectToAction("paymentoption", new {orderGuid = deliveryOption.OrderGuid });// View("PaymentOption", deliveryOption);
        }

        [HttpGet]
        [ActionName("paymentoption")]
        public ActionResult PaymentOption(Guid orderGuid)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            var model = orderDomainLogic.GetPaymentType(orderGuid);
            ViewBag.orderGuid = orderGuid;
            return View("PaymentOption", model);
        }

        [HttpPost]
        [ActionName("paymentoption")]
        public ActionResult OrderConfirmation(Guid orderGuid, PaymentType paymentType)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            var result = orderDomainLogic.UpdateDeliveryOptionPaymentMethod(orderGuid, paymentType);
            if (!result)
                return RedirectTo404();
            var model = orderDomainLogic.GetOrderDetails(orderGuid);
            return RedirectToAction("orderconfirmation", new {orderGuid = orderGuid });//return View("OrderConfirmation", model);
        }

        [HttpGet]
        [ActionName("orderconfirmation")]
        public ActionResult OrderConfirmation(Guid orderGuid)
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();
            var model = orderDomainLogic.GetOrderDetails(orderGuid);
            return View("OrderConfirmation", model);
        }

        [HttpPost]
        [ActionName("orderconfirm")]
        public ActionResult OrderConfirm()
        {
            OrderDomainLogic orderDomainLogic = new OrderDomainLogic();

            return View("ThankUOrder");
        }
    }
}