using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Utilities
{
    public class Conversions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string DictionaryToJson(Dictionary<string, string> dict)
        {

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(dict);
        }
    }
}
