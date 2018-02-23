using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bringly.Domain
{
    public class Paging
    {
        /// <summary>
        /// gives Current page of the paging
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// From where the paging starts
        /// </summary>
        public int FirstIndex { get; set; }
        /// <summary>
        /// From where the paging ends
        /// </summary>
        public int LastIndex { get; set; }
        /// <summary>
        /// gives the last page of the paging
        /// </summary>
        public int LastPage { get; set; }
        /// <summary>
        /// Contains Other Query String in URL
        /// </summary>
        public string QueryString { get; set; }
        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalRecords { get; set; }
               /// <summary>
        /// Used to get Sort by
        /// </summary>
        public string SortBy
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_sortBy) ? _sortBy.ToLower() : _sortBy;
            }
            set { _sortBy = value; }
        }

        private string _sortBy = string.Empty;


    }
}
