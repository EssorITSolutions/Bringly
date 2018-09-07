using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Enums;
using Bringly.Domain.Enums.User;
using Bringly.Domain.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Bringly.DomainLogic
{
    public class OrderDomainLogic : BaseClass.DomainLogicBase
    {
        private string _orderPrefixName { get; set; }

        public OrderDomainLogic()
        {
            _orderPrefixName = ConfigurationManager.AppSettings["orderPrefixName"];
        }

        /// <summary>
        /// get all the orders for the given buyer guid
        /// </summary>
        /// <param name="buyerGuid">The buyer guid for which want to get orders</param>
        /// <param name="sortByColumn">Optional, column name by which order will be sorted</param>
        /// <param name="sortOrder">Sort order(asc, desc)</param>
        /// <param name="sortingOrderStatus">Order status for which sorting will be done</param>
        /// <returns>Return list of orders for given buyer guid</returns>
        public MyOrder GetBuyerOrders(Guid buyerGuid, OrderStatus? sortingOrderStatus, string sortByColumn = "orderdate", string sortOrder = "desc", string activeTab = "totalOrders", int LatestPage = 0)
        {
            MyOrder myOrder = new MyOrder();

            myOrder.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage;

            var orderData = (from ordr in bringlyEntities.tblOrders
                             where ordr.IsDeleted == false && ordr.FK_CreatedByGuid == buyerGuid
                             join ordrItem in bringlyEntities.tblOrderItems
                             on ordr.OrderGuid equals ordrItem.FK_OrderGuid
                             select new Order
                             {
                                 BusinessGuid = ordr.tblBusiness.BusinessGuid,
                                 BusinessName = ordr.tblBranch.BranchName,
                                 BusinessImage = ordr.tblBranch.BranchImage,

                                 OrderDate = ordr.DateCreated,
                                 OrderGuid = ordr.OrderGuid,

                                 OrderItems = (from oI in bringlyEntities.tblOrderItems
                                               join i in bringlyEntities.tblItems
                                               on oI.FK_ItemGuid equals i.ItemGuid
                                               where oI.FK_OrderGuid == ordr.OrderGuid
                                               select new Domain.OrderItem
                                               {
                                                   ItemGuid = i.ItemGuid,
                                                   OrderGuid = ordrItem.FK_OrderGuid,
                                                   ItemName = i.ItemName,
                                                   OrderItemGuid = ordrItem.OrderItemGuid,
                                                   Quantity = oI.Quantity,
                                                   ItemPrice = ordrItem.UnitPrice,
                                                   DeliveryCharge = i.DeliveryCharge ?? 0

                                               }),

                                 SubTotal = (from oI in bringlyEntities.tblOrderItems
                                             where oI.FK_OrderGuid == ordr.OrderGuid
                                             select (oI.Quantity * oI.UnitPrice)).Sum(),
                                 Total = ordr.OrderTotal,
                                 OrderNumber = _orderPrefixName + ordr.OrderID.ToString(),
                                 OrderStatusId = ordr.FK_OrderStatusId ?? 0,
                             });

            if (sortingOrderStatus == null)
                orderData = orderData.OrderBy(x => x.OrderDate);

            if (string.IsNullOrEmpty(sortByColumn))
            {
                sortByColumn = "orderdate";
                sortOrder = "desc";
            }

            var data = orderData.DistinctBy(x => x.OrderDate).ToList();



            myOrder.CompletedOrders = data.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).ToList();
            myOrder.CancelledOrders = data.Where(x => (int)OrderStatus.Cancelled == (x.OrderStatusId)).ToList();
            myOrder.PendingOrders = data.Where(x => (int)OrderStatus.Pending == (x.OrderStatusId)).ToList();

            myOrder.PendingOrderCount = myOrder.PendingOrders == null ? 0 : myOrder.PendingOrders.Count;
            myOrder.CancelledOrderCount = myOrder.CancelledOrders == null ? 0 : myOrder.CancelledOrders.Count;
            myOrder.CompletedOrderCount = myOrder.CompletedOrders == null ? 0 : myOrder.CompletedOrders.Count;


            switch (sortingOrderStatus)
            {
                case OrderStatus.Pending:
                    myOrder.PendingOrders = SortMyOrderWithStatus(myOrder.PendingOrders, sortByColumn, sortOrder).ToList();
                    break;

                case OrderStatus.Completed:
                    myOrder.CompletedOrders = SortMyOrderWithStatus(myOrder.CompletedOrders, sortByColumn, sortOrder).ToList();
                    break;

                case OrderStatus.Cancelled:
                    myOrder.CancelledOrders = SortMyOrderWithStatus(myOrder.CancelledOrders, sortByColumn, sortOrder).ToList();
                    break;
            }

            if (string.IsNullOrEmpty(activeTab) || activeTab.Equals("totalorders", StringComparison.OrdinalIgnoreCase))
            {
                myOrder.PageSize = PageSize;
                int skip = 0;
                int take = myOrder.PageSize;
                if (myOrder.CurrentPage == 1)
                    skip = 0;
                else
                    skip = ((myOrder.CurrentPage * myOrder.PageSize) - myOrder.PageSize);

                myOrder.CompletedOrders = myOrder.CompletedOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).Skip(skip).Take(take).ToList();
                myOrder.CancelledOrders = myOrder.CancelledOrders.Where(x => (int)OrderStatus.Cancelled == (x.OrderStatusId)).Skip(skip).Take(take).ToList();
                myOrder.PendingOrders = myOrder.PendingOrders.Where(x => (int)OrderStatus.Pending == (x.OrderStatusId)).Skip(skip).Take(take).ToList();
            }
            else
            {
                myOrder.PageSize = 10;
                int skip = 0;
                int take = myOrder.PageSize;
                if (myOrder.CurrentPage == 1)
                    skip = 0;
                else
                    skip = ((myOrder.CurrentPage * myOrder.PageSize) - myOrder.PageSize);

                activeTab = activeTab.ToLower();
                switch (activeTab)
                {
                    case "pending":
                        myOrder.PendingOrders = myOrder.PendingOrders.Where(x => (int)OrderStatus.Pending == (x.OrderStatusId)).Skip(skip).Take(take).ToList();
                        break;

                    case "completed":
                        myOrder.CompletedOrders = myOrder.CompletedOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).Skip(skip).Take(take).ToList();
                        break;

                    case "cancelled":
                        myOrder.CancelledOrders = myOrder.CancelledOrders.Where(x => (int)OrderStatus.Cancelled == (x.OrderStatusId)).Skip(skip).Take(take).ToList();
                        break;

                    default:

                        myOrder.CompletedOrders = myOrder.CompletedOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).Skip(skip).Take(take).ToList();
                        myOrder.CancelledOrders = myOrder.CancelledOrders.Where(x => (int)OrderStatus.Cancelled == (x.OrderStatusId)).Skip(skip).Take(take).ToList();
                        myOrder.PendingOrders = myOrder.PendingOrders.Where(x => (int)OrderStatus.Pending == (x.OrderStatusId)).Skip(skip).Take(take).ToList();

                        break;
                }
            }

            myOrder.SortByColumn = sortByColumn;
            myOrder.SortOrder = sortOrder == "asc" ? "desc" : "asc";
            myOrder.SortingOrderStatus = sortingOrderStatus;

            return myOrder;
        }

        /// <summary>
        /// Get the order details for given order guid
        /// </summary>
        /// <param name="orderGuid">Guid to which want order details</param>
        /// <returns>Order details for the given order guid</returns>
        public Order GetOrderDetails(Guid orderGuid)
        {
            var orderData = (from ordr in bringlyEntities.tblOrders
                             where ordr.IsDeleted == false && ordr.OrderGuid == orderGuid
                             && ordr.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                             join ordrItem in bringlyEntities.tblOrderItems
                             on ordr.OrderGuid equals ordrItem.FK_OrderGuid
                             select new Order
                             {
                                 BusinessGuid = ordr.tblBusiness.BusinessGuid,
                                 BusinessName = ordr.tblBusiness.BusinessName,
                                 BusinessImage = ordr.tblBusiness.BusinessImage,
                                 OrderDate = ordr.DateCreated,
                                 OrderGuid = ordr.OrderGuid,

                                 OrderItems = (from oI in bringlyEntities.tblOrderItems
                                               join i in bringlyEntities.tblItems
                                               on oI.FK_ItemGuid equals i.ItemGuid
                                               where oI.FK_OrderGuid == ordr.OrderGuid
                                               select new Domain.OrderItem
                                               {
                                                   ItemGuid = i.ItemGuid,
                                                   OrderGuid = oI.FK_OrderGuid,
                                                   ItemName = i.ItemName,
                                                   OrderItemGuid = oI.OrderItemGuid,
                                                   Quantity = oI.Quantity,
                                                   ItemPrice = oI.UnitPrice,
                                                   BusinessGuid = i.FK_BusinessGuid,
                                                   CategoryGuid = i.FK_CategoryGuid,
                                                   Discount = i.Discount,
                                                   ItemImage = i.ItemImage,
                                                   ItemSize = i.ItemSize,
                                                   ItemWeight = i.ItemWeight,
                                                   DeliveryCharge = i.DeliveryCharge ?? 0
                                               }),

                                 SubTotal = (from oI in bringlyEntities.tblOrderItems
                                             where oI.FK_OrderGuid == ordr.OrderGuid
                                             select (oI.Quantity * oI.UnitPrice)).Sum(),

                                 Total = ordr.OrderTotal,
                                 OrderNumber = _orderPrefixName + ordr.OrderID.ToString(),
                                 OrderStatusId = ordr.FK_OrderStatusId ?? 0,
                                 PaymentMethod = ordr.PaymentType == (int)PaymentType.CreditCard ? "Credit Card" : "Cash",
                             }).FirstOrDefault();

            orderData.UserProfile = (from usr in bringlyEntities.tblUsers
                                     where usr.IsActive == true && usr.IsDeleted == false
                                     && usr.UserGuid == UserVariables.LoggedInUserGuid
                                     select new UserRegistration
                                     {
                                         BillingAddress = usr.tblUserAddresses.Where(x => x.IsDeleted == false
                                         && x.AddressType == Bringly.Domain.Enums.User.UserAddressType.Billing.ToString())
                                         .Select(x => new UserAddress
                                         {
                                             Address = x.Address,
                                             AddressType = x.AddressType,
                                             CityGuid = x.FK_CityGuid,
                                             Country = x.Country,
                                             CountryGuid = x.CountryGuid ?? Guid.Empty,
                                             CityName = x.tblCity.CityName,
                                             Latitude = x.Latitude,
                                             Longitude = x.Longitude,
                                             PlaceId = x.PlaceId,
                                             PostCode = x.PostCode,
                                             UserAddressGuid = x.UserAddressGuid,
                                             UserGuid = x.FK_UserGuid
                                         }).FirstOrDefault(),

                                         ShippingAddress = usr.tblUserAddresses.Where(x => x.IsDeleted == false
                                         && x.AddressType == Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString())
                                         .Select(x => new UserAddress
                                         {
                                             Address = x.Address,
                                             AddressType = x.AddressType,
                                             CityGuid = x.FK_CityGuid,
                                             Country = x.Country,
                                             CountryGuid = x.CountryGuid ?? Guid.Empty,
                                             CityName = x.tblCity.CityName,
                                             Latitude = x.Latitude,
                                             Longitude = x.Longitude,
                                             PlaceId = x.PlaceId,
                                             PostCode = x.PostCode,
                                             UserAddressGuid = x.UserAddressGuid,
                                             UserGuid = x.FK_UserGuid
                                         }).FirstOrDefault(),

                                         EmailAddress = usr.EmailAddress,
                                         FullName = usr.FullName,
                                         MobileNumber = usr.MobileNumber,
                                         ProfileImage = usr.ImageName,
                                     }).FirstOrDefault();

            return orderData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public MyOrder GetMyOrderCounts(Guid userGuid, UserRoles userRoles)
        {
            MyOrder myOrder = new MyOrder();

            if (userRoles == UserRoles.Buyer)
            {
                var orderData = (from odr in bringlyEntities.tblOrders
                                 where odr.FK_CreatedByGuid == userGuid && odr.IsDeleted == false
                                 select odr);

                var data = orderData.DistinctBy(x => x.OrderGuid).ToList();

                myOrder.CompletedOrderCount = data.Where(x => (int)OrderStatus.Completed == x.FK_OrderStatusId).Count();
                myOrder.CancelledOrderCount = data.Where(x => (int)OrderStatus.Cancelled == x.FK_OrderStatusId).Count();
                myOrder.PendingOrderCount = data.Where(x => (int)OrderStatus.Pending == x.FK_OrderStatusId).Count();
                myOrder.TotalOrderCount = data.Count();
            }

            else if (userRoles == UserRoles.Merchant)
            {
                var orderData = (from odr in bringlyEntities.tblOrders
                                 where odr.IsDeleted == false
                                 where odr.tblBusiness.FK_CreatedByGuid == userGuid
                                 select odr);

                var data = orderData.DistinctBy(x => x.OrderGuid).ToList();

                myOrder.CompletedOrderCount = data.Where(x => (int)OrderStatus.Completed == x.FK_OrderStatusId).Count();
                myOrder.CancelledOrderCount = data.Where(x => (int)OrderStatus.Cancelled == x.FK_OrderStatusId).Count();
                myOrder.PendingOrderCount = data.Where(x => (int)OrderStatus.Pending == x.FK_OrderStatusId).Count();
                myOrder.RejectedOrderCount = data.Where(x => (int)OrderStatus.Rejected == x.FK_OrderStatusId).Count();
                myOrder.TotalOrderCount = data.Count();
            }

            return myOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="requestQuery"></param>
        /// <returns></returns>
        public MyOrder GetMerchantOrders(Guid merchantGuid, RequestQuery requestQuery, string activeTab)
        {
            MyOrder myOrder = new MyOrder();

            var orderData = (from odr in bringlyEntities.tblOrders
                             join busi in bringlyEntities.tblBusinesses
                             on odr.FK_BusinessGuid equals busi.BusinessGuid
                             where odr.IsDeleted == false && busi.FK_CreatedByGuid == merchantGuid
                             select new Order
                             {
                                 BusinessGuid = busi.BusinessGuid,
                                 BusinessName = odr.tblBusiness.BusinessName,
                                 BusinessImage = odr.tblBusiness.BusinessImage,
                                 Discount = odr.OrderDiscount,
                                 OrderGuid = odr.OrderGuid,
                                 OrderDate = odr.DateCreated,
                                 OrderItems = (from oI in bringlyEntities.tblOrderItems
                                               join i in bringlyEntities.tblItems
                                               on oI.FK_ItemGuid equals i.ItemGuid
                                               where oI.FK_OrderGuid == odr.OrderGuid
                                               select new Domain.OrderItem
                                               {
                                                   ItemGuid = i.ItemGuid,
                                                   OrderGuid = oI.FK_OrderGuid,
                                                   ItemName = i.ItemName,
                                                   OrderItemGuid = oI.OrderItemGuid,
                                                   Quantity = oI.Quantity,
                                                   ItemPrice = i.ItemPrice,
                                                   DeliveryCharge = i.DeliveryCharge ?? 0
                                               }),

                                 SubTotal = (from oI in bringlyEntities.tblOrderItems
                                             where oI.FK_OrderGuid == odr.OrderGuid
                                             select (oI.Quantity * oI.UnitPrice)).Sum(),
                                 Total = odr.OrderTotal,
                                 OrderNumber = _orderPrefixName + odr.OrderID.ToString(),
                                 OrderStatusId = odr.FK_OrderStatusId ?? 0,
                                 OrderStatus = odr.tblOrderStatu.OrderStatusDisplayName,
                                 PaymentMethod = odr.PaymentType == (int)PaymentType.CreditCard ? "Credit Card" : "Cash",
                                 OrderType = odr.OrderType == (int)OrderType.SelfDelivery ? "Self Delivery" : "Pick Up",

                                 DeliveryBoy = (from usr in bringlyEntities.tblUsers
                                                where usr.UserGuid == odr.DeliveryBoyUserGuid
                                                select new DeliveryBoy
                                                {
                                                    DeliveryBoyContactNumber = usr.MobileNumber,
                                                    DeliveryBoyGuid = usr.UserGuid,
                                                    DeliveryBoyName = usr.FullName
                                                }).FirstOrDefault(),

                                 UserProfile = new UserRegistration
                                 {
                                     EmailAddress = odr.tblUser.EmailAddress,
                                     FullName = odr.tblUser.FullName,
                                     MobileNumber = odr.tblUser.MobileNumber,
                                     ProfileImage = odr.tblUser.ImageName,
                                     UserGuid = odr.tblUser.UserGuid,
                                     BillingAddress = odr.tblUser.tblUserAddresses.Where(x => x.FK_UserGuid == odr.tblUser.UserGuid).Select(x => new UserAddress
                                     {
                                         Address = x.Address,
                                         CityGuid = x.FK_CityGuid,
                                         Country = x.Country,
                                         Latitude = x.Latitude,
                                         Longitude = x.Longitude,
                                         PlaceId = x.PlaceId,
                                         PostCode = x.PostCode,
                                         UserAddressGuid = x.UserAddressGuid
                                     }).FirstOrDefault()

                                 }
                             });

            myOrder.TotalOrders = orderData.DistinctBy(x => x.OrderDate).ToList();
            myOrder.TotalOrderCount = myOrder.TotalOrders == null ? 0 : myOrder.TotalOrders.Count;

            myOrder.CompletedOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).ToList();
            myOrder.CancelledOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Cancelled == (x.OrderStatusId)).ToList();
            myOrder.PendingOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Pending == (x.OrderStatusId)).ToList();

            myOrder.PendingOrderCount = myOrder.PendingOrders == null ? 0 : myOrder.PendingOrders.Count;
            myOrder.CancelledOrderCount = myOrder.CancelledOrders == null ? 0 : myOrder.CancelledOrders.Count;
            myOrder.CompletedOrderCount = myOrder.CompletedOrders == null ? 0 : myOrder.CompletedOrders.Count;
            myOrder.RejectedOrderCount = myOrder.RejectedOrders == null ? 0 : myOrder.RejectedOrders.Count;

            switch (activeTab)
            {
                case "pending":
                    myOrder.TotalOrderCount = myOrder.PendingOrders.Count;
                    myOrder.PendingOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Pending == x.OrderStatusId).Skip((requestQuery.CurrentPage - 1) * requestQuery.PageSize).Take(requestQuery.PageSize).ToList();
                    break;

                case "completed":
                    myOrder.TotalOrderCount = myOrder.CompletedOrders.Count;
                    myOrder.CompletedOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).Skip((requestQuery.CurrentPage - 1) * requestQuery.PageSize).Take(requestQuery.PageSize).ToList();
                    break;

                case "cancelled":
                    myOrder.TotalOrderCount = myOrder.CancelledOrders.Count;
                    myOrder.CancelledOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Cancelled == x.OrderStatusId).Skip((requestQuery.CurrentPage - 1) * requestQuery.PageSize).Take(requestQuery.PageSize).ToList();
                    break;
                default:
                    myOrder.TotalOrderCount = myOrder.PendingOrders.Count;
                    myOrder.PendingOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Pending == x.OrderStatusId).Skip((requestQuery.CurrentPage - 1) * requestQuery.PageSize).Take(requestQuery.PageSize).ToList();
                    break;
            }

            //myOrder.RejectedOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Rejected == x.OrderStatusId).Skip((requestQuery.CurrentPage - 1) * requestQuery.PageSize).Take(requestQuery.PageSize).ToList();
            return myOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessGuid"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public MyOrder GetBusinessOrders(Guid BusinessGuid, OrderStatus? orderStatus = null)
        {
            MyOrder myOrder = new MyOrder();
            var orderData = (from odr in bringlyEntities.tblOrders
                             where odr.IsDeleted == false && odr.FK_BusinessGuid == BusinessGuid
                             select new Order
                             {
                                 BusinessGuid = BusinessGuid,
                                 BusinessName = odr.tblBusiness.BusinessName,
                                 BusinessImage = odr.tblBusiness.BusinessImage,
                                 Discount = odr.OrderDiscount,
                                 OrderDate = odr.DateCreated,
                                 OrderItems = (from oI in bringlyEntities.tblOrderItems
                                               join i in bringlyEntities.tblItems
                                               on oI.FK_ItemGuid equals i.ItemGuid
                                               where oI.FK_OrderGuid == odr.OrderGuid
                                               select new Domain.OrderItem
                                               {
                                                   ItemGuid = i.ItemGuid,
                                                   OrderGuid = oI.FK_OrderGuid,
                                                   ItemName = i.ItemName,
                                                   OrderItemGuid = oI.OrderItemGuid,
                                                   Quantity = oI.Quantity,
                                                   ItemPrice = i.ItemPrice,
                                                   DeliveryCharge = i.DeliveryCharge ?? 0
                                               }),

                                 SubTotal = (from oI in bringlyEntities.tblOrderItems
                                             where oI.FK_OrderGuid == odr.OrderGuid
                                             select (oI.Quantity * oI.UnitPrice)).Sum(),

                                 Total = odr.OrderTotal,
                                 OrderNumber = _orderPrefixName + odr.OrderID.ToString(),
                                 OrderStatusId = odr.FK_OrderStatusId ?? 0,

                                 UserProfile = new UserRegistration
                                 {
                                     EmailAddress = odr.tblUser.EmailAddress,
                                     FullName = odr.tblUser.FullName,
                                     MobileNumber = odr.tblUser.MobileNumber,
                                     ProfileImage = odr.tblUser.ImageName,
                                     UserGuid = odr.tblUser.UserGuid,
                                 }
                             });

            myOrder.TotalOrders = orderData.DistinctBy(x => x.OrderDate).ToList();

            myOrder.CompletedOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Completed == x.OrderStatusId).ToList();
            myOrder.CancelledOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Cancelled == x.OrderStatusId).ToList();
            myOrder.PendingOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Pending == x.OrderStatusId).ToList();
            myOrder.RejectedOrders = myOrder.TotalOrders.Where(x => (int)OrderStatus.Rejected == x.OrderStatusId).ToList();

            myOrder.PendingOrderCount = myOrder.PendingOrders == null ? 0 : myOrder.PendingOrders.Count;
            myOrder.CancelledOrderCount = myOrder.CancelledOrders == null ? 0 : myOrder.CancelledOrders.Count;
            myOrder.CompletedOrderCount = myOrder.CompletedOrders == null ? 0 : myOrder.CompletedOrders.Count;
            myOrder.RejectedOrderCount = myOrder.RejectedOrders == null ? 0 : myOrder.RejectedOrders.Count;

            return myOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(Guid orderGuid, OrderStatus orderStatus)
        {
            var order = bringlyEntities.tblOrders.Where(odr => odr.IsDeleted == false && odr.OrderGuid == orderGuid).FirstOrDefault();
            if (order == null)
                return false;

            order.FK_OrderStatusId = (int)orderStatus;
            bringlyEntities.SaveChanges();
            return true;
        }

        public DeliveryOption GetDeliveryOption(Guid OrderGuid)
        {
            var deliveryOptionData = (from user in bringlyEntities.tblUsers
                                      join odr in bringlyEntities.tblOrders
                                      on user.UserGuid equals odr.FK_CreatedByGuid
                                      where user.IsDeleted == false && user.IsActive == true && odr.OrderGuid == OrderGuid
                                      select new DeliveryOption
                                      {
                                          UserGuid = user.UserGuid,
                                          UserFullName = user.FullName,
                                          EmailAddress = user.EmailAddress,
                                          MobileNumber = user.MobileNumber,
                                          OrderGuid = OrderGuid,
                                          BillingAddress = user.tblUserAddresses.Where(z => z.IsDeleted == false && z.AddressType.Equals("Billing", StringComparison.OrdinalIgnoreCase)).Select(z => new UserAddress
                                          {
                                              Address = z.Address,
                                              AddressType = z.AddressType,
                                              CountryGuid = z.CountryGuid,
                                              CityGuid = z.FK_CityGuid,
                                              CityName = z.tblCity.CityName,
                                              Country = z.Country,
                                              Latitude = z.Latitude,
                                              Longitude = z.Longitude,
                                              PlaceId = z.PlaceId,
                                              PostCode = z.PostCode,
                                              UserGuid = z.FK_UserGuid
                                          }).FirstOrDefault(),
                                          ShippingAddress = user.tblUserAddresses.Where(z => z.IsDeleted == false && z.AddressType.Equals("Shipping", StringComparison.OrdinalIgnoreCase)).Select(z => new UserAddress
                                          {
                                              Address = z.Address,
                                              AddressType = z.AddressType,
                                              CountryGuid = z.CountryGuid,
                                              CityGuid = z.FK_CityGuid,
                                              CityName = z.tblCity.CityName,
                                              Country = z.Country,
                                              Latitude = z.Latitude,
                                              Longitude = z.Longitude,
                                              PlaceId = z.PlaceId,
                                              PostCode = z.PostCode,
                                              UserGuid = z.FK_UserGuid
                                          }).FirstOrDefault(),
                                          DeliveryType = (OrderType)odr.OrderType
                                      }).FirstOrDefault();

            CommonDomainLogic commonDomainLogic = new CommonDomainLogic();
            deliveryOptionData.CityList = commonDomainLogic.GetCities();
            deliveryOptionData.CountryList = commonDomainLogic.GetCountryList();

            return deliveryOptionData;
        }

        public PaymentType GetPaymentType(Guid orderGuid)
        {
            var order = bringlyEntities.tblOrders
                .Where(o => o.IsDeleted == false && o.OrderGuid == orderGuid)
                .FirstOrDefault();
            if (order == null)
                return (PaymentType)5;
            return (PaymentType)order.PaymentType;
        }

        public bool SaveDeliveryOption(DeliveryOption deliveryOption)
        {
            var oderObj = bringlyEntities.tblOrders
                .Where(odr => odr.IsDeleted == false && odr.OrderGuid == deliveryOption.OrderGuid).FirstOrDefault();

            if (oderObj == null)
                return false;
            oderObj.OrderType = (short)deliveryOption.DeliveryType;

            var orderAddress = bringlyEntities.tblOrderAddresses
                .Where(z => z.IsDeleted == false && z.FK_OrderGuid == deliveryOption.OrderGuid).FirstOrDefault();

            if (orderAddress != null)
            {
                orderAddress.Address = deliveryOption.ShippingAddress.Address;
                orderAddress.Country = deliveryOption.ShippingAddress.Country;
                orderAddress.CountryGuid = deliveryOption.ShippingAddress.CountryGuid;
                orderAddress.FK_CityGuid = deliveryOption.ShippingAddress.CityGuid;
                orderAddress.Latitude = deliveryOption.ShippingAddress.Latitude;
                orderAddress.Longitude = deliveryOption.ShippingAddress.Longitude;
                orderAddress.MobileNumber = deliveryOption.MobileNumber;
                orderAddress.PlaceId = deliveryOption.ShippingAddress.PlaceId;
                orderAddress.PostCode = deliveryOption.ShippingAddress.PostCode;
            }
            else
            {
                orderAddress = new tblOrderAddress();
                orderAddress.OrderAddressGuid = Guid.NewGuid();
                orderAddress.Address = deliveryOption.ShippingAddress.Address;
                orderAddress.Country = deliveryOption.ShippingAddress.Country;
                orderAddress.CountryGuid = deliveryOption.ShippingAddress.CountryGuid;
                orderAddress.DateCreated = DateTime.Now;
                orderAddress.FK_CityGuid = deliveryOption.ShippingAddress.CityGuid;
                orderAddress.FK_OrderGuid = deliveryOption.OrderGuid;
                orderAddress.FK_UserGuid = deliveryOption.UserGuid;
                orderAddress.Latitude = deliveryOption.ShippingAddress.Latitude;
                orderAddress.Longitude = deliveryOption.ShippingAddress.Longitude;
                orderAddress.MobileNumber = deliveryOption.MobileNumber;
                orderAddress.PlaceId = deliveryOption.ShippingAddress.PlaceId;
                orderAddress.PostCode = deliveryOption.ShippingAddress.PostCode;
                bringlyEntities.tblOrderAddresses.Add(orderAddress);
            }

            var shippingAddress = bringlyEntities.tblUserAddresses
                .Where(z => z.IsDeleted == false && z.AddressType.Equals("Shipping", StringComparison.OrdinalIgnoreCase)
                && z.FK_UserGuid == deliveryOption.UserGuid).FirstOrDefault();

            if (shippingAddress != null)
            {
                shippingAddress.Address = deliveryOption.ShippingAddress.Address;
                shippingAddress.Country = deliveryOption.ShippingAddress.Country;
                shippingAddress.CountryGuid = deliveryOption.ShippingAddress.CountryGuid;
                shippingAddress.FK_CityGuid = deliveryOption.ShippingAddress.CityGuid;
                shippingAddress.FK_UserGuid = deliveryOption.UserGuid;
                shippingAddress.PostCode = deliveryOption.ShippingAddress.PostCode;
            }

            else
            {
                shippingAddress = new tblUserAddress();
                shippingAddress.UserAddressGuid = Guid.NewGuid();
                shippingAddress.AddressType = "Shipping";
                shippingAddress.Address = deliveryOption.ShippingAddress.Address;
                shippingAddress.Country = deliveryOption.ShippingAddress.Country;
                shippingAddress.CountryGuid = deliveryOption.ShippingAddress.CountryGuid;
                shippingAddress.DateCreated = DateTime.Now;
                shippingAddress.FK_CityGuid = deliveryOption.ShippingAddress.CityGuid;
                shippingAddress.FK_UserGuid = deliveryOption.UserGuid;
                shippingAddress.Latitude = deliveryOption.ShippingAddress.Latitude;
                shippingAddress.Longitude = deliveryOption.ShippingAddress.Longitude;
                shippingAddress.PlaceId = deliveryOption.ShippingAddress.PlaceId;
                shippingAddress.PostCode = deliveryOption.ShippingAddress.PostCode;
                shippingAddress.IsDeleted = false;
                bringlyEntities.tblUserAddresses.Add(shippingAddress);
            }

            bringlyEntities.SaveChanges();

            return true;
        }

        public bool UpdateDeliveryOptionPaymentMethod(Guid orderGuid, PaymentType paymentType)
        {
            var oderObj = bringlyEntities.tblOrders
                .Where(odr => odr.IsDeleted == false && odr.OrderGuid == orderGuid)
                .FirstOrDefault();

            if (oderObj == null)
                return false;

            oderObj.PaymentType = (int)paymentType;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// Get the order details for given order guid
        /// </summary>
        /// <param name="orderGuid">Guid to which want order details</param>
        /// <returns>Order details for the given order guid</returns>
        public Order GetOrderConfirmationDetails(Guid orderGuid)
        {
            var orderData = (from ordr in bringlyEntities.tblOrders
                             join ordrItem in bringlyEntities.tblOrderItems
                             on ordr.OrderGuid equals ordrItem.FK_OrderGuid
                             where ordr.IsDeleted == false && ordr.OrderGuid == orderGuid
                            && ordr.FK_CreatedByGuid == UserVariables.LoggedInUserGuid
                             select new Order
                             {
                                 BusinessGuid = ordr.tblBusiness.BusinessGuid,
                                 BusinessName = ordr.tblBusiness.BusinessName,
                                 BusinessImage = ordr.tblBusiness.BusinessImage,
                                 OrderDate = ordr.DateCreated,
                                 OrderGuid = ordr.OrderGuid,

                                 OrderItems = (from oI in bringlyEntities.tblOrderItems
                                               join i in bringlyEntities.tblItems
                                               on oI.FK_ItemGuid equals i.ItemGuid
                                               where oI.FK_OrderGuid == ordr.OrderGuid
                                               select new Domain.OrderItem
                                               {
                                                   ItemGuid = i.ItemGuid,
                                                   OrderGuid = oI.FK_OrderGuid,
                                                   ItemName = i.ItemName,
                                                   OrderItemGuid = oI.OrderItemGuid,
                                                   Quantity = oI.Quantity,
                                                   ItemPrice = oI.UnitPrice,
                                                   BusinessGuid = i.FK_BusinessGuid,
                                                   CategoryGuid = i.FK_CategoryGuid,
                                                   Discount = i.Discount,
                                                   ItemImage = i.ItemImage,
                                                   ItemSize = i.ItemSize,
                                                   ItemWeight = i.ItemWeight,
                                                   DeliveryCharge = i.DeliveryCharge ?? 0
                                               }),

                                 SubTotal = (from oI in bringlyEntities.tblOrderItems
                                             where oI.FK_OrderGuid == ordr.OrderGuid
                                             select (oI.Quantity * oI.UnitPrice)).Sum(),

                                 Total = ordr.OrderTotal,
                                 OrderNumber = _orderPrefixName + ordr.OrderID.ToString(),
                                 OrderStatusId = ordr.FK_OrderStatusId ?? 0,
                                 PaymentMethod = ordr.PaymentType == (int)PaymentType.CreditCard ? "Credit Card" : "Cash",
                             }).FirstOrDefault();

            orderData.UserProfile = (from usr in bringlyEntities.tblUsers
                                     where usr.IsActive == true && usr.IsDeleted == false
                                     && usr.UserGuid == UserVariables.LoggedInUserGuid
                                     join oa in bringlyEntities.tblOrderAddresses
                                     on usr.UserGuid equals oa.FK_UserGuid
                                     select new UserRegistration
                                     {
                                         BillingAddress = usr.tblUserAddresses.Where(x => x.IsDeleted == false
                                         && x.AddressType == Bringly.Domain.Enums.User.UserAddressType.Billing.ToString())
                                         .Select(x => new UserAddress
                                         {
                                             Address = x.Address,
                                             AddressType = x.AddressType,
                                             CityGuid = x.FK_CityGuid,
                                             Country = x.Country,
                                             CountryGuid = x.CountryGuid ?? Guid.Empty,
                                             CityName = x.tblCity.CityName,
                                             Latitude = x.Latitude,
                                             Longitude = x.Longitude,
                                             PlaceId = x.PlaceId,
                                             PostCode = x.PostCode,
                                             UserAddressGuid = x.UserAddressGuid,
                                             UserGuid = x.FK_UserGuid
                                         }).FirstOrDefault(),

                                         ShippingAddress = usr.tblUserAddresses.Where(x => x.IsDeleted == false
                                         && x.AddressType == Bringly.Domain.Enums.User.UserAddressType.Shipping.ToString())
                                         .Select(x => new UserAddress
                                         {
                                             Address = x.Address,
                                             AddressType = x.AddressType,
                                             CityGuid = x.FK_CityGuid,
                                             Country = x.Country,
                                             CountryGuid = x.CountryGuid ?? Guid.Empty,
                                             CityName = x.tblCity.CityName,
                                             Latitude = x.Latitude,
                                             Longitude = x.Longitude,
                                             PlaceId = x.PlaceId,
                                             PostCode = x.PostCode,
                                             UserAddressGuid = x.UserAddressGuid,
                                             UserGuid = x.FK_UserGuid
                                         }).FirstOrDefault(),

                                         EmailAddress = usr.EmailAddress,
                                         FullName = usr.FullName,
                                         MobileNumber = usr.MobileNumber,
                                         ProfileImage = usr.ImageName,
                                     }).FirstOrDefault();

            return orderData;
        }

        /// <summary>
        /// Sorts the buyer order with order status
        /// </summary>
        /// <param name="data">List of oders to be sorted</param>
        /// <param name="sortByColumn">Column names by which order will be sorted</param>
        /// <param name="sortOrder">Sort order(asc or desc)</param>
        /// <returns>IEnumerable list of sorted order</returns>
        private IEnumerable<Order> SortMyOrderWithStatus(List<Order> data, string sortByColumn, string sortOrder)
        {
            IEnumerable<Order> orderData;
            switch (sortByColumn)
            {
                case "business":

                    if (sortOrder.Equals("asc")) orderData = data.OrderBy(x => x.BusinessName);
                    else orderData = data.OrderByDescending(x => x.BusinessName);
                    break;

                case "order":
                    if (sortOrder.Equals("asc")) orderData = data.OrderBy(x => x.OrderNumber);
                    else orderData = data.OrderByDescending(x => x.OrderNumber);
                    break;

                case "totalsum":
                    if (sortOrder.Equals("asc")) orderData = data.OrderBy(x => x.SubTotal);
                    else orderData = data.OrderByDescending(x => x.SubTotal);
                    break;

                case "orderdate":
                    if (sortOrder.Equals("asc")) orderData = data.OrderBy(x => x.OrderDate);
                    else orderData = data.OrderByDescending(x => x.OrderDate);
                    break;

                default:
                    orderData = data.OrderBy(x => x.OrderDate);
                    break;
            }

            return orderData;
        }


    }
}
