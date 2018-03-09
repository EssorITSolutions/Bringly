using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain.Enums
{
    public enum ImageType
    {
        User,
        Restaurant,
        Item,
        Default
    }
    public enum MessageType
    {
        Success, Error, Info
    }
    public enum TemplateType
    {
        Review, Mail,FeedBack,Other
    }
    public enum OrderStatus
    {
        Incomplete,Inprogress,completed,Cancelled
    }

}
