namespace  ScheduleJob.Service.Model
{
    /// <summary>
    /// Generic class for paged results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T> where T : class
    {
        /// <summary>
        /// Gets or sets data list.
        /// </summary>
        public List<T>? ResultList { get; set; }

        /// <summary>
        /// Gets or sets number of total records retrieved.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total pages for data list.
        /// </summary>
        public int PageNumber { get; set; }
    }
}