using Bringly.Data;
using Bringly.Domain;
using Bringly.Domain.Business;
using Bringly.Domain.Common;
using Bringly.Domain.Enums;
using Bringly.Domain.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using Utilities.Helper;

namespace Bringly.DomainLogic.User
{
    public class UserDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// Add a new user.
        /// Add the user profile data
        /// </summary>
        /// <param name="userRegistration">User registration/profile data</param>
        /// <returns>true if user added sucessfuly else false</returns>
        public bool AddUserProfile(UserRegistration userRegistration)
        {
            tblUser user = new tblUser();
            user.UserGuid = Guid.NewGuid();
            user.FullName = userRegistration.FullName;
            user.EmailAddress = userRegistration.EmailAddress;
            user.MobileNumber = userRegistration.MobileNumber;
            user.CompanyorIndividual = string.IsNullOrEmpty(userRegistration.CompanyorIndividual) ? "" : userRegistration.CompanyorIndividual;

            user.UserRegistrationType = TypeCast.ToType<int>(userRegistration.UserRegistrationType);
            user.DateCreated = DateTime.Now;
            user.IsDeleted = false;
            user.IsActive = true;
            user.FK_PreferedCity = bringlyEntities.tblCities.Where(x => x.IsDeleted == false).ToList().FirstOrDefault().CityGuid;
            user.GoogleLoginEmail = "";
            user.FacebookLoginEmail = "";
            bringlyEntities.tblUsers.Add(user);

            foreach (UserAddress usrAddress in userRegistration.UserAddresses)
            {
                user.tblUserAddresses.Add(
                    new tblUserAddress
                    {
                        UserAddressGuid = Guid.NewGuid(),
                        FK_UserGuid = user.UserGuid,
                        Address = usrAddress.Address,
                        AddressType = usrAddress.AddressType,
                        FK_CityGuid = usrAddress.CityGuid,
                        PostCode = usrAddress.PostCode,
                        Country = usrAddress.Country,
                        DateCreated = DateTime.Now,
                        Latitude = usrAddress.Latitude,
                        Longitude = usrAddress.Longitude,
                        PlaceId = usrAddress.PlaceId
                    });
            }

            if (user.UserRegistrationType == 3)
            {
                tblBusiness business = new tblBusiness();
                business.BusinessGuid = Guid.NewGuid();
                business.FK_BusinessTypeGuid = userRegistration.BusinessTypeGuid ?? Guid.Empty;
                business.DateCreated = DateTime.Now;
                business.Email = user.EmailAddress;
                business.Address = "";
                business.PinCode = "";
                business.BusinessName = "";
                business.PNumber = "";
                business.Phone = "";
                business.FK_CreatedByGuid = user.UserGuid;
                business.IsDeleted = false;
                business.ManagerUserGuid = user.UserGuid;
                bringlyEntities.tblBusinesses.Add(business);
            }
          
            bringlyEntities.SaveChanges();
            EmailDomainLogic emailDomainLogic = new EmailDomainLogic();
            emailDomainLogic.SendUserVerificationConfirmationEmail(user.UserGuid);
            return true;
        }

        /// <summary>
        /// Find user by user guid
        /// </summary>
        /// <param name="userGuid">User Guid</param>
        /// <returns>User profile data</returns>
        public UserProfile FindUser(Guid userGuid)
        {
            tblUser User = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses)
                .Include(i => i.tblUserAddresses.Select(c => c.tblCity)).Where(x => x.UserGuid == userGuid).FirstOrDefault();
            UserProfile userProfile = new UserProfile();
            userProfile.UserAddresses = new List<UserAddress>();

            if (User != null)
            {
                userProfile.UserGuid = User.UserGuid;
                userProfile.FullName = User.FullName;
                userProfile.EmailAddress = User.EmailAddress;
                userProfile.MobileNumber = User.MobileNumber;
                userProfile.CVRNumber = User.CVRNumber;
                userProfile.RoleGuid = (User.RoleGuid != null && User.RoleGuid.HasValue) ? User.RoleGuid.Value : Guid.Empty;
                userProfile.UserRegistrationType = User.UserRegistrationType.ToString();
                userProfile.PreferedCity = User.FK_PreferedCity != null ? User.FK_PreferedCity.ToString() : "";
                userProfile.ProfileImage = !string.IsNullOrEmpty(User.ImageName) ? User.ImageName : !string.IsNullOrEmpty(User.GoogleUserProfileImageUrl) ? User.GoogleUserProfileImageUrl : !string.IsNullOrEmpty(User.FacebookUserProfileImageUrl) ? User.FacebookUserProfileImageUrl : CommonDomainLogic.DefaultProfileImage;
                foreach (tblUserAddress usrAddress in User.tblUserAddresses)
                {
                    userProfile.UserAddresses.Add(new UserAddress { UserAddressGuid = usrAddress.UserAddressGuid, Address = usrAddress.Address, CityGuid = usrAddress.FK_CityGuid, CityName = usrAddress.tblCity.CityName, PostCode = usrAddress.PostCode, AddressType = usrAddress.AddressType });
                }
            }

