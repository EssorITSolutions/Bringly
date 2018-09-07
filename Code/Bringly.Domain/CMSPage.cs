using System;

namespace Bringly.Domain
{
    public class CMSPage
    {
        public int PageId { get; set; }
        public Guid PageGuid { get; set; }
        public string PageName { get; set; }
        [AllowHtml]
        public string PageDescription { get; set; }
        public int Priority { get; set; }
        public string PageAlias { get; set; }
    }
}
