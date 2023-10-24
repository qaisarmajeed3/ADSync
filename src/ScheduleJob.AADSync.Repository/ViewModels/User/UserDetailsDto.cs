using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SHC.ScheduleJob.Domain.Models;
using SHC.ScheduleJob.Domain.Enum;

namespace SHC.ScheduleJob.Repository.ViewModels
{
    /// <summary>
    /// Class for mapping user details data from db.
    /// </summary>
    public class UserDetailsDto
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
        /// Gets or sets title of user.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets display name of user.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets emailid of user.
        /// </summary>
        public string? EmailId { get; set; }

        /// <summary>
        /// Gets or sets status of user.
        /// </summary>
        public UserStatus? Status { get; set; }

        /// <summary>
        /// gets or sets work phone number of user.
        /// </summary>
        public string? WorkPhoneNumber { get; set; }

        /// <summary>
        /// gets or sets alternative phone number of user.
        /// </summary>
        public string? AssistantPhoneNumber { get; set; }

        /// <summary>
        /// gets or sets fax number of user.
        /// </summary>
        public string? FaxNumber { get; set; }

        /// <summary>
        /// gets or sets address of user.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// gets or sets city of user address.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// gets or sets state of user.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// gets or sets zip code of user address.
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// Gets or sets users organizations.
        /// </summary>
        public List<Facility>? Organization { get; set; }

        /// <summary>
        /// Gets or sets role details.
        /// </summary>
        public List<RoleDetailsDto>? RoleDetails { get; set; }

        /// <summary>
        /// Gets or sets user assigned roles.
        /// </summary>
        public List<Roles>? Roles { get; set; }
    }
}
