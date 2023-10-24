using System;

namespace  ScheduleJob.Domain.Interface
{
    /// <summary>
    /// Interface for base model entities.
    /// </summary>
    public interface IModel
    {
        // Returns created date.
        DateTime? CreatedDate { get; set; }

        // Returns modified date.
        DateTime ModifiedDate { get; set; }

        //Returns created by.
        string? CreatedBy { get; set; }

        // Return modified by.
        string? ModifiedBy { get; set; }
    }
}
