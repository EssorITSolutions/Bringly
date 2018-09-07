namespace Bringly.Domain
{
    /// <summary>
    /// Wrapper for the query string parameters for searching, sorting, and paging
    /// </summary>
    public class RequestQuery
    {
        /// <summary>
        /// Search parameter
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Current page size
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// Sort by column name
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Sort order by(ascending or descending)
        /// </summary>
        public string SortOrder { get; set; } = "asc";
    }
}
