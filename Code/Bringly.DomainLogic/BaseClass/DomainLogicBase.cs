using Bringly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.DomainLogic.BaseClass
{
    public abstract class DomainLogicBase
    {
       public BringlyEntities bringlyEntities = new BringlyEntities();
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="saltValue"></param>
        ///// <returns></returns>
        //protected string EncryptText(string text, string saltValue)
        //{
        //    return Utilities.Encryption.EncryptText(text, saltValue, Settings.CvGatewaySettings.PasswordPassPhrase, Settings.CvGatewaySettings.PasswordIterations, Settings.CvGatewaySettings.PasswordInitVector);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="saltValue"></param>
        ///// <returns></returns>
        //protected string DecryptText(string text, string saltValue)
        //{
        //    return Utilities.Encryption.DecryptText(text, saltValue, Settings.CvGatewaySettings.PasswordPassPhrase, Settings.CvGatewaySettings.PasswordIterations, Settings.CvGatewaySettings.PasswordInitVector);
        //}
        /// <summary>
        /// 
        /// </summary>
        protected string CurrentDomain
        {
            get
            {
                var current = System.Web.HttpContext.Current;
                if (current.Request.IsLocal)
                {
                    var uri = current.Request.Url;
                    return uri.Scheme + "://" + uri.Host + ":" + uri.Port + "/";
                }
                return current.Request.Url.Host.ToLower();
            }
        }
    }
}
