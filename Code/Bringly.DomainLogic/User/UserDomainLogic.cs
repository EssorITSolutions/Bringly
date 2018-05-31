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
using Bringly.Domain.Enums;
using DotNetOpenAuth.AspNet.Clients;
using System.Configuration;
using Utilities.EmailSender;
using Utilities.EmailSender.Domain;
using Bringly.Domain.Business;

namespace Bringly.DomainLogic.User
{
    public class UserDomainLogic : BaseClass.DomainLogicBase
    {
        public bool AddUserProfile(UserRegistration userRegistration)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            tblUser user = new tblUser();
            user.UserGuid = Guid.NewGuid();
            user.FullName = userRegistration.FullName;
            user.EmailAddress = userRegistration.EmailAddress;
            user.CompanyorIndividual = string.IsNullOrEmpty(userRegistration.CompanyorIndividual)?"": userRegistration.CompanyorIndividual;
            user.Password = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            user.UserRegistrationType = TypeCast.ToType<int>(userRegistration.UserRegistrationType);
            user.DateCreated = DateTime.Now;
            user.IsDeleted = false;
            user.IsActive = true;
            user.PreferedCity = bringlyEntities.tblCities.Where(x => x.IsDeleted == false).ToList().FirstOrDefault().CityGuid;
            user.GoogleLoginEmail = "";
            user.FacebookLoginEmail ="";
            bringlyEntities.tblUsers.Add(user);
            bringlyEntities.SaveChanges();
            foreach (UserAddress usrAddress in userRegistration.UserAddresses)
            {
                    user.tblUserAddresses.Add(
                        new tblUserAddress
                        {
                            UserAddressGuid = Guid.NewGuid(),
                            UserGuid = user.UserGuid,
                            Address = usrAddress.Address,
                            AddressType = usrAddress.AddressType,
                            CityGuid = usrAddress.CityGuid,
                            PostCode = usrAddress.PostCode,
                            DateCreated = DateTime.Now
                        });
            }
            bringlyEntities.SaveChanges();
            if (user.UserRegistrationType == 3)
            {
                tblBusiness business = new tblBusiness();
                business.BusinessGuid = Guid.NewGuid();
                business.BusinessTypeGuid = userRegistration.BusinessTypeGuid;
                business.DateCreated = DateTime.Now;
                business.Email = user.EmailAddress;
                business.Address = "";
                business.PinCode = "";
                business.BusinessName = "";
                business.PNumber = "";
                business.Phone = "";
                business.CreatedByGuid = user.UserGuid;
                business.IsDeleted = false;
                business.ManagerUserGuid = user.UserGuid;
                bringlyEntities.tblBusinesses.Add(business);
                bringlyEntities.SaveChanges();
            }
            UserLogin userLogin = new Domain.User.UserLogin();
            userLogin.Username = user.EmailAddress;
            userLogin.UserPassword = user.Password;
            UserLogin(userLogin);
            EmailDomain emailDomain = new EmailDomain();
            emailDomain.EmailTo = user.EmailAddress;
            emailDomain.EmailFrom = user.EmailAddress;
            tblTemplate Template= bringlyEntities.tblTemplates.Where(x => x.TemplateType == "LoginCredentials").ToList().FirstOrDefault();
            emailDomain.EmailSubject = Template.Subject;
            emailDomain.EmailBody = Template.Body.Replace("{ToName}", user.FullName).Replace("{username}", user.EmailAddress).Replace("password", user.Password);
           // EmailSender.sendEmail(emailDomain);
            return true;
        }

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
                userProfile.CVRNumber = User.CVRNumber;
                userProfile.RoleGuid = (User.RoleGuid!=null && User.RoleGuid.HasValue)? User.RoleGuid.Value: Guid.Empty;
                userProfile.UserRegistrationType = User.UserRegistrationType.ToString();
                userProfile.PreferedCity = User.PreferedCity != null ? User.PreferedCity.ToString() : "";
                userProfile.ProfileImage = string.IsNullOrEmpty(User.ImageName) ? CommonDomainLogic.DefaultProfileImage : User.ImageName;
                foreach (tblUserAddress usrAddress in User.tblUserAddresses)
                {
                    userProfile.UserAddresses.Add(new UserAddress { UserAddressGuid = usrAddress.UserAddressGuid, Address = usrAddress.Address, CityGuid = usrAddress.CityGuid, CityName = usrAddress.tblCity.CityName, PostCode = usrAddress.PostCode, AddressType = usrAddress.AddressType });
                }
            }
            return userProfile;
        }

        public UserProfile FindUsertoberegistered(string email,int UserRegistrationType,Guid BusinessGuid)
        {
            tblUser User = new tblUser();
            tblBusiness Business = new tblBusiness();
            UserProfile userProfile = new UserProfile();
            userProfile.UserAddresses = new List<UserAddress>();
            bool alreadyregistered = false;
            if (UserRegistrationType == 3)
            {
                User = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Include(i => i.tblUserAddresses.Select(c => c.tblCity)).Where(x => x.EmailAddress == email).FirstOrDefault();
                if(User!=null)
                {
                    Business = bringlyEntities.tblBusinesses.Where(x => x.CreatedByGuid == User.UserGuid && x.BusinessGuid== BusinessGuid && x.IsDeleted==false).FirstOrDefault();
                    alreadyregistered = (Business == null) ? false : true;
                }
            }
            else {
                User = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Include(i => i.tblUserAddresses.Select(c => c.tblCity)).Where(x => x.EmailAddress == email).FirstOrDefault();
            }
            
           
            if (User != null && !alreadyregistered)
            {
                userProfile.UserGuid = User.UserGuid;
                userProfile.FullName = User.FullName;
                userProfile.EmailAddress = User.EmailAddress;
                userProfile.MobileNumber = User.MobileNumber;
                userProfile.CVRNumber = User.CVRNumber;
                userProfile.RoleGuid = (User.RoleGuid != null && User.RoleGuid.HasValue) ? User.RoleGuid.Value : Guid.Empty;
                userProfile.UserRegistrationType = User.UserRegistrationType.ToString();
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
            user.CVRNumber = string.IsNullOrEmpty(userProfile.CVRNumber)?"": userProfile.CVRNumber;
            //UserVariables.UserName = "";
            AuthencationTicket(user);
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
                            UserAddressGuid=Guid.NewGuid(),
                            UserGuid = userProfile.UserGuid,
                            Address = usrAddress.Address,
                            AddressType = usrAddress.AddressType,
                            CityGuid = usrAddress.CityGuid,
                            PostCode = usrAddress.PostCode,
                            DateCreated=DateTime.Now
                        });
                }
            }
            bringlyEntities.SaveChanges();
            return true;
        }

        public List<Restaurant> FavouriteRestaurants()
        {
            List<Guid> favouriteRestaurantGuids = bringlyEntities.tblFavourites.Where(f => f.CreatedByGuid == UserVariables.LoggedInUserGuid).Select(t => t.LocationGuid).ToList();
            return bringlyEntities.tblRestaurants.Where(r => favouriteRestaurantGuids.Contains(r.RestaurantGuid)).Select(f => new Restaurant { RestaurantImage = f.RestaurantImage, RestaurantGuid = f.RestaurantGuid, RestaurantName = f.RestaurantName, CityName = f.tblCity.CityName, IsFavorite = true, DateCreated = f.DateCreated }).ToList();
        }
        public List<BusinessObject> FavouriteLocations()
        {
            List<Guid> favouriteLocationGuids = bringlyEntities.tblFavourites.Where(f => f.CreatedByGuid == UserVariables.LoggedInUserGuid).Select(t => t.LocationGuid).ToList();
            var s = bringlyEntities.tblLocations.Where(r => favouriteLocationGuids.Contains(r.LocationGuid) && r.IsDeleted == false).Select(f => new BusinessObject { BusinessImage = f.LocationImage, BusinessGuid = f.LocationGuid, BusinessName = f.LocationName, CityName = f.tblCity.CityName, IsFavorite = true, DateCreated = f.DateCreated }).ToList();
            return bringlyEntities.tblLocations.Where(r => favouriteLocationGuids.Contains(r.LocationGuid) && r.IsDeleted == false).Select(f => new BusinessObject { BusinessImage = f.LocationImage, BusinessGuid = f.LocationGuid, BusinessName = f.LocationName, CityName = f.tblCity.CityName, IsFavorite = true, DateCreated = f.DateCreated }).ToList();
        }

        public bool AddFavourite(Guid locationGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.LocationGuid == locationGuid && f.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite == null)
            {
                bringlyEntities.tblFavourites.Add(new tblFavourite { FavouriteGuid = Guid.NewGuid(), LocationGuid = locationGuid, CreatedByGuid = UserVariables.LoggedInUserGuid, DateCreated = DateTime.Now });
                try
                {
                    bringlyEntities.SaveChanges();
                }
                catch (Exception ex)
                { }
            }
            return true;
        }

        public bool RemoveFavourite(Guid locationGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.LocationGuid == locationGuid && f.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
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

        public Message UserLogin(UserLogin userLogin,bool IsNewUser=false)
        {
            Message message = new Message();            
            tblUser user = bringlyEntities.tblUsers.Where(u => u.EmailAddress == userLogin.Username && u.Password == userLogin.UserPassword && u.IsDeleted == false).FirstOrDefault();
            if (user != null && user.IsActive)
            {
                AuthencationTicket(user);
                if (IsNewUser)
                {
                    message.MessageType = Domain.Enums.MessageType.NewUser;
                }
                else
                {
                    message.MessageType = Domain.Enums.MessageType.Success;
                }
                
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
            string urlpath = CommonDomainLogic.GetCurrentDomain;
            tblReview Review = bringlyEntities.tblReviews.Where(r => r.ReviewGuid == ReviewGuid && r.IsCompleted==false && r.IsSkipped==false).FirstOrDefault();
            MyReview.ReviewGuid = (Review != null && Review.ReviewGuid != Guid.Empty) ? Review.ReviewGuid : Guid.Empty;
            MyReview.UserGuid = (Review != null && Review.UserGuid != Guid.Empty) ? Review.UserGuid : Guid.Empty;
            MyReview.Review = (Review != null && !string.IsNullOrEmpty(Review.Review)) ? Review.Review : "";
            MyReview.RestaurantImage = (Review != null && !string.IsNullOrEmpty(Review.tblRestaurant.RestaurantImage)) ? urlpath+ CommonDomainLogic.GetImagePath(ImageType.Restaurant,Review.tblRestaurant.RestaurantImage) : CommonDomainLogic.GetImagePath(ImageType.Restaurant, "");
            MyReview.RestaurantName = (Review != null && !string.IsNullOrEmpty(Review.tblRestaurant.RestaurantName)) ? Review.tblRestaurant.RestaurantName : ""; 
            return MyReview;
        }

        public List<string> GetUserGuidFromEmailAddress(string emailaddress)
        {
            List<string> guidlist = new List<string>();
            guidlist.Add(bringlyEntities.tblUsers.Where(x => x.EmailAddress == emailaddress).FirstOrDefault().UserGuid.ToString());
            return guidlist;
        }

        public MyReview InsertReview(MyReview myReview)
        {  
                tblReview review = bringlyEntities.tblReviews.Where(u => u.ReviewGuid == myReview.ReviewGuid && u.IsDeleted == false).FirstOrDefault();
            tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
            review.Review = string.IsNullOrEmpty(myReview.Review)?"": myReview.Review;
            review.IsSkipped = myReview.IsSkipped;
            review.Rating = (byte)myReview.Rating;
            review.IsCompleted = myReview.IsSkipped?false:true;
            review.IsProcessed = false;
            review.DateCreated = DateTime.Now;
            if (review.IsCompleted)
            {
                EmailDomainLogic email = new EmailDomainLogic();
                ComposeEmail myEmail = new ComposeEmail();
                myEmail.EmailMessage = new Email();
                myEmail.EmailMessage.TemplateType = Enum.GetName(typeof(TemplateType),TemplateType.Review);
                tblTemplate templatereview = bringlyEntities.tblTemplates.Where(x => x.TemplateType == myEmail.EmailMessage.TemplateType).ToList().FirstOrDefault();
                myEmail.EmailMessage.Body = review.Review;
                myEmail.EmailMessage.EmailFrom = userfrom.EmailAddress;
                myEmail.EmailToGuid = GetUserGuidFromEmailAddress(templatereview.EmailFrom).ToArray();
                myEmail.EmailMessage.Subject = templatereview.Subject;
                myEmail.CreatedGuid= bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault().UserGuid;
                email.SendReviewEmail(myEmail);
                myEmail = new ComposeEmail();
                myEmail.EmailMessage = new Email();
                myEmail.EmailMessage.TemplateType = Enum.GetName(typeof(TemplateType), TemplateType.FeedBack);
                tblTemplate templatefeedback = bringlyEntities.tblTemplates.Where(x => x.TemplateType == myEmail.EmailMessage.TemplateType).ToList().FirstOrDefault();
                myEmail.EmailMessage.Body = review.Review;
                myEmail.EmailToGuid = GetUserGuidFromEmailAddress(userfrom.EmailAddress).ToArray();
                myEmail.EmailMessage.EmailFrom = templatefeedback.EmailFrom;
                myEmail.EmailMessage.Subject = templatefeedback.Subject;
                myEmail.CreatedGuid =  bringlyEntities.tblUsers.Where(x => x.EmailAddress == templatefeedback.EmailFrom).ToList().FirstOrDefault().UserGuid;
                email.SendReviewEmail(myEmail);
            }
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
                myReview.RestaurantName = restaurantReviews.tblRestaurant.RestaurantName;
                myReview.RestaurantImage = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, restaurantReviews.tblRestaurant.RestaurantImage);
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
                .Select(f => new RestaurantReview {
                    UserName = f.tblUser.FullName,
                    //em.tblEmailToes.Where(z=>z.EmailGuid== em.EmailGuid).ToList().Select(a=> new EmailToClass { UserName = a.tblUser.FullName }).ToList()
                    IsProcessed = f.IsProcessed.HasValue ? f.IsProcessed.Value : false, IsApproved = f.IsApproved.HasValue ? f.IsApproved.Value : false,
                    ReviewGuid = f.ReviewGuid, Review = f.Review, UserGuid = f.UserGuid, RestaurantGuid = f.RestaurantGuid, DateCreated = f.DateCreated, RestaurantName = f.tblRestaurant.RestaurantName, Rating = f.Rating })
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
                var ss = ex;
                message.MessageType = Domain.Enums.MessageType.Error;
                if(Isapprove)
                message.MessageText = "Error while approving the review";
                else message.MessageText = "Error while rejecting the review";
            }
            return message;
        }

        public List<Contact> GetAllBuyers()
        {
            List<Contact> UserContact = new List<Contact>();
            UserContact = bringlyEntities.tblUsers.Where(x => x.UserRegistrationType == 2).
                Select(up => new Contact { FullName = up.FullName, EmailAddress = up.EmailAddress, UserGuid = up.UserGuid }).ToList();
            return UserContact;
        }
        public List<Contact> GetAllMerchants()
        {
            List<Contact> UserContact = new List<Contact>();
            UserContact = bringlyEntities.tblUsers.Where(x => x.UserRegistrationType == 3).
                Select(up => new Contact { FullName = up.FullName, EmailAddress = up.EmailAddress, UserGuid = up.UserGuid }).ToList();
            return UserContact;
        }

        public Message RegisterUserthroughSociallogin(SocialLogin socialLogin)
        {
            
            tblUser User = bringlyEntities.tblUsers.Where(x => x.EmailAddress.ToLower() == socialLogin.EmailAddress.ToLower()).FirstOrDefault();
            bool IsNewUser = false;
            UserLogin userLogin = new Domain.User.UserLogin();
            if (User == null)
            {
                User = new tblUser();
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                User.UserGuid = Guid.NewGuid();
                User.FullName = socialLogin.FullName;
                User.UserRegistrationType = 2;
                User.EmailAddress = socialLogin.EmailAddress;
                User.DateCreated = DateTime.Now;
                User.IsDeleted = false;
                User.IsActive = true;
                User.PreferedCity = bringlyEntities.tblCities.Where(x => x.IsDeleted == false).ToList().FirstOrDefault().CityGuid;
                User.Password = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                User.GoogleLoginEmail = socialLogin.IsGoogleLogin ? socialLogin.EmailAddress : "";
                User.FacebookLoginEmail = !socialLogin.IsGoogleLogin ? socialLogin.EmailAddress : "";
                bringlyEntities.tblUsers.Add(User);
                bringlyEntities.SaveChanges();
                IsNewUser = true;
                userLogin.Username = socialLogin.EmailAddress;
                userLogin.UserPassword = User.Password;
            }
            else
            {
                if (string.IsNullOrEmpty(User.FacebookLoginEmail) && !socialLogin.IsGoogleLogin)
                {
                    User.FacebookLoginEmail = socialLogin.EmailAddress;
                    bringlyEntities.SaveChanges();
                }
                else if (string.IsNullOrEmpty(User.GoogleLoginEmail) && socialLogin.IsGoogleLogin)
                {
                    User.GoogleLoginEmail = socialLogin.EmailAddress;
                    bringlyEntities.SaveChanges();
                }
                userLogin.Username = User.EmailAddress;
                userLogin.UserPassword = User.Password;
            }


            return UserLogin(userLogin, IsNewUser);
        }

        public string FacebookLogin(Uri uri)
        {

            var fb = new Facebook.FacebookClient();
            //string aa = "https://www.facebook.com/v2.6/dialog/oauth?client_id="+id+ "&client_secret="+key+"&scope=public_profile,email&response_type=code&redirect_uri=" + uri.AbsoluteUri + "&state=STATE_TOKEN";
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["AppId"],
                client_secret = ConfigurationManager.AppSettings["AppSecret"],
                redirect_uri = uri.AbsoluteUri.Replace("http", "https"),
                response_type = "code",
                scope = "email"
            });

            return loginUrl.AbsoluteUri;
        }

        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Where(c => c.IsDeleted == false).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).ToList();
        }

        public bool AddUpdateUser(UserProfile userProfile)
        {
            tblUser user = new tblUser();
            if (userProfile.UserGuid != Guid.Empty)
            {
                user = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Where(x => x.UserGuid == userProfile.UserGuid).FirstOrDefault();
                user.FullName = userProfile.FullName;
                user.MobileNumber = userProfile.MobileNumber;
                user.EmailAddress = userProfile.EmailAddress;
                user.RoleGuid = userProfile.RoleGuid;
            }
            else
            {
                user.FullName = userProfile.FullName;
                user.MobileNumber = userProfile.MobileNumber;
                user.EmailAddress = userProfile.EmailAddress;
                user.RoleGuid = userProfile.RoleGuid;
                user.UserGuid = Guid.NewGuid();
                user.UserRegistrationType = 4;
                user.IsActive = true;
                user.IsDeleted = false;
                user.ParentGuid = UserVariables.LoggedInUserGuid;
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                user.Password = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                userProfile.UserGuid = user.UserGuid;
                user.DateCreated = DateTime.Now;
                bringlyEntities.tblUsers.Add(user);
            }
            
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
                            UserAddressGuid = Guid.NewGuid(),
                            UserGuid = userProfile.UserGuid,
                            Address = usrAddress.Address,
                            AddressType = usrAddress.AddressType,
                            CityGuid = usrAddress.CityGuid,
                            PostCode = usrAddress.PostCode,
                            DateCreated = DateTime.Now
                        });
                }
            }
            bringlyEntities.SaveChanges();
            return true;
        }
        public List<UserProfile> GetAllUsers()
        {
            List<UserProfile> user = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Where(x => x.ParentGuid == UserVariables.LoggedInUserGuid && x.UserRegistrationType==4 && x.IsDeleted==false)
                .Select(z => new UserProfile {
                    UserGuid=z.UserGuid,
                    FullName=z.FullName,
                    RoleGuid=z.RoleGuid.Value,
                    RoleName=bringlyEntities.tblRoles.Where(x=>x.RoleGuid==z.RoleGuid.Value).FirstOrDefault().RoleName

                })
                .ToList();

            return user;
        }

        public bool DeleteUser(Guid userGuid)
        {
            tblUser user = bringlyEntities.tblUsers.Where(x => x.UserGuid == userGuid).FirstOrDefault();
            user.IsDeleted = true;
            bringlyEntities.SaveChanges();
            return true;
        }

    }
}