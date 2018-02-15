using Elmah;
using System;
using System.Text;
using System.Web;

namespace Utilities
{
    /// <summary>
    ///  ErrorLog.LogError(ex, "Error sending email for order " + orderID);
    /// </summary>
    public static class ErrorLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="contextualMessage"></param>
        /// <param name="eventName"></param>
        public static void LogError(Exception ex, string contextualMessage = "", string eventName = "")
        {
            try
            {
                var sb = new StringBuilder();
                sb.Clear();
                sb.Append(Environment.NewLine);
                sb.Append("Contextual Message : " + contextualMessage);
                sb.Append(Environment.NewLine);
                sb.Append("Event Name : " + eventName);

                Exception annotatedException = null;
                if (ex != null)
                {
                    annotatedException = new Exception(sb.ToString(), ex);
                }
                else
                {
                    new Exception(sb.ToString());
                }

                ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
            }
            catch (Exception)
            {
                // uh oh! just keep going
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextualMessage"></param>
        /// <param name="eventName"></param>
        public static void LogError(string contextualMessage, string eventName)
        {
            LogError(null, contextualMessage: contextualMessage, eventName: eventName);
        }

    }
}
