using  ScheduleJob.Domain.Models;
using  ScheduleJob.Service.Model;

namespace  ScheduleJob.Service.Interface
{
    /// <summary>
    /// Interface for defining user services.
    /// </summary>
    public interface IUserService : IEntityService<User>
    {
        
        /// <summary>
        /// Insert or update exisitng document
        /// </summary>
        /// <param name="instance">Instance of user document</param>
        /// <returns></returns>
        Task Upsert(User user);

        /// <summary>
        /// Execute the job to import user from Azure AD
        /// </summary>
        /// <returns></returns>
        Task ExecuteJobAsync();
    }
}