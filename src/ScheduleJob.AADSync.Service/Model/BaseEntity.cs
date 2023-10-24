
namespace  ScheduleJob.Service.Models
{
    public class BaseEntity
    {
        private DateTime? _createdDate = null;
        private DateTime? _modifiedDate = null;

        // <summary>
        /// Gets or sets createdDate field of entity
        /// </summary>
        public DateTime CreatedDate
        {
            get { return this._createdDate ?? DateTime.UtcNow; }
            set { this._createdDate = value; }
        }

        // <summary>
        /// Gets or sets modifiedDate field of entity
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return this._modifiedDate ?? DateTime.UtcNow; }
            set { this._modifiedDate = value; }
        }

        // <summary>
        /// Gets or sets createdBy field of entity
        /// </summary>
        public string? CreatedBy { get; set; } = default!;

        // <summary>
        /// Gets or sets modifiedBy field of entity
        /// </summary>
        public string? ModifiedBy { get; set; } = default!;

        // <summary>
        /// Gets or sets IsDeleted field of entity
        /// </summary>
        public bool? IsDeleted { get; set; }


    }
}
