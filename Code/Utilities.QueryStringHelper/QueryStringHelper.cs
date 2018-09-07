using System;
using System.Web;
namespace Utilities
{
    public static class QueryStringHelper
    {
        public static string getQueryStringVaue(string keyName)
        {
            return HttpContext.Current.Request.QueryString[keyName];
        }

        static bool isNumeric(string val)
        {
            Double result;
            return Double.TryParse(val, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        /// <summary>
        /// used to get Interger Value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>If no integer value or query string not exists then returns False</returns>
        public static bool getIntValue(string key, out int value)
        {
            value = 0;
            string data = getQueryStringVaue(key);
            if (data == "") return false;
            if (!isNumeric(data)) return false;
            value = Convert.ToInt32(data); return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int getIntValue(string key)
        {            
            return Convert.ToInt32(getQueryStringVaue(key));
            
        }

        /// <summary>
        /// used to get Interger Value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>If no integer value or query string not exists then returns False</returns>
        public static int? getNullIntValue(string key)
        {
            string data = getQueryStringVaue(key);
            if (data == "") return null;
            if (isNumeric(data)) { return Convert.ToInt32(data); }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool getStringValue(string key, out string value)
        {
            value = string.Empty;
            string data = getQueryStringVaue(key);
            if (data == "") return false;
            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data)) return false;
            value = data; return true;
        }

        /// <summary>
        /// get query string value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object getValue(string key)
        {
            return getQueryStringVaue(key);
        }

    }
}
