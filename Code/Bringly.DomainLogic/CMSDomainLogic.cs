using Bringly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bringly.DomainLogic
{
    public class CMSDomainLogic : BaseClass.DomainLogicBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CMSPage> GetCMSPages()
        {
            return bringlyEntities.tblPages
                .Where(p => p.IsActive == true && p.IsDeleted == false)
                .Select(p => new CMSPage
                {
                    PageAlias = p.PageAlieas,
                    PageDescription = p.PageDecription,
                    PageGuid = p.PageGuid,
                    PageId = p.PageId,
                    PageName = p.PageName,
                    Priority = p.Priority
                }).OrderBy(x => x.Priority).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public CMSPage GetCMSPageByAlias(string alias)
        {
            return bringlyEntities.tblPages
                .Where(p => p.IsActive == true && p.IsDeleted == false && p.PageAlieas != null && p.PageAlieas.Equals(alias, StringComparison.OrdinalIgnoreCase))
                .Select(p => new CMSPage
                {
                    PageAlias = p.PageAlieas,
                    PageDescription = p.PageDecription,
                    PageGuid = p.PageGuid,
                    PageId = p.PageId,
                    PageName = p.PageName,
                    Priority = p.Priority
                }).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public CMSPage GetCmsPageContent(string alias)
        {
            return bringlyEntities.tblPages
                .Where(p => p.IsActive == true && p.IsDeleted == false && p.PageAlieas != null && p.PageAlieas.Equals(alias, StringComparison.OrdinalIgnoreCase))
                .Select(p => new CMSPage
                {
                    PageAlias = p.PageAlieas,
                    PageDescription = p.PageDecription,
                    PageGuid = p.PageGuid,
                    PageId = p.PageId,
                    PageName = p.PageName,
                    Priority = p.Priority
                }).FirstOrDefault();
        }

    }
}
