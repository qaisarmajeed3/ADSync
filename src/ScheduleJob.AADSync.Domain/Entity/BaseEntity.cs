using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using  ScheduleJob.Domain.Interface;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace  ScheduleJob.Domain.Entity
{
    /// <summary>
    /// Class that defines the base entity.
    /// </summary>
    public class BaseEntity : Model, IEntity
    {
        /// <summary>
        /// Gets or sets the id in a collection.
        /// </summary>
        static BaseEntity()
        {
        }
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = default!;
    }

    /// <summary>
    /// A record of information about reference entity.
    /// </summary>
    public class BaseReferenceEntity
    {
        /// <summary>
        /// gets or sets reference.
        /// </summary>
        public string? Reference { get; set; }
        /// <summary>
        /// gets or sets type.
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// gets or sets identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Identifier { get; set; } = default!;

    }
}
