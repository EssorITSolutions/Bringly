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
                userProfile.PreferedCity = User.PreferedCity!=null? User.PreferedCity.ToString():"";
                userProfile.ProfileImage = string.IsNullOrEmpty(User.ImageName)?CommonDomainLogic.DefaultProfileImage:User.ImageName;
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

        public bool AddFavourite(Guid restaurantGuid,string IsFavourite)
        {
            tblFavourite userFavourite = bringlyEntities.tblFavourites.Where(f => f.RestaurantGuid == restaurantGuid && f.CreatedByGuid == UserVariables.LoggedInUserGuid).FirstOrDefault();
            if (userFavourite == null)
            {
                bringlyEntities.tblFavourites.Add(new tblFavourite { FavouriteGuid = Guid.NewGuid(), RestaurantGuid = restaurantGuid, CreatedByGuid = UserVariables.LoggedInUserGuid, DateCreated = DateTime.Now });
                bringlyEntities.SaveChanges();
            }
            return true;
        }

        public bool RemoveFavourite(Guid restaurantGuid,string IsFavourite)
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
                    imageName = Path.GetFileName("Buyer_" + Guid.NewGuid() + "_" + Path.GetExtension(Request.Files[i].FileName));
                     imageLocation = CommonDomainLogic.GetImagePath(Domain.Enums.ImageType.User, imageName);
                    Request.Files[i].SaveAs(HttpContext.Current.Server.MapPath(imageLocation));
                }
                userProfile.ProfileImage = imageName;
                user.ImageName = userProfile.ProfileImage;
                bringlyEntities.SaveChanges();
                return imageLocation;
            }
            else {
                return imageLocation;
            }
            
        }

    }
}