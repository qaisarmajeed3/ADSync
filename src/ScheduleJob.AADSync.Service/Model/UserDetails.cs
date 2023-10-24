using  ScheduleJob.Domain.Enum;

namespace  ScheduleJob.Service.Model
{
    /// <summary>
    /// Class for user details response.
    /// </summary>
    /// <summary>
    /// Class for user details along with roles and facilities.
    /// </summary>
    public class UserDetails
    {
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets first name of user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets display name of user.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets title of user.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets user status.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets primary facility id of user.
        /// </summary>
        public string? PrimaryFacilityId { get; set; }

        /// <summary>
        /// Gets or sets is primary or not with values Yes,No and N/A
        /// </summary>
        public string? IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets work phone number of user.
        /// </summary>
        public string? WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets assistant phone number of user.
        /// </summary>
        public string? AssistantPhone { get; set; }

        /// <summary>
        /// Gets or sets fax number of user.
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// Gets or sets email id of user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets address of user.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// gets or sets Line 1 of address of user.
        /// </summary>
        public string? AddressLine1 { get; set; }

        /// <summary>
        /// gets or sets Line 2 of user.
        /// </summary>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets city of user.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets state of user.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets zip code of user.
        /// </summary>
        public string? Zip { get; set; }

        /// <summary>
        /// Get or sets the active directory Id
        /// </summary>
        public string? ActiveDirectoryId { get; set; }

        /// <summary>
        /// Gets or sets assigned facilities of user.
        /// </summary>
        public List<string>? Facilities { get; set; }

        
    }
}