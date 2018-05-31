using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using Microsoft.Web.WebPages.OAuth;

namespace OAuth
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            GoogleOAuth2Client clientGoog = new GoogleOAuth2Client(ConfigurationManager.AppSettings["GoogleId"], ConfigurationManager.AppSettings["GoogleKey"]);
            IDictionary<string, string> extraData = new Dictionary<string, string>();            
            OpenAuth.AuthenticationClients.Add("google", () => clientGoog, extraData);

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["AppId"],
                appSecret: ConfigurationManager.AppSettings["AppSecret"]
                );
        }
        
    }
    //public class FacebookCredential
    //{
    //    public string appId { get; } = "177937239517445";
    //    public string appSecret { get; } = "501b525acc2a454d7002b5368513ec40";
    //}
}
