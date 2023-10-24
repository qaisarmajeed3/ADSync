using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace  ScheduleJob.Service.Models
{
    /// <summary>
    /// Class for defining feilds of Task
    /// </summary>
    public class TaskModel:BaseEntity
    {
        /// <summary>
        /// Gets or sets Id.s
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // <summary>
        /// Gets or sets Description field
        /// </summary>
        public string? Description { get; set; }
    }
    
}
