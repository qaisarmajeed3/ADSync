using System;

namespace  ScheduleJob.Domain.Interface
{
    /// <summary>
    /// Interface for handling base entity.
    /// </summary>
    public interface IEntity : IModel
    {
        // Returns Id in the collection.
        string? Id { get; set; }
    }
}
