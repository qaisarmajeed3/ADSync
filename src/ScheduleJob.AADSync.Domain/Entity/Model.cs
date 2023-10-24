using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using  ScheduleJob.Domain.Interface;

namespace  ScheduleJob.Domain.Entity
{
    /// <summary>
    /// Base Model class for response.
    /// </summary>
    public class Model : IModel
    {
        private DateTime? _createdDate = null;
        private DateTime? _modifiedDate = null;

        /// <summary>
        /// Gets or sets created date.
        /// </summary>
        public DateTime? CreatedDate
        {
            get { return this._createdDate ?? DateTime.UtcNow; }
            set { this._createdDate = value; }
        }

        /// <summary>
        /// Gets or sets modified date.
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return this._modifiedDate ?? DateTime.UtcNow; }
            set { this._modifiedDate = value; }
        }

        // Gets or sets createdBy.
        public string? CreatedBy { get; set; }

        // Gets or sets modifiedBy.
        public string? ModifiedBy { get; set; }

    }
}
