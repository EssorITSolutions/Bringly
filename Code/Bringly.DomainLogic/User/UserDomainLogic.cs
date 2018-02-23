using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Bringly.Data;
using Bringly.Domain.User;
using Bringly.Domain.Enums.User;
using Bringly.Domain;
using System.IO;
using System.Web;
using System.Web.Security;
using Bringly.Domain.Common;
namespace Bringly.DomainLogic.User
{
    public class UserDomainLogic : BaseClass.DomainLogicBase
    {

        public UserProfile FindUser(Guid userGuid)
        {

            tblUser User = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Include(i => i.tblUserAddresses.Select(c => c.tblCity)).Where(x => x.UserGuid == userGuid).FirstOrDefault();
            UserProfile userProfile = new UserProfile();
            userProfile.UserAddresses = new List<UserAddress>();
            if (User != null)
            {
                userProfile.UserGuid = User.UserGuid;
                userProfile.FullName = User.FullName;
                userProfile.EmailAddress = User.EmailAddress;
                userProfile.MobileNumber = User.MobileNumber;
                userProfile.PreferedCity = User.PreferedCity != null ? User.PreferedCity.ToString() : "";
                userProfile.ProfileImage = string.IsNullOrEmpty(User.ImageName) ? CommonDomainLogic.DefaultProfileImage : User.ImageName;
                foreach (tblUserAddress usrAddress in User.tblUserAddresses)
                {
                    userProfile.UserAddresses.Add(new UserAddress { UserAddressGuid = usrAddress.UserAddressGuid, Address = usrAddress.Address, CityGuid = usrAddress.CityGuid, CityName = usrAddress.tblCity.CityName, PostCode = usrAddress.PostCode, AddressType = usrAddress.AddressType });
                }
            }
            return userProfile;
        }
       
        public bool UpdatePreferedCity(Guid cityGuid)
        {
            tblUser user = bringlyEntities.tblUsers.Where(u => u.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            user.PreferedCity = cityGuid;
            bringlyEntities.SaveChanges();
            return true;
        }

        public bool UpdateUserProfile(UserProfile userProfile)
        {
            tblUser user = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Where(x => x.UserGuid == userProfile.UserGuid).FirstOrDefault();
            user.FullName = userProfile.FullName;
            user.MobileNumber = userProfile.MobileNumber;
            foreach (UserAddress usrAddress in userProfile.UserAddresses)
            {
                if (usrAddress.UserAddressGuid != Guid.Empty)
                {
                    tblUserAddress userExistingAddress = user.tblUserAddresses.Where(x => x.UserAddressGuid == usrAddress.UserAddressGuid).FirstOrDefault();
                    userExistingAddress.UserGuid = userProfile.UserGuid;
                    userExistingAddress.Address = usrAddress.Address;
                    userExistingAddress.AddressType = usrAddress.AddressType;
                    userExistingAddress.CityGuid = usrAddress.CityGuid;
                    userExistingAddress.PostCode = usrAddress.PostCode;
                }
                else
                {
                    user.tblUserAddresses.Add(
                        new tblUserAddress
                        {
                            UserGuid = userProfile.UserGuid,
                            Address = usrAddress.Address,
                            AddressType = usrAddress.AddressType,
                            CityGuid = usrAddress.CityGuid,
                            PostCode = usrAddress.PostCode
                        });
                }
            }
            bringlyEntities.SaveChanges();
            return true;
        }

        public List<Restaurant> FavouriteRestaurants()
        {
            List<Guid> favouriteRestaurantGuids = bringlyEntities.tblFavourites.Where(f => f.CreatedByGuid == UserVariables.LoggedInUserGuid).Select(t => t.RestaurantGuid).ToList();
            return bringlyEntities.tblRestaurants.Where(r => favouriteRestaurantGuids.Contains(r.RestaurantGuid)).Select(f => new Restaurant { RestaurantImage = f.RestaurantImage, RestaurantGuid = f.RestaurantGuid, RestaurantName = f.RestaurantName, CityName = f.tblCity.CityName, IsFavorite = true, DateCreated = f.DateCreated }).ToList();
        }

        public bool AddFavourite(Guid restaurantGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.RestaurantGuid == restaurantGuid && f.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite == null)
            {
                bringlyEntities.tblFavourites.Add(new tblFavourite { FavouriteGuid = Guid.NewGuid(), RestaurantGuid = restaurantGuid, CreatedByGuid = UserVariables.LoggedInUserGuid, DateCreated = DateTime.Now });
                bringlyEntities.SaveChanges();
            }
            return true;
        }

