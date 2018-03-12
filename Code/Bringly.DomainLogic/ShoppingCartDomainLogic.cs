using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Enums;
using Bringly.DomainLogic.User;

namespace Bringly.DomainLogic
{
    public class ShoppingCartDomainLogic : BaseClass.DomainLogicBase
    {
        public ShoppingCart GetItemsIncart()
        {
            ShoppingCart OrderItemList = new ShoppingCart();
            string orderStatus = Enum.GetName(typeof(OrderStatus), OrderStatus.Incomplete);            
            List<Items> Items = bringlyEntities.tblOrderItems.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblOrder.OrderStatus == orderStatus).ToList()
                .Select(a => new Items { ItemWeight=a.tblItem.ItemWeight, ItemImage =a.tblItem.ItemImage,ItemGuid=a.ItemGuid ,ItemSize=a.tblItem.ItemSize,ItemName=a.tblItem.ItemName,Quantity=a.Quantity
                ,Discount=a.tblItem.Discount ,ItemPrice=a.tblItem.ItemPrice,DeliveryCharge=a.tblItem.DeliveryCharge.HasValue? a.tblItem.DeliveryCharge.Value:0}).ToList();
            OrderItemList.ItemsList = Items;
            OrderItemList.SubTotal = OrderItemList.ItemsList.Sum(x => x.Quantity * x.ItemPrice);
            OrderItemList.Discount = OrderItemList.ItemsList.Sum(x => x.Discount);
            OrderItemList.DeliveryCharge = OrderItemList.ItemsList.Sum(x => x.DeliveryCharge);
            OrderItemList.Total = OrderItemList.SubTotal - OrderItemList.Discount + OrderItemList.DeliveryCharge;

            return OrderItemList;
        }
        public bool CartCheckout(ShoppingCart shoppingCart)
        {
            if (shoppingCart.ItemsList != null && shoppingCart.ItemsList.Count > 0)
            {
                string orderstatus = Enum.GetName(typeof(OrderStatus), OrderStatus.Completed);
                tblOrder tblOrder = bringlyEntities.tblOrders.Where(x => x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.OrderStatus != orderstatus).FirstOrDefault();
               
                foreach (Items item in shoppingCart.ItemsList)
                {
                    tblOrderItem tblItem = bringlyEntities.tblOrderItems.Where(x => x.ItemGuid == item.ItemGuid && x.OrderGuid == x.tblOrder.OrderGuid && x.CreatedByGuid == UserVariables.LoggedInUserGuid && x.tblOrder.OrderStatus != orderstatus).FirstOrDefault();
                    tblItem.Quantity = item.Quantity;
                    bringlyEntities.SaveChanges();
                }
              
                tblOrder.OrderStatus = Enum.GetName(typeof(OrderStatus), OrderStatus.Inprogress);
                tblOrder.OrderSubTotal = shoppingCart.SubTotal;
                tblOrder.OrderTotal = shoppingCart.Total;
                bringlyEntities.SaveChanges();
                return true;
            }
            else {
                return false;
            }
         
        }
    }
}
