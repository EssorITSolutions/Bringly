
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
namespace Utilities.EmailSender.Domain
{
    /// <summary>
    /// how to use
    /// var list = new ListWithDuplicates();
    /// list.Add("k1", "v1");
    /// list.Add("k1", "v2");
    /// list.Add("k1", "v3");
    /// </summary>
    public class ListOfAttachments : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            Add(element);
        }
    }

    /// <summary>
    /// how to use
    /// var list = new ListOfMultipleEmailTo();
    /// list.Add("k1", "v1");
    /// list.Add("k1", "v2");
    /// list.Add("k1", "v3");
    /// </summary>
    public class ListOfMultipleEmailTo : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            Add(element);
        }
    }

    /// <summary>
    /// how to use
    /// var list = new ListOfMultipleEmailCC();
    /// list.Add("k1", "v1");
    /// list.Add("k1", "v2");
    /// list.Add("k1", "v3");
    /// </summary>
    public class ListOfMultipleEmailCC : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            Add(element);
        }
    }



    public enum EmailMailPriority
    {
        Normal,
        Low,
        High
    }
    public class EmailDomain
    {
        bool _hasNetworkCredential, _enableSsl, _canSendEmail;
        string _networkCredentialUserName = "", _networkCredentialPassword = "", _host = "localhost";
        int _port = 25;
        public EmailDomain()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["HasNetworkCredential"]))
            {
                _hasNetworkCredential = Convert.ToBoolean(ConfigurationManager.AppSettings["HasNetworkCredential"]);
            }
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EnableSsl"]))
            {
                _enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            }
            _canSendEmail = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CanSendEmail"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["CanSendEmail"]);
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NetworkCredential_UserName"]))
            {
                _networkCredentialUserName = Convert.ToString(ConfigurationManager.AppSettings["NetworkCredential_UserName"]);
            }
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NetworkCredential_Password"]))
            {
                _networkCredentialPassword = Convert.ToString(ConfigurationManager.AppSettings["NetworkCredential_Password"]);
            }
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Port"]))
            {
                _port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            }
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Host"]))
            {
                _host = Convert.ToString(ConfigurationManager.AppSettings["Host"]);
            }
            ListOfAttachments = new ListOfAttachments();
            ListOfMultpleEmailTo = new ListOfMultipleEmailTo();
            ListOfMultipleEmailCc = new ListOfMultipleEmailCC();
        }

        public string EmailBody { get; set; }

        public string EmailTo { get; set; }
        public string EmailToName { get; set; }

        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }

        public string EmailCc { get; set; }
        public string EmailCcName { get; set; }

        public string EmailBcc { get; set; }
        public string EmailBccName { get; set; }

        public string EmailReply { get; set; }
        public string EmailReplyName { get; set; }

        public string EmailSubject { get; set; }

        public EmailMailPriority? MailPriority { get; set; }

        public bool IsEmailSent { get; internal set; }
        public string EmailNotSentError { get; internal set; }

        /// <summary>
        /// if not set then Default True
        /// </summary>
        public bool? IsBodyHtml { get; set; }
        /// <summary>
        /// create a AppSetting in web.config file with name 'Port'
        /// </summary>
        public int Port
        {
            //get
            //{
            //    try
            //    {
            //        return Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            //    }
            //    catch { return 25; }
            //}
            get { return _port; }
            set { _port = value; }

        }
        /// <summary>
        /// create a AppSetting in web.config file with name 'Host'
        /// </summary>
        public string Host
        {
            //get
            //{
            //    try
            //    {
            //        return Convert.ToString(ConfigurationManager.AppSettings["Host"]);
            //    }
            //    catch { return "localhost"; }
            //}
            get { return _host; }
            set { _host = value; }
        }
        /// <summary>
        /// Tells User will pass the Network Credentials or not. 
        /// Can Also Create a AppSetting in web.config file with name 'HasNetworkCredential'
        /// </summary>
        public bool HasNetworkCredential { get { return _hasNetworkCredential; } set { _hasNetworkCredential = value; } }
        /// <summary>
        /// User name for Network Credential 
        /// Can Also Create a AppSetting in web.config file with name 'NetworkCredential_UserName'
        /// </summary>
        public string NetworkCredentialUserName { get { return _networkCredentialUserName; } set { _networkCredentialUserName = value; } }
        /// <summary>
        /// Password for Network Credential 
        /// Can Also Create a AppSetting in web.config file with name 'NetworkCredential_Password'
        /// </summary>
        public string NetworkCredentialPassword { get { return _networkCredentialPassword; } set { _networkCredentialPassword = value; } }
        /// <summary>
        /// Tells is SSL needs to be used. Default false
        /// Can Also Create a AppSetting in web.config file with name 'EnableSsl'
        /// </summary>
        public bool EnableSsl { get { return _enableSsl; } set { _enableSsl = value; } }

        /// <summary>
        /// Key contains the Name of File to be Display and Value is the Full path of the File
        /// </summary>
        public Hashtable Attachments { get; set; }
        /// <summary>
        /// used to The that can email will be sent or not
        /// Can Also Create a AppSetting in web.config file with name 'CanSendEmail' .. If Set To False .. Email will be not send
        /// </summary>
        public bool CanSendEmail { get { return _canSendEmail; } set { _canSendEmail = value; } }
        /// <summary>
        /// used For sending attachments. This is speciall used if Attached File display name can be same
        /// Key contains the Name of File to be Display and Value is the Full path of the File
        /// </summary>
        public ListOfAttachments ListOfAttachments { get; set; }
        /// <summary>
        ///  key is email to 
        ///  value is Email TO Name
        /// </summary>
        public ListOfMultipleEmailTo ListOfMultpleEmailTo { get; set; }
        /// <summary>
        ///  key is email to 
        ///  value is Email TO Name
        /// </summary>
        public ListOfMultipleEmailCC ListOfMultipleEmailCc { get; set; }

    }
}