            return userProfile;
        }

        /// <summary>
        /// Find user by email address
        /// </summary>
        /// <param name="email">Email address of user</param>
        /// <param name="UserRegistrationType">User registration type</param>
        /// <param name="BusinessGuid">Business Guid of the user(For merchant only)</param>
        /// <returns>User profile data</returns>
        public UserProfile FindUsertoberegistered(string email, int UserRegistrationType, Guid? BusinessGuid)
        {
            tblUser User = new tblUser();
            tblBusiness Business = new tblBusiness();
            UserProfile userProfile = new UserProfile();
            userProfile.UserAddresses = new List<UserAddress>();
            bool alreadyregistered = false;
            if (UserRegistrationType == 3)
            {
                User = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Include(i => i.tblUserAddresses.Select(c => c.tblCity)).Where(x => x.EmailAddress == email).FirstOrDefault();
                if (User != null)
                {
                    Business = bringlyEntities.tblBusinesses.Where(x => x.FK_CreatedByGuid == User.UserGuid && x.BusinessGuid == BusinessGuid && x.IsDeleted == false).FirstOrDefault();
                    alreadyregistered = (Business == null) ? false : true;
                }
            }
            else
            {
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
                userProfile.PreferedCity = User.FK_PreferedCity != null ? User.FK_PreferedCity.ToString() : "";
                userProfile.ProfileImage = !string.IsNullOrEmpty(User.ImageName) ? User.ImageName : !string.IsNullOrEmpty(User.GoogleUserProfileImageUrl) ? User.GoogleUserProfileImageUrl : !string.IsNullOrEmpty(User.FacebookUserProfileImageUrl) ? User.FacebookUserProfileImageUrl : CommonDomainLogic.DefaultProfileImage;
                foreach (tblUserAddress usrAddress in User.tblUserAddresses)
                {
                    userProfile.UserAddresses.Add(new UserAddress { UserAddressGuid = usrAddress.UserAddressGuid, Address = usrAddress.Address, CityGuid = usrAddress.FK_CityGuid, CityName = usrAddress.tblCity.CityName, PostCode = usrAddress.PostCode, AddressType = usrAddress.AddressType });
                }
            }

            return userProfile;
        }

        /// <summary>
        /// Update buyer(user) prefered city
        /// </summary>
        /// <param name="cityGuid">Selected city guid</param>
        /// <returns>true if updated else false</returns>
        public bool UpdatePreferedCity(Guid cityGuid)
        {
            tblUser user = bringlyEntities.tblUsers.Where(u => u.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            user.FK_PreferedCity = cityGuid;
            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// Update user file data
        /// </summary>
        /// <param name="userProfile">User profile data to be updated</param>
        /// <returns>true if updated else false</returns>
        public bool UpdateUserProfile(UserProfile userProfile)
        {
            tblUser user = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Where(x => x.UserGuid == userProfile.UserGuid).FirstOrDefault();
            user.FullName = userProfile.FullName;
            user.MobileNumber = userProfile.MobileNumber;
            user.CVRNumber = string.IsNullOrEmpty(userProfile.CVRNumber) ? "" : userProfile.CVRNumber;

            if (!string.IsNullOrEmpty(userProfile.UserRegistrationType))
            {
                user.UserRegistrationType = TypeCast.ToType<int>(userProfile.UserRegistrationType);
                user.CompanyorIndividual = string.IsNullOrEmpty(userProfile.CompanyorIndividual) ? "" : userProfile.CompanyorIndividual;
            }

            AuthencationTicket(user);

            foreach (UserAddress usrAddress in userProfile.UserAddresses)
            {
                if (usrAddress.UserAddressGuid != Guid.Empty)
                {
                    tblUserAddress userExistingAddress = user.tblUserAddresses.Where(x => x.UserAddressGuid == usrAddress.UserAddressGuid).FirstOrDefault();
                    userExistingAddress.FK_UserGuid = userProfile.UserGuid;
                    userExistingAddress.Address = usrAddress.Address;
                    userExistingAddress.AddressType = usrAddress.AddressType;
                    userExistingAddress.FK_CityGuid = usrAddress.CityGuid;
                    userExistingAddress.PostCode = usrAddress.PostCode;
                }
                else
                {
                    user.tblUserAddresses.Add(
                        new tblUserAddress
                        {
                            UserAddressGuid = Guid.NewGuid(),
                            FK_UserGuid = userProfile.UserGuid,
                            Address = usrAddress.Address,
                            AddressType = usrAddress.AddressType,
                            FK_CityGuid = usrAddress.CityGuid,
                            PostCode = usrAddress.PostCode,
                            DateCreated = DateTime.Now
                        });
                }
            }

            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// Get favourite restaurant list
        /// </summary>
        /// <returns>List of favourite restaurants</returns>
        public List<Restaurant> FavouriteRestaurants()
        {
            List<Guid> favouriteRestaurantGuids = bringlyEntities.tblFavourites.Where(f => f.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).Select(t => t.FK_LocationGuid).ToList();
            return bringlyEntities.tblRestaurants.Where(r => favouriteRestaurantGuids.Contains(r.RestaurantGuid)).Select(f => new Restaurant { RestaurantImage = f.RestaurantImage, RestaurantGuid = f.RestaurantGuid, RestaurantName = f.RestaurantName, CityName = f.tblCity.CityName, IsFavorite = true, DateCreated = f.DateCreated }).ToList();
        }

        /// <summary>
        /// Get favourite buseness branches of buyer
        /// </summary>
        /// <param name="take">number of record to be fetched</param>
        /// <returns>list of businessObject</returns>
        public List<BusinessObject> FavouriteLocations(int take = 0)
        {
            List<Guid> favouriteLocationGuids = bringlyEntities.tblFavourites.Where(f => f.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).Select(t => t.FK_LocationGuid).ToList();
            if (take <= 0)
                return bringlyEntities.tblBranches
                    .Where(r => favouriteLocationGuids.Contains(r.BranchGuid) && r.IsDeleted == false)
                    .Select(f => new BusinessObject
                    {
                        BusinessImage = f.BranchImage,
                        BusinessGuid = f.BranchGuid,
                        BusinessName = f.BranchName,
                        CityName = f.tblCity.CityName,
                        IsFavorite = true,
                        DateCreated = f.DateCreated,
                        Address = f.Address
                    }).ToList();
            else
                return bringlyEntities.tblBranches
                    .Where(r => favouriteLocationGuids.Contains(r.BranchGuid) && r.IsDeleted == false).Take(take)
                    .Select(f => new BusinessObject
                    {
                        BusinessImage = f.BranchImage,
                        BusinessGuid = f.BranchGuid,
                        BusinessName = f.BranchName,
                        CityName = f.tblCity.CityName,
                        IsFavorite = true,
                        DateCreated = f.DateCreated,
                        Address = f.Address
                    }).ToList();
        }

        /// <summary>
        /// Add the given branch guid as fevourite
        /// </summary>
        /// <param name="locationGuid">Branch guid to be add as fevourite</param>
        /// <param name="IsFavourite">true or false(set as fevourite or not) </param>
        /// <returns>true if sucess else false</returns>
        public bool AddFavourite(Guid locationGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.FK_LocationGuid == locationGuid && f.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite == null)
            {
                bringlyEntities.tblFavourites.Add(new tblFavourite { FavouriteGuid = Guid.NewGuid(), FK_LocationGuid = locationGuid, FK_CreatedByGuid = UserVariables.LoggedInUserGuid, DateCreated = DateTime.Now });
                try
                {
                    bringlyEntities.SaveChanges();
                }
                catch (Exception)
                { }
            }

            return true;
        }

        /// <summary>
        /// Remove the givenbranch guid from favourite
        /// </summary>
        /// <param name="locationGuid">Branch guid to be remove</param>
        /// <param name="IsFavourite">true or false</param>
        /// <returns>true if sucess else false</returns>
        public bool RemoveFavourite(Guid locationGuid, string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.FK_LocationGuid == locationGuid && f.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite != null)
            {
                bringlyEntities.tblFavourites.Remove(userFavourite);
                bringlyEntities.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public string UpdateProfileImage(HttpRequestBase Request)
        {
            tblUser user = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            string imageName = "";
            string imageLocation = "";
            if (Request.Files.Count > 0 && user != null)
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

        /// <summary>
        /// Login with user cridentials
        /// </summary>
        /// <param name="userLogin">UserName and User password</param>
        /// <param name="IsNewUser">optional</param>
        /// <returns>Message type</returns>
        public Message UserLogin(UserLogin userLogin, bool IsNewUser = false)
        {
            Message message = new Message();
            tblUser user = bringlyEntities.tblUsers.Where(u => u.EmailAddress == userLogin.Username && u.Password == userLogin.UserPassword && u.IsDeleted == false && u.IsVerified == true).FirstOrDefault();
            if (user != null && user.IsActive)
            {
                if (user.UserRegistrationType == 3)
                {
                    if (user.IsAdminApproved == true)
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
                    else
                    {
                        message.MessageType = Domain.Enums.MessageType.Error;
                        message.MessageText = "Account is not activated yet. Please contact admin.";
                    }
                }
                else
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
            }
            else if (user != null && !user.IsActive)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public MyReview IsPendingReview(Guid UserGuid)
        {
            MyReview MyReview = new MyReview();
            tblReview Review = bringlyEntities.tblReviews.Include("tbiUser").Where(r => r.FK_UserGuid == UserGuid).OrderByDescending(r => r.DateCreated).FirstOrDefault();
            MyReview.ReviewGuid = (Review != null && Review.ReviewGuid != Guid.Empty) ? Review.ReviewGuid : Guid.Empty;
            return MyReview;
        }

        /// <summary>
        /// Get review record by review Guid
        /// </summary>
        /// <param name="ReviewGuid">Review Guid for which record will be fetched</param>
        /// <returns>Record for the ReviewGuid</returns>
        public MyReview GetReviewByGuid(Guid ReviewGuid)
        {
            MyReview MyReview = new MyReview();
            string urlpath = CommonDomainLogic.GetCurrentDomain;
            tblReview Review = bringlyEntities.tblReviews.Where(r => r.ReviewGuid == ReviewGuid).FirstOrDefault();
            MyReview.ReviewGuid = (Review != null && Review.ReviewGuid != Guid.Empty) ? Review.ReviewGuid : Guid.Empty;
            MyReview.UserGuid = (Review != null && Review.FK_UserGuid != Guid.Empty) ? Review.FK_UserGuid : Guid.Empty;
            MyReview.Review = (Review != null && !string.IsNullOrEmpty(Review.Review)) ? Review.Review : "";
            MyReview.BusinessImage = (Review != null && !string.IsNullOrEmpty(Review.tblBusiness.BusinessImage)) ? urlpath + CommonDomainLogic.GetImagePath(ImageType.Restaurant, Review.tblBusiness.BusinessImage) : CommonDomainLogic.GetImagePath(ImageType.Restaurant, "");
            MyReview.BusinessName = (Review != null && !string.IsNullOrEmpty(Review.tblBusiness.BusinessName)) ? Review.tblBusiness.BusinessName : "";
            return MyReview;
        }

        /// <summary>
        /// Get user guid by its email address
        /// </summary>
        /// <param name="emailaddress">Emaill address uf user</param>
        /// <returns>Guid of the user</returns>
        public List<string> GetUserGuidFromEmailAddress(string emailaddress)
        {
            List<string> guidlist = new List<string>();
            guidlist.Add(bringlyEntities.tblUsers.Where(x => x.EmailAddress == emailaddress).FirstOrDefault().UserGuid.ToString());
            return guidlist;
        }

        /// <summary>
        /// Get merchant and branch manager user guid  to send the email for the review
        /// </summary>
        /// <param name="emailaddress">Merchant emaill address</param>
        /// <param name="businessGuid">Merchant business guid</param>
        /// <returns>List of guid(merchant's and manager guids)</returns>
        public List<string> GetUserGuidFromEmailAddressToSendEmailForReviewMerchant(Guid? branchGuid, Guid businessGuid)
        {
            List<string> guidlist = new List<string>();
            
            var merchantGuid = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == businessGuid && x.IsDeleted == false).Select(x => x.FK_CreatedByGuid).FirstOrDefault();

            if (merchantGuid != null)
            {
                if (branchGuid != null && branchGuid != Guid.Empty)
                {
                    var guid = (from b in bringlyEntities.tblBranches
                                where b.FK_BusinessGuid == businessGuid && b.BranchGuid == branchGuid && b.FK_CreatedByGuid == merchantGuid && b.IsDeleted == false
                                select b.ManagerUserGuid.ToString()).FirstOrDefault();

                    if (!string.IsNullOrEmpty(guid))
                        guidlist.Add(guid);
                }
                guidlist.Add(merchantGuid.ToString());
                guidlist.Distinct();
            }
            return guidlist;
        }

        public string GetMerchantOrBuyerEmailForBuyerReviewFeedback(Guid? branchGuid, Guid businessGuid)
        {
            if (branchGuid != null && branchGuid != Guid.Empty)
            {
                var guid = (from b in bringlyEntities.tblBranches
                            where b.BranchGuid == branchGuid && b.IsDeleted == false
                            select b.ManagerUserGuid).FirstOrDefault();

                if (guid != null)
                {
                    var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == guid).FirstOrDefault();
                    if (user != null)
                        return user.EmailAddress;
                }
            }

            var merchantGuid = bringlyEntities.tblBusinesses.Where(x => x.BusinessGuid == businessGuid && x.IsDeleted == false).Select(x => x.FK_CreatedByGuid).FirstOrDefault();

            if (merchantGuid != null)
            {
                return bringlyEntities.tblUsers
                     .Where(usr => usr.UserGuid == merchantGuid && usr.IsDeleted == false && usr.IsActive == true).Select(usr => usr.EmailAddress).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Create new review.
        /// </summary>
        /// <param name="myReview">New Review Data</param>
        /// <returns>MyReview</returns>
        public MyReview InsertReview(MyReview myReview)
        {
            tblReview review = bringlyEntities.tblReviews.Where(u => u.ReviewGuid == myReview.ReviewGuid && u.IsDeleted == false).FirstOrDefault();
            if (review != null)
            {
                tblUser userfrom = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault();
                review.Review = string.IsNullOrEmpty(myReview.Review) ? "" : myReview.Review;
                review.IsSkipped = myReview.IsSkipped;
                review.Rating = (byte)myReview.Rating;
                review.IsReviewed = true;
                review.DateCreated = DateTime.Now;

                if (myReview.IsSkipped)
                {
                    var reviews = (from z in bringlyEntities.tblReviews
                                   where z.FK_BusinessGuid == review.FK_BusinessGuid && z.IsDeleted == false
                                   && z.IsProcessed == false && z.IsApproved == false && z.IsReviewed == false
                                   select z);
                    foreach (var reviewObj in reviews)
                    {
                        reviewObj.IsSkipped = true;
                        reviewObj.IsReviewed = true;
                    }
                }

                bringlyEntities.SaveChanges();

                EmailDomainLogic email = new EmailDomainLogic();
                ComposeEmail myEmail = new ComposeEmail();
                myEmail.EmailMessage = new Email();
                myEmail.EmailMessage.TemplateType = Enum.GetName(typeof(TemplateType), TemplateType.Review);
                tblEmailTemplate templatereview = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == myEmail.EmailMessage.TemplateType).ToList().FirstOrDefault();
                if (templatereview != null)
                {
                    myEmail.EmailMessage.Body = review.Review;
                    myEmail.EmailMessage.EmailFrom = userfrom.EmailAddress;
                    myEmail.EmailToGuid = GetUserGuidFromEmailAddressToSendEmailForReviewMerchant(review.FK_BranchGuid, review.FK_BusinessGuid).ToArray();
                    //myEmail.EmailToGuid = GetUserGuidFromEmailAddressToSendEmailForReviewMerchant(userfrom.EmailAddress, review.FK_BusinessGuid).ToArray();
                    myEmail.EmailMessage.Subject = templatereview.Subject;
                    myEmail.CreatedGuid = bringlyEntities.tblUsers.Where(x => x.UserGuid == UserVariables.LoggedInUserGuid).ToList().FirstOrDefault().UserGuid;
                    email.SendReviewEmail(myEmail);
                }

                myEmail = new ComposeEmail();
                myEmail.EmailMessage = new Email();
                myEmail.EmailMessage.TemplateType = Enum.GetName(typeof(TemplateType), TemplateType.FeedBack);
                tblEmailTemplate templatefeedback = bringlyEntities.tblEmailTemplates.Where(x => x.TemplateType == myEmail.EmailMessage.TemplateType).ToList().FirstOrDefault();
                if (templatefeedback != null)
                {
                    myEmail.EmailMessage.Body = review.Review;
                    myEmail.EmailToGuid = GetUserGuidFromEmailAddress(userfrom.EmailAddress).ToArray();
                    myEmail.EmailMessage.EmailFrom = GetMerchantOrBuyerEmailForBuyerReviewFeedback(review.FK_BranchGuid, review.FK_BusinessGuid);
                    myEmail.EmailMessage.Subject = templatefeedback.Subject;
                    myEmail.CreatedGuid = bringlyEntities.tblUsers.Where(x => x.EmailAddress == myEmail.EmailMessage.EmailFrom).ToList().FirstOrDefault().UserGuid;
                    email.SendReviewEmail(myEmail);
                }
            }

            return GetMyReviewBuyer(myReview.UserGuid);
        }

        /// <summary>
        /// Get all review for the buyer
        /// </summary>
        /// <param name="UserGuid">Buyer user guid</param>
        /// <returns>MyReview</returns>
        public MyReview GetMyReviewBuyer(Guid UserGuid, int LatestPage = 0)
        {
            MyReview myReview = new MyReview();

            myReview.PageSize = PageSize;
            myReview.CurrentPage = LatestPage > 0 ? LatestPage : CurrentPage; ;//ViewBag.CurrentPage

            int orderStatusId = (int)OrderStatus.Completed;
            var restaurantData = (from z in bringlyEntities.tblReviews
                                  join x in bringlyEntities.tblOrders
                                  on z.FK_OrderGuid equals x.OrderGuid
                                  where z.FK_UserGuid == UserGuid && z.IsDeleted == false && z.IsSkipped == false && z.IsReviewed == false
                                  && x.FK_OrderStatusId == orderStatusId
                                  select new MyReview
                                  {
                                      BusinessGuid = z.FK_BusinessGuid,
                                      UserGuid = z.FK_UserGuid,
                                      ReviewGuid = z.ReviewGuid,
                                      BusinessName = z.tblBusiness.BusinessName,
                                      BusinessImage = z.tblBusiness.BusinessImage
                                  }).FirstOrDefault();

            if (restaurantData != null && restaurantData.ReviewGuid != Guid.Empty)
            {
                myReview.BusinessGuid = restaurantData.BusinessGuid;
                myReview.UserGuid = restaurantData.UserGuid;
                myReview.ReviewGuid = restaurantData.ReviewGuid;
                myReview.BusinessName = restaurantData.BusinessName;
                myReview.BusinessImage = CommonDomainLogic.GetCurrentDomain + CommonDomainLogic.GetImagePath(ImageType.Restaurant, restaurantData.BusinessImage);
            }

            myReview.GivenBusinessReviews = bringlyEntities.tblReviews
                .Where(u => u.FK_UserGuid == UserGuid && u.IsDeleted == false
                && u.IsSkipped == false && u.IsReviewed == true
                && u.tblOrder.FK_OrderStatusId == orderStatusId
                )
                .Select(f => new BusinessReview { IsApproved = f.IsApproved.HasValue ? f.IsApproved.Value : false, ReviewGuid = f.ReviewGuid, Review = f.Review, UserGuid = f.FK_UserGuid, BusinessGuid = f.FK_BusinessGuid, DateCreated = f.DateCreated, BusinessName = f.tblBusiness.BusinessName, Rating = f.Rating })
                .OrderByDescending(x => x.DateCreated)
                .ToList();

            myReview.TotalRecords = myReview.GivenBusinessReviews.Count;

            int skip = 0;
            int take = myReview.PageSize;
            if (myReview.CurrentPage == 1)
                skip = 0;
            else
                skip = ((myReview.CurrentPage * myReview.PageSize) - myReview.PageSize);

            myReview.GivenBusinessReviews = myReview.GivenBusinessReviews.Skip(skip).Take(take).ToList();

            return myReview;
        }

        /// <summary>
        /// Get all the review records for the merchant and menagers
        /// Need to modified logic for the manager
        /// </summary>
        /// <param name="myReview"></param>
        /// <returns></returns>
        public MyReview GetMyReviewMerchant(MyReview myReview, int LatestPage = 0)
        {
            myReview.PageSize = PageSize;
            myReview.CurrentPage = CurrentPage;
            myReview.SortBy = SortBy;
            int orderStatusId = (int)OrderStatus.Completed;
           
            myReview.GivenBusinessReviews = bringlyEntities.tblReviews
                .Where(u => !u.IsDeleted && u.FK_CreatedByGuid == myReview.UserGuid && u.IsReviewed == true
                && u.IsSkipped == false && orderStatusId == u.tblOrder.FK_OrderStatusId).Join(
                bringlyEntities.tblUsers,
                r => r.FK_UserGuid,
                usr => usr.UserGuid,
                (r, usr) => new BusinessReview
                {
                    UserName = usr.FullName,
                    //em.tblEmailToes.Where(z=>z.EmailGuid== em.EmailGuid).ToList().Select(a=> new EmailToClass { UserName = a.tblUser.FullName }).ToList()
                    IsProcessed = r.IsProcessed.HasValue ? r.IsProcessed.Value : false,
                    IsApproved = r.IsApproved.HasValue ? r.IsApproved.Value : false,
                    IsReviewed = r.IsReviewed,
                    ReviewGuid = r.ReviewGuid,
                    Review = r.Review,
                    UserGuid = r.FK_UserGuid,
                    DateCreated = r.DateCreated,
                    Rating = r.Rating,
                    BusinessName = r.tblBusiness.BusinessName,
                }).OrderByDescending(x => x.IsApproved).OrderByDescending(x => !x.IsProcessed).ToList();

            myReview.TotalRecords = myReview.GivenBusinessReviews.Count;

            int Skip = 0;
            int Take = myReview.PageSize;
            if (myReview.CurrentPage == 1)
            {
                Skip = 0;
            }
            else
            {
                Skip = ((myReview.CurrentPage * myReview.PageSize) - myReview.PageSize);
            }

            tblReview restaurantReviews = bringlyEntities.tblReviews.Where(u => u.FK_CreatedByGuid == myReview.UserGuid && u.IsDeleted == false).ToList().FirstOrDefault();

            if (restaurantReviews != null && restaurantReviews.ReviewGuid != Guid.Empty)
            {
                myReview.BusinessGuid = restaurantReviews.FK_BusinessGuid;
                myReview.UserGuid = restaurantReviews.FK_UserGuid;
                myReview.ReviewGuid = restaurantReviews.ReviewGuid;
                myReview.BusinessImage = restaurantReviews.tblBusiness.BusinessImage;
                myReview.BusinessName = restaurantReviews.tblBusiness.BusinessName;
            }

            myReview.GivenBusinessReviews = myReview.GivenBusinessReviews.Skip(Skip).Take(Take).ToList();
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

        /// <summary>
        /// Approve the review by the merchant
        /// </summary>
        /// <param name="ReviewGuid">Review guid</param>
        /// <param name="Isapprove">true if approved else false</param>
        /// <returns>Message</returns>
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
            catch (Exception ex)
            {
                var ss = ex;
                message.MessageType = Domain.Enums.MessageType.Error;
                if (Isapprove) message.MessageText = "Error while approving the review";
                else message.MessageText = "Error while rejecting the review";
            }

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAllBuyers()
        {
            List<Contact> UserContact = new List<Contact>();
            UserContact = bringlyEntities.tblUsers.Where(x => x.UserRegistrationType == 2).
                Select(up => new Contact { FullName = up.FullName, EmailAddress = up.EmailAddress, UserGuid = up.UserGuid }).ToList();

            return UserContact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAllMerchants()
        {
            List<Contact> UserContact = new List<Contact>();
           
            var DirectorUserContact = (from usr in bringlyEntities.tblUsers
                                       join bi in bringlyEntities.tblBusinesses on usr.UserGuid equals bi.FK_CreatedByGuid

                                       where usr.UserRegistrationType == 3 && usr.IsDeleted == false && usr.IsActive == true && bi.IsDeleted == false

                                       orderby bi.BusinessName ascending
                                       orderby usr.FullName ascending
                                       select new Contact
                                       {
                                           FullName = bi.BusinessName + " - Director - " + usr.FullName,
                                           EmailAddress = usr.EmailAddress,
                                           UserGuid = usr.UserGuid
                                       }).ToList();

            var ManagerUserContact = bringlyEntities.tblUsers.Where(x => x.IsDeleted == false && x.IsActive == true).OrderBy(x => x.FullName).Join(
                bringlyEntities.tblUsers,
                x => x.FK_ParentGuid,
                z => z.UserGuid,
                (x, z) => new Contact
                {
                    EmailAddress = x.EmailAddress,
                    FullName = bringlyEntities.tblBusinesses.Where(y => y.IsDeleted == false && y.FK_CreatedByGuid == x.FK_ParentGuid).Select(y => y.BusinessName).FirstOrDefault() + " - Manager - " + x.FullName,
                    UserGuid = x.UserGuid
                });

            UserContact.AddRange(DirectorUserContact);
            UserContact.AddRange(ManagerUserContact);

            return UserContact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socialLogin"></param>
        /// <returns>for new user 0, for new unverified user 1 and for verified user 2</returns>
        public Message RegisterUserthroughSociallogin(SocialLogin socialLogin)
        {
            tblUser User = bringlyEntities.tblUsers.Where(x => x.EmailAddress.ToLower() == socialLogin.EmailAddress.ToLower()).FirstOrDefault();
            UserLogin userLogin = new Domain.User.UserLogin();
            if (User == null)
            {
                User = new tblUser();
                User.UserGuid = Guid.NewGuid();
                User.FullName = socialLogin.FullName;
                User.UserRegistrationType = 2;
                User.EmailAddress = socialLogin.EmailAddress;
                User.DateCreated = DateTime.Now;
                User.IsDeleted = false;
                User.IsActive = true;
                User.IsVerified = true;
                User.FK_PreferedCity = bringlyEntities.tblCities.Where(x => x.IsDeleted == false).ToList().FirstOrDefault().CityGuid;
                User.GoogleLoginEmail = socialLogin.IsGoogleLogin ? socialLogin.EmailAddress : "";
                User.FacebookLoginEmail = !socialLogin.IsGoogleLogin ? socialLogin.EmailAddress : "";
                User.GoogleProviderUserId = socialLogin.GoogleProviderUserId;
                User.GoogleUserProfileImageUrl = socialLogin.GoogleUserProfileImageUrl;
                User.FacebookProviderUserId = socialLogin.FacebookProviderUserId;
                User.FacebookUserProfileImageUrl = socialLogin.FacebookUserProfileImageUrl;
                bringlyEntities.tblUsers.Add(User);
                bringlyEntities.SaveChanges();
                userLogin.Username = socialLogin.EmailAddress;
                userLogin.UserPassword = User.Password;
                EmailDomainLogic emailDomainLogic = new EmailDomainLogic();
                emailDomainLogic.SendThankUForSocialSignUpEmail(User.UserGuid);
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

            return UserLogin(userLogin);
        }

        /// <summary>
        /// Facebook social media lofin integration
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string FacebookLogin(Uri uri)
        {
            var fb = new Facebook.FacebookClient();
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

        /// <summary>
        /// Get lsit of cities
        /// </summary>
        /// <returns>List of cities</returns>
        public List<City> GetCities()
        {
            return bringlyEntities.tblCities.Where(c => c.IsDeleted == false).Select(c => new City { CityGuid = c.CityGuid, CityName = c.CityName, CityUrlName = c.CityUrlName }).ToList();
        }

        /// <summary>
        /// Add/update user profile recordss
        /// </summary>
        /// <param name="userProfile">User profile date</param>
        /// <returns>true if sucess else false</returns>
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
                user.FK_ParentGuid = UserVariables.LoggedInUserGuid;
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                user.Password = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                userProfile.UserGuid = user.UserGuid;
                user.DateCreated = DateTime.Now;
                bringlyEntities.tblUsers.Add(user);

                if (userProfile.RoleGuid != null)
                {
                    tblManager tblManager = new tblManager
                    {
                        BranchGuid = Guid.Empty,
                        BusinessGuid = bringlyEntities.tblBusinesses.Where(x => x.FK_CreatedByGuid == UserVariables.LoggedInUserGuid).Select(x => x.BusinessGuid).FirstOrDefault(),
                        CreatedByGuid = UserVariables.LoggedInUserGuid,
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        ManagerGuid = Guid.NewGuid(),
                        UserGuid = user.UserGuid
                    };
                    bringlyEntities.tblManagers.Add(tblManager);
                }
            }

            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserProfile> GetAllUsers()
        {
            List<UserProfile> user = bringlyEntities.tblUsers.Include(a => a.tblUserAddresses).Where(x => x.FK_ParentGuid == UserVariables.LoggedInUserGuid && x.UserRegistrationType == 4 && x.IsDeleted == false)
                .Select(z => new UserProfile
                {
                    UserGuid = z.UserGuid,
                    FullName = z.FullName,
                    RoleGuid = z.RoleGuid.Value,
                    RoleName = bringlyEntities.tblRoles.Where(x => x.RoleGuid == z.RoleGuid.Value).FirstOrDefault().RoleName
                }).ToList();

            return user;
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="userGuid">User guid to be deleted</param>
        /// <returns>true if deleted else false</returns>
        public bool DeleteUser(Guid userGuid)
        {
            tblUser user = bringlyEntities.tblUsers.Where(x => x.UserGuid == userGuid).FirstOrDefault();
            user.IsDeleted = true;
            bringlyEntities.SaveChanges();
            return true;
        }

        /// <summary>
        /// Verify the user
        /// </summary>
        /// <param name="guid">User guid to be verify</param>
        /// <returns>true if sucess else false</returns>
        public bool VerifyUser(Guid guid)
        {
            if (guid == Guid.Empty)
                return false;
            tblUser tblUser = bringlyEntities.tblUsers.Where(usr => usr.IsDeleted == false && usr.UserGuid == guid).FirstOrDefault();
            if (tblUser == null)
                return false;

            if (tblUser.IsVerified != null && tblUser.IsVerified == true)
                return false;

            tblUser.IsVerified = true;
            tblUser.VerificationDate = DateTime.Now;
            string userIp = string.Empty;
            UtilityHelper.GetIpValue(out userIp);
            tblUser.IPAddress = userIp;
            bringlyEntities.SaveChanges();

            EmailDomainLogic emailDomainLogic = new EmailDomainLogic();
            emailDomainLogic.SendThankUForVerificationEmail(guid);

            return true;
        }

        /// <summary>
        /// Create user password
        /// </summary>
        /// <param name="guid">User guid to whom password be created</param>
        /// <param name="password">User password</param>
        /// <returns>0 if user is not found else UserRegistrationType</returns>
        public int CreateUserPassword(Guid guid, string password)
        {
            var user = bringlyEntities.tblUsers.Where(usr => usr.IsDeleted == false && usr.IsVerified == true && usr.UserGuid == guid).FirstOrDefault();
            if (user == null)
                return 0;

            user.Password = EncryptionHelper.EncryptText(password, user.EmailAddress);
            user.IsActive = true;
            bringlyEntities.SaveChanges();
            return user.UserRegistrationType;
        }

        /// <summary>
        /// Get messages for the user
        /// </summary>
        /// <param name="UserGuid">User guid to whom fetch the records</param>
        /// <returns>List of MyMessage</returns>
        public List<MyMessage> GetMyMessages(Guid UserGuid)
        {
            var messages = (from email in bringlyEntities.tblEmails

                            join emailTo in bringlyEntities.tblEmailToes on email.EmailGuid equals emailTo.FK_EmailGuid

                            join usr in bringlyEntities.tblUsers on email.FK_CreatedByGuid equals usr.UserGuid

                            where emailTo.FK_UserGuid == UserGuid && email.IsDeleted == false && emailTo.IsDeleted == false

                            orderby email.DateCreated descending

                            select new MyMessage
                            {
                                Body = email.Body,
                                CreatedByGuid = email.FK_CreatedByGuid,
                                EmailFrom = email.EmailFrom,
                                EmailGuid = email.EmailGuid,
                                EmailToGuid = emailTo.EmailToGuid,
                                FromName = usr.FullName,
                                Subject = email.Subject,
                                EmailFromImagePath = !string.IsNullOrEmpty(usr.ImageName) ? usr.ImageName : !string.IsNullOrEmpty(usr.GoogleUserProfileImageUrl) ? usr.GoogleUserProfileImageUrl : !string.IsNullOrEmpty(usr.FacebookUserProfileImageUrl) ? usr.FacebookUserProfileImageUrl : null,
                                DateCreated = email.DateCreated
                            }).Take(2);
            var te = messages.ToList();
            return te;
        }

        /// <summary>
        /// Login with encrypted password
        /// </summary>
        /// <param name="userLogin">User name and password</param>
        /// <param name="IsNewUser"></param>
        /// <returns>Message</returns>
        public Message UserLoginWithHash(UserLogin userLogin, bool IsNewUser = false)
        {
            Message message = new Message();
            tblUser user = bringlyEntities.tblUsers.Where(u => u.EmailAddress == userLogin.Username && u.IsDeleted == false && u.IsVerified == true).FirstOrDefault();
            if (user != null && user.IsActive)
            {
                //var decryptedUserPasswordA = EncryptionHelper.EncryptText(userLogin.UserPassword, userLogin.Username);
                //var fulNAme = "Bringly Website";
                var decryptedUserPassword = EncryptionHelper.DecryptText(user.Password, user.EmailAddress);

                if (decryptedUserPassword != null && decryptedUserPassword.Equals(userLogin.UserPassword))
                {
                    if (user.UserRegistrationType == 3)
                    {
                        if (user.IsAdminApproved == true)
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
                        else
                        {
                            message.MessageType = Domain.Enums.MessageType.Error;
                            message.MessageText = "Account is not activated yet. Please contact admin.";
                        }
                    }
                    else
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
                }
                else
                {
                    message.MessageType = Domain.Enums.MessageType.Error;
                    message.MessageText = "Wrong password";
                }
            }
            else if (user != null && !user.IsActive)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public bool IsSelfDeliveryActive(Guid merchantGuid)
        {
            var business = bringlyEntities.tblBusinesses.Where(busi => busi.IsDeleted == false && busi.FK_CreatedByGuid == merchantGuid).Select(bus => bus).FirstOrDefault();

            if (business == null)
                return false;

            return business.IsSelfDelivery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="isSelfDelivery"></param>
        /// <returns></returns>
        public bool SetSelfDeliveryForMerchant(Guid merchantGuid, bool isSelfDelivery)
        {
            var business = bringlyEntities.tblBusinesses.Where(busi => busi.IsDeleted == false && busi.FK_CreatedByGuid == merchantGuid).FirstOrDefault();

            if (business == null)
                return false;

            business.IsSelfDelivery = isSelfDelivery;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cPw"></param>
        /// <param name="nPw"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public int ChangePassword(string cPw, string nPw, Guid userGuid)
        {
            if (cPw == null || nPw == null)
                return 0;

            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == userGuid && usr.IsDeleted == false && usr.IsActive == true).FirstOrDefault();
            if (user == null)
                return 0;

            var dbDecryptedPassword = EncryptionHelper.DecryptText(user.Password, user.EmailAddress);

            if (string.IsNullOrEmpty(dbDecryptedPassword))
                return 1;

            if (!cPw.Equals(dbDecryptedPassword, StringComparison.CurrentCulture))
                return 1;

            user.Password = EncryptionHelper.EncryptText(nPw, user.EmailAddress);
            bringlyEntities.SaveChanges();

            return 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public bool DeActivateAccount(string password, Guid userGuid)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == userGuid && usr.IsDeleted == false && usr.IsActive == true).FirstOrDefault();
            if (user == null)
                return false;

            var dbDecryptedPassword = EncryptionHelper.DecryptText(user.Password, user.EmailAddress);

            if (string.IsNullOrEmpty(dbDecryptedPassword))
                return false;

            if (!password.Equals(dbDecryptedPassword, StringComparison.CurrentCulture))
                return false;

            user.IsActive = false;
            bringlyEntities.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public bool DeleteAccount(string password, Guid userGuid)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            var user = bringlyEntities.tblUsers.Where(usr => usr.UserGuid == userGuid && usr.IsDeleted == false).FirstOrDefault();
            if (user == null)
                return false;

            var dbDecryptedPassword = EncryptionHelper.DecryptText(user.Password, user.EmailAddress);

            if (string.IsNullOrEmpty(dbDecryptedPassword))
                return false;

            if (!password.Equals(dbDecryptedPassword, StringComparison.CurrentCulture))
                return false;

            user.IsDeleted = true;
            user.DeletedBy = UserVariables.LoggedInUserGuid;
            bringlyEntities.SaveChanges();

            return true;
        }
    }
}