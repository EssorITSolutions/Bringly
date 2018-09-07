using Bringly.Domain;
using Bringly.Domain.Business;
using Bringly.DomainLogic;
using Bringly.DomainLogic.Business;
using Bringly.DomainLogic.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bringly.UI.Controllers
{
    public class RestaurantController : BaseClasses.AuthoriseUserControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult City(string id)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            CommonDomainLogic _commonDomainLogic = new CommonDomainLogic();
            ChooseCity chooseCity = new ChooseCity();
            if (!string.IsNullOrEmpty(id))
            {
                Guid _cityguid = _commonDomainLogic.FindCityGuid(id);
                UserDomainLogic _userDomainLogic = new UserDomainLogic();
                _userDomainLogic.UpdatePreferedCity(_cityguid);
                chooseCity.Cities = _commonDomainLogic.GetCityByGUID(_cityguid);
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }
            else
            {
                chooseCity.Cities = _commonDomainLogic.GetCities();
                chooseCity.SelectedCity = new City { CityName = chooseCity.Cities.FirstOrDefault().CityName, CityGuid = chooseCity.Cities.FirstOrDefault().CityGuid, CityUrlName = chooseCity.Cities.FirstOrDefault().CityUrlName };
            }

            return View("ListRestaurants", restaurantDomainLogic.GetRestaurantsByCity(chooseCity.SelectedCity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        public ActionResult MyRestaurant(Restaurant restaurant)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            BusinessObject restaurantSearch = new BusinessObject();
            restaurantSearch = restaurantDomainLogic.GetRestaurantByRestaurantGuid(new Guid("C37FB6CE-E2F5-4893-BB92-21136A3E5756"));
            return View(restaurantSearch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RestaurantGuid"></param>
        /// <returns></returns>
        public ActionResult RestaurantItems(Nullable<Guid> RestaurantGuid)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            RestaurantGuid = new Guid("7B9D0CFF-F15F-49CA-95EF-EA81CDEA4E34");
            List<Items> itemslist = new List<Items>();
            itemslist = restaurantDomainLogic.GetItemsByRestaurantGuid(RestaurantGuid.HasValue ? RestaurantGuid.Value : Guid.Empty);
            return View(itemslist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addToCart(Items item)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.addToCart(item), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult getCartCount()
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.getCartCount(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult deleteItemFromCart(Guid ItemGuid)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return Json(shoppingCartDomainLogic.deleteItemFromCart(ItemGuid), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantMenuItems()
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            Nullable<Guid> RestaurantGuid = new Guid("7B9D0CFF-F15F-49CA-95EF-EA81CDEA4E34");
            List<Items> itemslist = new List<Items>();
            itemslist = restaurantDomainLogic.GetMerchantItems(RestaurantGuid.HasValue ? RestaurantGuid.Value : Guid.Empty);
            return View(itemslist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemGuid"></param>
        /// <returns></returns>
        public ActionResult AddItem(Nullable<Guid> ItemGuid)
        {
            ViewBag.PopupTitle = "Item Detail";
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            return PartialView("_AddItem", restaurantDomainLogic.GetMenuItem(ItemGuid));
        }

        [HttpPost]
        public ActionResult AddEditItem(Items Item)
        {
            RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
            if (ModelState.IsValid)
            {
                if (restaurantDomainLogic.AddEditItem(Item))
                {
                    TempData["IspopUp"] = true;
                }
            }

            ViewBag.PopupTitle = "Item Detail";
            return PartialView("_AddItem", restaurantDomainLogic.GetMenuItem(Item.ItemGuid));
        }

        public ActionResult UploadMenuItemImage(FormCollection frm)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string imageName = "";
                    if (Request.Files.Count > 0)
                    {
                        RestaurantDomainLogic restaurantDomainLogic = new RestaurantDomainLogic();
                        imageName = restaurantDomainLogic.UploadMenuItemImage(Request);
                    }
                    return Json(new { NewImage = imageName, Message = "File uploaded successfully", IsSuccess = true });
                }
                catch (Exception ex)
                {
                    return Json(new { NewImage = "", Message = "Error occurred. Error details: " + ex.Message, IsSuccess = false });
                }
            }
            else
            {
                return Json(new { NewImage = "", Message = "No image selected.", IsSuccess = false });
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRestaurentProfile(BusinessObject BusinessObject)
        {
            RestaurantDomainLogic restaurantDomailLogic = new RestaurantDomainLogic();
            return Json(restaurantDomailLogic.UpdateRestaurantProfile(BusinessObject), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get add new discount coupon html.
        /// </summary>
        /// <returns>Html view to add new discount coupon.</returns>
        [HttpGet]
        [ActionName("adddiscount")]
        public ActionResult AddDiscountCoupon()
        {
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            var model = new Discount();
            model.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);
            var branch = model.BranchList.Count > 0 ? model.BranchList[0] : null;
            if (branch != null)
            {
                Guid branchGuid = Guid.Empty;
                Guid.TryParse(branch.Value, out branchGuid);
                model.ProductList = GetProductListByBranchGuid(branchGuid);
                model.ProductList = model.ProductList ?? new List<CustomSelectListItem>();
            }

            return View("AddDiscountCoupon", model);
        }

        /// <summary>
        /// Add new discount coupon
        /// </summary>
        /// <param name="discount">discount objet</param>
        /// <returns>Redirect to get all coupons.</returns>
        [HttpPost]
        [ActionName("adddiscount")]
        public ActionResult AddDiscountCoupon(Discount discount)
        {
            if (!ModelState.IsValid)
            {
                return View("AddDiscountCoupon", discount);
            }

            if (discount.DiscountType == 2 && (discount.ProductIds == null || discount.ProductIds.Length < 1))
            {
                    ModelState.AddModelError("ProductIds", "Select at least one product.");
                    BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
                    discount.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);
                    if (discount.BranchGuid != null && discount.BranchGuid != Guid.Empty)
                    {
                        discount.ProductList = GetProductListByBranchGuid(discount.BranchGuid);
                        discount.ProductList = discount.ProductList ?? new List<CustomSelectListItem>();
                    }
                    return View("AddDiscountCoupon", discount);
            }

            if (discount.DiscountPriceType == 1 && discount.DiscountValue > 100)
            {
                ModelState.AddModelError("DiscountValue", "Percentage Value cann't be greater than 100%.");
                BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
                discount.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);
                if (discount.BranchGuid != null && discount.BranchGuid != Guid.Empty)
                {
                    discount.ProductList = GetProductListByBranchGuid(discount.BranchGuid);
                    discount.ProductList = discount.ProductList ?? new List<CustomSelectListItem>();
                }
                return View("AddDiscountCoupon", discount);
            }

            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            var result = shoppingCartDomainLogic.AddDiscount(discount);
            var message = result ? "Coupon has been added" : "Operation failled.";
            TempData["hasError"] = !result;
            TempData["message"] = message;
            return RedirectToAction("coupons"); 
        }

        /// <summary>
        /// Delete discount coupon
        /// </summary>
        /// <param name="couponId">discount coupon id to be deleted. Integer type.</param>
        /// <returns>Redirect to get all coupons</returns>
        [HttpPost]
        [ActionName("deletecoupon")]
        public ActionResult DeleteDiscountCoupon(int couponId)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            var resault = shoppingCartDomainLogic.DeleteDiscountCoupon(couponId);
            var message = resault ? "Coupon has been deleted." : "Operation failled.";
            TempData["hasError"] = !resault;
            TempData["message"] = message;
            return RedirectToAction("coupons");
        }

        /// <summary>
        /// Edit discount coupon
        /// </summary>
        /// <param name="couponId">coupon id to be edited. Integer type.</param>
        /// <returns>Html view to edit discount coupon.</returns>
        [HttpGet]
        [ActionName("editcoupon")]
        public ActionResult EditCoupon(int couponId)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
            var model = shoppingCartDomainLogic.GetDiscountCoupon(couponId);

            model.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);

            if (model.BranchGuid != null && model.BranchList.Count > 0)
            {
                var selectedBranch = model.BranchList.Where(x => x.Value == model.BranchGuid.ToString()).Select(b => b).FirstOrDefault();
                if (selectedBranch != null)
                {
                    Guid branchGuid = Guid.Empty;
                    Guid.TryParse(selectedBranch.Value, out branchGuid);
                    model.ProductList = GetProductListByBranchGuid(branchGuid);
                    model.ProductList = model.ProductList ?? new List<CustomSelectListItem>();
                }
            }
            return View("EditCoupon", model);
        }

        /// <summary>
        /// Update discount coupon
        /// </summary>
        /// <param name="discount">Discount updated model data</param>
        /// <returns>Redirect to get all coupon</returns>
        [HttpPost]
        [ActionName("couponupdate")]
        public ActionResult UpdateDiscountCoupon(Discount discount)
        {
            if (!ModelState.IsValid)
            {
                return View("EditCoupon", discount);
            }

            if (discount.DiscountType == 2 && (discount.ProductIds == null || discount.ProductIds.Length < 1))
            {
                ModelState.AddModelError("ProductIds", "Select at least one product.");
                BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
                discount.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);
                if (discount.BranchGuid != null && discount.BranchGuid != Guid.Empty)
                {
                    discount.ProductList = GetProductListByBranchGuid(discount.BranchGuid);
                    discount.ProductList = discount.ProductList ?? new List<CustomSelectListItem>();
                }
                return View("EditCoupon", discount);
            }

            if (discount.DiscountPriceType == 1 && discount.DiscountValue > 100)
            {
                ModelState.AddModelError("DiscountValue", "Percentage Value cann't be greater than 100%.");
                BusinessDomainLogic businessDomainLogic = new BusinessDomainLogic();
                discount.BranchList = businessDomainLogic.GetBranchList(UserVariables.LoggedInUserGuid);
                if (discount.BranchGuid != null && discount.BranchGuid != Guid.Empty)
                {
                    discount.ProductList = GetProductListByBranchGuid(discount.BranchGuid);
                    discount.ProductList = discount.ProductList ?? new List<CustomSelectListItem>();
                }
                return View("EditCoupon", discount);
            }

            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            var result = shoppingCartDomainLogic.UpdateDicountCoupon(discount);
            var message = result ? "Coupon has been updated." : "Operation failled.";
            TempData["hasError"] = !result;
            TempData["message"] = message;

            return RedirectToAction("coupons");
        }

        /// <summary>
        /// Get all the discount coupons foe the loggedIn merchant
        /// </summary>
        /// <returns>Html view to show the grid for coupons</returns>
        [HttpGet]
        [ActionName("coupons")]
        public ActionResult GetDiscountCoupons()
        {
            int Currentpage = 0;
            TempData["CurrentPage"] = null;

            if (Request.Url.AbsoluteUri.Contains('?') && Request.Url.AbsoluteUri.Contains("page"))
            {
                TempData["CurrentPage"] = Request.Url.AbsoluteUri.Split('&')[1].Split('=')[1].Split('&')[0];
                Currentpage = TempData["CurrentPage"] == null ? 0 : Convert.ToInt32(TempData["CurrentPage"]);
                TempData.Keep();
            }

            var hasError = TempData["hasError"] == null ? false : Convert.ToBoolean(TempData["hasError"]);
            var message = Convert.ToString(TempData["message"]);
            if (hasError && !string.IsNullOrEmpty(message))
                ErrorBlock(message);

            else if (!hasError && !string.IsNullOrEmpty(message))
                Success(message);

            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            var data = shoppingCartDomainLogic.GetDiscountCoupons(Currentpage);

            return View("DiscountCoupons", data);
        }

        /// <summary>
        /// Get all the product for the given branch
        /// </summary>
        /// <param name="branchGuid">branch guid</param>
        /// <returns>List of products</returns>
        [HttpGet]
        public ActionResult GetProductsBranchGuid(Guid branchGuid)
        {
            return Json(GetProductListByBranchGuid(branchGuid), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Helper method 
        /// Get products for the given guid
        /// </summary>
        /// <param name="branchGuid">Branch guid</param>
        /// <returns>CustomSelectListItem of products </returns>
        [NonAction]
        private List<CustomSelectListItem> GetProductListByBranchGuid(Guid branchGuid)
        {
            ShoppingCartDomainLogic shoppingCartDomainLogic = new ShoppingCartDomainLogic();
            return shoppingCartDomainLogic.GetProductListByBranchGuid(branchGuid);
        }

    }
}