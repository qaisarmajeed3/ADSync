using  ScheduleJob.Service.Models;

namespace  ScheduleJob.Service.Interface
{
    /// <summary>
    /// Interface for task service operations.
    /// </summary>
    public interface ITaskService
    {
        
        /// <summary>
        /// Execute the job to create new email work item from email exchange.
        /// </summary>
        /// <returns></returns>
        Task ExecuteJobAsync(string token);
    }
}
