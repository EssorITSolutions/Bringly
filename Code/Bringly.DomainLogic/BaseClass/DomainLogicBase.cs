using Bringly.Data;
using Utilities;

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

        #region Paging Properties

        /// <summary>
        /// returns the current page
        /// </summary>
        protected int CurrentPage
        {
            get
            {
                int currentPage;
                if (!QueryStringHelper.getIntValue("page", out currentPage))
                {
                    currentPage = 1;
                }
                return currentPage;
            }
        }

        /// <summary>
        /// default page size - SetUp in web.config - AppSetting Name : DefaultPageSize
        /// </summary>
        protected int PageSize
        {
            get
            {
                return Settings.SystemSettings.DefaultPageSize;
            }
        }

        protected int PageSizeBusiness
        {
            get
            {
                return Settings.SystemSettings.DefaultPageSizeBusiness;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected string SortBy
        {
            get
            {
                string outPut = QueryStringHelper.getQueryStringVaue("SortBy");
                if (string.IsNullOrWhiteSpace(outPut) || string.IsNullOrEmpty(outPut))
                {
                    return string.Empty;
                }
                return outPut;
            }
        }
    }
}
