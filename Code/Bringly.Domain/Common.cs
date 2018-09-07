using Bringly.Domain.Enums;

namespace Bringly.Domain.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageType MessageType { get; set; }
        public string MessageText { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Template
    {
        /// <summary>
        /// 
        /// </summary>
        public TemplateType TemplateType { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Unauthorize
    {
        public string ReturnUrl { get; set; }
    }

}
