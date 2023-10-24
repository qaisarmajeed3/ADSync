namespace SHC.ScheduleJob.Repository.ViewModels
{
    /// <summary>
    /// Class for mapping user details response from db.
    /// </summary>
    public class UserDetailsListDto
    {
        /// <summary>
        /// Gets or sets user details from database.
        /// </summary>
        public List<UserDetailsDto>? UserDetails { get; set; }

        /// <summary>
        /// Gets or sets total record count from database.
        /// </summary>
        public List<TotalRecords>? TotalRecords { get; set; }
    }
}