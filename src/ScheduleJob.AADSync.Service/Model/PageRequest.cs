namespace  ScheduleJob.Service.Model
{
    /// <summary>
    /// Mapping class for pagination,sort and search data request.
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// Maximum items in a page.
        /// </summary>
        const int maxPageSize = 100;

        /// <summary>
        /// Gets or sets page number.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// Gets or sets search token.
        /// </summary>
        public string SearchToken { get; set; } = "";

        /// <summary>
        /// Gets or sets sort by details.
        /// </summary>
        public string SortBy { get; set; } = "";

        /// <summary>
        /// Gets or sets order by details.
        /// </summary>
        public string Order { get; set; } = "";

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}