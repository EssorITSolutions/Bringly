using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bringly.DomainLogic
{
    public class ShoppingCartDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ShoppingCart fillCart()
        {
            ShoppingCart OrderItemList = new ShoppingCart();
            int orderStatus = (int)OrderStatus.Incomplete;

            var items = (from odr in bringlyEntities.tblOrders
                         join oi in bringlyEntities.tblOrderItems on odr.OrderGuid equals oi.FK_OrderGuid
                         where odr.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && odr.FK_OrderStatusId != null
                         && odr.FK_OrderStatusId == orderStatus
                         select new Items
                         {
                             ItemWeight = oi.tblItem.ItemWeight,
                             ItemImage = oi.tblItem.ItemImage,
                             ItemGuid = oi.FK_ItemGuid,
                             ItemSize = oi.tblItem.ItemSize,
                             ItemName = oi.tblItem.ItemName,
                             Quantity = oi.Quantity,
                             Discount = oi.tblItem.Discount,
                             ItemPrice = oi.tblItem.ItemPrice,
                             DeliveryCharge = oi.tblItem.DeliveryCharge.HasValue ? oi.tblItem.DeliveryCharge.Value : 0
                         }).ToList();

            OrderItemList.ItemsList = items;
            OrderItemList.SubTotal = OrderItemList.ItemsList.Sum(x => x.Quantity * x.ItemPrice);
            OrderItemList.Discount = OrderItemList.ItemsList.Sum(x => x.Discount);
            OrderItemList.DeliveryCharge = OrderItemList.ItemsList.Sum(x => x.DeliveryCharge);
            OrderItemList.Total = OrderItemList.SubTotal - OrderItemList.Discount + OrderItemList.DeliveryCharge;

            OrderItemList.OrderGuid = bringlyEntities.tblOrders
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                && x.FK_OrderStatusId != null && x.FK_OrderStatusId == orderStatus)
                .Select(o => o.OrderGuid)
                .FirstOrDefault();

            return OrderItemList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public bool CartCheckout(ShoppingCart shoppingCart)
        {
            if (shoppingCart.ItemsList != null && shoppingCart.ItemsList.Count > 0)
            {
                int orderstatus = (int)OrderStatus.Incomplete;

                tblOrder tblOrder = bringlyEntities.tblOrders
                    .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.FK_OrderStatusId == orderstatus)
                    .FirstOrDefault();

                foreach (Items item in shoppingCart.ItemsList)
                {
                    tblOrderItem tblItem = bringlyEntities.tblOrderItems
                        .Where(x => x.FK_ItemGuid == item.ItemGuid && x.FK_OrderGuid == x.tblOrder.OrderGuid
                        && x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                        && x.tblOrder.FK_OrderStatusId == orderstatus)
                        .FirstOrDefault();

                    tblItem.Quantity = item.Quantity;
                    bringlyEntities.SaveChanges();
                }

                tblOrder.FK_OrderStatusId = (int)OrderStatus.Inprogress;
                //tblOrder.OrderSubTotal = shoppingCart.SubTotal;
                tblOrder.OrderTotal = shoppingCart.Total;
                bringlyEntities.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool addToCart(Items item)
        {
            Guid OrderGuid = Guid.Empty;
            int orderStatus = (int)OrderStatus.Incomplete;

            tblOrder tblOrder = bringlyEntities.tblOrders
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.FK_OrderStatusId != orderStatus)
                .FirstOrDefault();

            if (tblOrder != null && tblOrder.OrderGuid != Guid.Empty)
            {
                OrderGuid = tblOrder.OrderGuid;
                tblOrder.FK_OrderStatusId = (int)(OrderStatus.Incomplete);
            }
            else
            {
                tblOrder = new tblOrder();
                tblOrder.OrderGuid = Guid.NewGuid();
                OrderGuid = tblOrder.OrderGuid;
                tblOrder.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
                tblOrder.FK_OrderStatusId = (int)OrderStatus.Incomplete;
                tblOrder.OrderDiscount = 0;
                //tblOrder.OrderSubTotal = 0;
                tblOrder.OrderTotal = 0;
                tblOrder.IsDeleted = false;
                bringlyEntities.tblOrders.Add(tblOrder);
            }

            bringlyEntities.SaveChanges();

            tblOrderItem OrderItem = bringlyEntities.tblOrderItems
                .Where(x => x.FK_ItemGuid == item.ItemGuid && x.FK_OrderGuid == OrderGuid
                && x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid)
                .FirstOrDefault();

            if (OrderItem != null && OrderItem.OrderItemGuid != Guid.Empty)
            {
                OrderItem.Quantity = item.Quantity + OrderItem.Quantity;
            }
            else
            {
                OrderItem = new tblOrderItem();
                OrderItem.OrderItemGuid = Guid.NewGuid();
                OrderItem.FK_OrderGuid = OrderGuid;
                OrderItem.FK_ItemGuid = item.ItemGuid;
                OrderItem.Quantity = item.Quantity;
                OrderItem.UnitPrice = 0;
                OrderItem.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
                OrderItem.DateCreated = DateTime.Now;
                bringlyEntities.tblOrderItems.Add(OrderItem);
            }

            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getCartCount()
        {
            int orderstatus = (int)OrderStatus.Incomplete;
            return bringlyEntities.tblOrderItems
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.FK_OrderGuid == x.tblOrder.OrderGuid && x.tblOrder.FK_OrderStatusId == orderstatus)
              .ToList().Sum(x => x.Quantity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <returns></returns>
        public bool deleteItemFromCart(Guid ItemGuid)
        {
            int orderstatus = (int)OrderStatus.Incomplete;
            tblOrder tblOrder = bringlyEntities.tblOrders
                .Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid && x.FK_OrderStatusId == orderstatus)
                .FirstOrDefault();

            tblOrderItem OrderItem = bringlyEntities.tblOrderItems
                .Where(x => x.FK_ItemGuid == ItemGuid && x.FK_OrderGuid == tblOrder.OrderGuid && x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid)
                .FirstOrDefault();

            bringlyEntities.tblOrderItems.Attach(OrderItem);
            bringlyEntities.tblOrderItems.Remove(OrderItem);
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discountObj"></param>
        /// <returns></returns>
        public bool AddDiscount(Discount discountObj)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            if (discountObj.ProductIds != null)
            {
                foreach (var id in discountObj.ProductIds)
                {
                    stringBuilder.Append(id.ToString() + ",");
                }
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }

            tblCoupon discount = new tblCoupon();
            discount.FK_BranchGuid = discountObj.BranchGuid;
            discount.CouponDescription = discountObj.CouponDescription;
            discount.CouponName = discountObj.CouponName;
            discount.CouponPriceType = discountObj.DiscountPriceType;
            discount.FK_CreatedByGuid = UserVariables.LoggedInUserGuid;
            discount.DateCreated = DateTime.Now;
            discount.DiscountLimitation = discountObj.DiscountLimitationType;
            discount.DiscountType = discountObj.DiscountType;
            discount.DiscountValue = discountObj.DiscountValue;
            discount.EndDate = discountObj.EndDate;
            discount.IsActive = discountObj.IsActive;
            discount.IsDeleted = false;
            discount.StartDate = discountObj.StartDate;
            discount.ProductIds = stringBuilder.ToString();
            bringlyEntities.tblCoupons.Add(discount);
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discountObj"></param>
        /// <returns></returns>
        public bool UpdateDicountCoupon(Discount discountObj)
        {
            if (discountObj == null)
                return false;

            var discount = bringlyEntities.tblCoupons
                .Where(dis => dis.CouponId == discountObj.CouponId && dis.IsDeleted == false && dis.CouponId == discountObj.CouponId)
                .FirstOrDefault();

            if (discount == null)
                return false;

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (discountObj.ProductIds != null)
            {
                foreach (var id in discountObj.ProductIds)
                {
                    stringBuilder.Append(id.ToString() + ",");
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }

            discount.CouponName = discountObj.CouponName;
            discount.CouponDescription = discountObj.CouponDescription;
            discount.CouponPriceType = discountObj.DiscountPriceType;
            discount.DiscountLimitation = discountObj.DiscountLimitationType;
            discount.DiscountValue = discountObj.DiscountValue;
            discount.EndDate = discountObj.EndDate;
            discount.ProductIds = stringBuilder.ToString();
            discount.StartDate = discountObj.StartDate;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public bool DeleteDiscountCoupon(int couponId)
        {
            if (couponId == 0)
                return false;

            var discount = bringlyEntities.tblCoupons.Where(dis => dis.IsDeleted == false && dis.CouponId == couponId).FirstOrDefault();

            if (discount == null)
                return false;

            discount.IsDeleted = true;
            discount.IsActive = false;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LatestPage"></param>
        /// <returns></returns>
        public MyDiscount GetDiscountCoupons(int LatestPage = 0)
        {
            MyDiscount myDiscount = new MyDiscount();
            myDiscount.PageSize = 10;
            myDiscount.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;
            myDiscount.SortBy = SortBy;

            var data = (from dis in bringlyEntities.tblCoupons
                        where dis.IsDeleted == false
                        join branch in bringlyEntities.tblBranches on dis.FK_BranchGuid equals branch.BranchGuid
                        where branch.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                        select new Discount
                        {
                            BranchGuid = branch.BranchGuid,
                            CouponDescription = dis.CouponDescription,
                            BranchName = branch.BranchName,
                            CouponId = dis.CouponId,
                            CouponName = dis.CouponName,
                            CreatedByGuid = dis.FK_CreatedByGuid ?? Guid.Empty,
                            DateCreated = dis.DateCreated,
                            DiscountLimitationType = dis.DiscountLimitation,
                            DiscountPriceType = dis.CouponPriceType,
                            DiscountValue = dis.DiscountValue,
                            DiscountType = dis.DiscountType,
                            EndDate = dis.EndDate,
                            IsActive = dis.IsActive,
                            IsDeleted = dis.IsDeleted,
                            StartDate = dis.StartDate
                        });

            myDiscount.Discounts = data.OrderByDescending(dis => dis.DateCreated).ToList();

            myDiscount.TotalRecords = myDiscount.Discounts.Count;
            int skip = 0;
            if (myDiscount.CurrentPage == 1)
                skip = 0;
            else
                skip = ((myDiscount.CurrentPage * myDiscount.PageSize) - myDiscount.PageSize);

            myDiscount.Discounts = myDiscount.Discounts.Skip(skip).Take(myDiscount.PageSize).ToList();

            return myDiscount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public Discount GetDiscountCoupon(int couponId)
        {
            var data = (from dis in bringlyEntities.tblCoupons
                        where dis.IsDeleted == false && dis.CouponId == couponId
                        join branch in bringlyEntities.tblBranches on dis.FK_BranchGuid equals branch.BranchGuid
                        where branch.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                        select new Discount
                        {
                            BranchGuid = branch.BranchGuid,
                            CouponDescription = dis.CouponDescription,
                            BranchName = branch.BranchName,
                            CouponId = dis.CouponId,
                            CouponName = dis.CouponName,
                            CreatedByGuid = dis.FK_CreatedByGuid ?? Guid.Empty,
                            DateCreated = dis.DateCreated,
                            DiscountLimitationType = dis.DiscountLimitation,
                            DiscountPriceType = dis.CouponPriceType,
                            DiscountValue = dis.DiscountValue,
                            DiscountType = dis.DiscountType,
                            ProductIdsString = dis.ProductIds,
                            EndDate = dis.EndDate,
                            IsActive = dis.IsActive,
                            IsDeleted = dis.IsDeleted,
                            StartDate = dis.StartDate
                        }).FirstOrDefault();
            if (data != null && !string.IsNullOrEmpty(data.ProductIdsString))
            {
                data.ProductIds = data.ProductIdsString.Split(',');
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchGuid"></param>
        /// <returns></returns>
        public List<CustomSelectListItem> GetProductListByBranchGuid(Guid branchGuid)
        {
            return bringlyEntities.tblItems
                .Where(item => item.IsActive == true && item.IsDeleted == false && item.FK_BranchGuid == branchGuid)
                .Select(item => new CustomSelectListItem
                {
                    Text = item.ItemName,
                    Value = item.ItemId.ToString()
                }).ToList();
        }
    }
}