        public bool RemoveFavourite(Guid restaurantGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.RestaurantGuid == restaurantGuid && f.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite != null)
            {
                bringlyEntities.tblFavourites.Remove(userFavourite);
                bringlyEntities.SaveChanges();
            }
            return true;
        }

        public string UpdateProfileImage(HttpRequestBase Request)
        {
            tblUser user = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            string imageName = "";
            string imageLocation = "";
            if (Request.Files.Count > 0)
            {
                UserDomainLogic userdomainLogic = new UserDomainLogic();
                UserProfile userProfile = new UserProfile();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    imageName = Path.GetFileName("Buyer_" + Guid.NewGuid() + Path.GetExtension(Request.Files[i].FileName));
                    imageLocation = CommonDomainLogic.GetImagePath(Domain.Enums.ImageType.User, imageName);
                    Request.Files[i].SaveAs(HttpContext.Current.Server.MapPath(imageLocation));
                }
                userProfile.ProfileImage = imageName;
                user.ImageName = userProfile.ProfileImage;
                bringlyEntities.SaveChanges();
                return imageLocation;
            }
            else
            {
                return imageLocation;
            }

        }

        public Message UserLogin(UserLogin userLogin)
        {
            Message message = new Message();
            tblUser user = bringlyEntities.tblUsers.Where(u => u.EmailAddress == userLogin.Username && u.Password == userLogin.UserPassword && u.IsDeleted == false).FirstOrDefault();
            if (user != null && user.IsActive)
            {
                AuthencationTicket(user);
                message.MessageType = Domain.Enums.MessageType.Success;
            }
            else if (user != null && user.IsActive == false)
            {
                message.MessageType = Domain.Enums.MessageType.Error;
                message.MessageText = "Your account has been deactivated, Please contact administrator";
            }
            else
            {
                message.MessageType = Domain.Enums.MessageType.Error;
                message.MessageText = "Wrong username or password";
            }
            return message;

        }

        public MyReview IsPendingReview(Guid UserGuid)
        {
            MyReview MyReview = new MyReview();
            tblReview Review= bringlyEntities.tblReviews.Include("tbiUser").Where(r => r.UserGuid == UserGuid).OrderByDescending(r => r.DateCreated).FirstOrDefault();
            MyReview.ReviewGuid = (Review != null && Review.ReviewGuid != Guid.Empty) ? Review.ReviewGuid : Guid.Empty; 
            return MyReview;
        }

        public MyReview GetReviewByGuid(Guid ReviewGuid)
        {
            MyReview MyReview = new MyReview();
            tblReview Review = bringlyEntities.tblReviews.Where(r => r.ReviewGuid == ReviewGuid && r.IsCompleted==false && r.IsSkipped==false).FirstOrDefault();
            MyReview.ReviewGuid = (Review != null && Review.ReviewGuid != Guid.Empty) ? Review.ReviewGuid : Guid.Empty;
            MyReview.UserGuid = (Review != null && Review.UserGuid != Guid.Empty) ? Review.UserGuid : Guid.Empty;
            MyReview.Review = (Review != null && !string.IsNullOrEmpty(Review.Review)) ? Review.Review : "";
            MyReview.RestaurantImage = (Review != null && !string.IsNullOrEmpty(Review.tblRestaurant.RestaurantImage)) ? Review.tblRestaurant.RestaurantImage : "";
            return MyReview;
        }

        public MyReview InsertReview(MyReview myReview)
        {  
                tblReview review = bringlyEntities.tblReviews.Where(u => u.ReviewGuid == myReview.ReviewGuid && u.IsDeleted == false).FirstOrDefault();
                review.Review = string.IsNullOrEmpty(myReview.Review)?"": myReview.Review;
                review.IsSkipped = myReview.IsSkipped;
                review.Rating = (byte)myReview.Rating;
                review.IsCompleted = myReview.IsSkipped?false:true;
                review.IsProcessed = false;
            bringlyEntities.SaveChanges();
           
            return GetMyReviewBuyer(myReview.UserGuid);
        }
        public MyReview GetMyReviewBuyer(Guid UserGuid)
        {
            MyReview myReview = new MyReview();
            tblReview restaurantReviews = bringlyEntities.tblReviews.Where(u => u.UserGuid == UserGuid && u.IsDeleted == false && u.IsCompleted == false && u.IsSkipped == false).ToList().FirstOrDefault();

            if (restaurantReviews != null && restaurantReviews.ReviewGuid != Guid.Empty)
            {
                myReview.RestaurantGuid = restaurantReviews.RestaurantGuid;
                myReview.UserGuid = restaurantReviews.UserGuid;
                myReview.ReviewGuid = restaurantReviews.ReviewGuid;
                myReview.RestaurantImage = restaurantReviews.tblRestaurant.RestaurantImage;
            }
            myReview.RestaurantReviews = bringlyEntities.tblReviews.Where(u => u.UserGuid == UserGuid && u.IsDeleted == false && u.IsCompleted==true)
                .Select(f=>new RestaurantReview { IsProcessed=f.IsProcessed.HasValue ? f.IsProcessed.Value : false, IsApproved = f.IsApproved.HasValue ? f.IsApproved.Value : false, ReviewGuid = f.ReviewGuid,Review=f.Review,UserGuid=f.UserGuid,RestaurantGuid=f.RestaurantGuid,DateCreated=f.DateCreated,RestaurantName=f.tblRestaurant.RestaurantName,Rating=f.Rating })
                .OrderByDescending(x=>x.DateCreated)
                .ToList();
            return myReview;
        }
        public MyReview GetMyReviewMerchant(MyReview myReview)
        {
            
            myReview.PageSize = PageSize;
            myReview.CurrentPage = CurrentPage;
            myReview.SortBy = SortBy;
            myReview.RestaurantReviews = bringlyEntities.tblReviews.Where(u => u.CreatedByGuid == myReview.UserGuid && u.IsDeleted == false && u.IsCompleted == true)
                .Select(f => new RestaurantReview { UserName = f.tblUser.FullName, IsProcessed = f.IsProcessed.HasValue ? f.IsProcessed.Value : false, IsApproved = f.IsApproved.HasValue ? f.IsApproved.Value : false, ReviewGuid = f.ReviewGuid, Review = f.Review, UserGuid = f.UserGuid, RestaurantGuid = f.RestaurantGuid, DateCreated = f.DateCreated, RestaurantName = f.tblRestaurant.RestaurantName, Rating = f.Rating })
                .OrderByDescending(x => x.IsApproved == true).OrderByDescending(x => x.IsProcessed == false)
                .ToList();
            myReview.TotalRecords = myReview.RestaurantReviews.Count;
            int Skip = 0;
            int Take = 5;
            //if (myReview.PageSize == 1)
            //    Skip = 0;
            //else
            if (myReview.CurrentPage == 1)
                Skip = 0;
            else
                Skip = ((myReview.CurrentPage* myReview.PageSize) - myReview.PageSize);
            
            tblReview restaurantReviews = bringlyEntities.tblReviews.Where(u => u.CreatedByGuid == myReview.UserGuid && u.IsDeleted == false && u.IsCompleted == true).ToList().FirstOrDefault();
          
            if (restaurantReviews != null && restaurantReviews.ReviewGuid != Guid.Empty)
            {
                myReview.RestaurantGuid = restaurantReviews.RestaurantGuid;
                myReview.UserGuid = restaurantReviews.UserGuid;
                myReview.ReviewGuid = restaurantReviews.ReviewGuid;
                myReview.RestaurantImage = restaurantReviews.tblRestaurant.RestaurantImage;
            }
            myReview.RestaurantReviews= myReview.RestaurantReviews.Skip(Skip).Take(Take).ToList();
            return myReview;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        private void AuthencationTicket(tblUser user)
        {
            string userData =
                user.UserGuid + "}" //0
                + user.EmailAddress + "}" //1
                + (user.UserRegistrationType).ToString() + "}"//2  
                + user.FullName;
            FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, userData, DateTime.Now, DateTime.Now.AddHours(5), false, "Bringly", FormsAuthentication.FormsCookiePath);
            string st = FormsAuthentication.Encrypt(tkt);
            HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, st) { HttpOnly = true };
            HttpContext.Current.Response.Cookies.Add(ck);
        }
        public Message ApproveReviewLogic(Guid ReviewGuid, bool Isapprove)
        {
            Message message = new Message();
            try
            {
                tblReview review = bringlyEntities.tblReviews.Where(u => u.ReviewGuid == ReviewGuid).FirstOrDefault();
                review.IsApproved = Isapprove;
                review.IsProcessed = true;
                review.ApproveDate = DateTime.Now;
                bringlyEntities.SaveChanges();
                message.MessageType = Domain.Enums.MessageType.Success;
                message.MessageText = "";
            }
            catch (Exception ex) {
                message.MessageType = Domain.Enums.MessageType.Error;
                if(Isapprove)
                message.MessageText = "Error while approving the review";
                else message.MessageText = "Error while rejecting the review";
            }
            return message;
        }
    }
}