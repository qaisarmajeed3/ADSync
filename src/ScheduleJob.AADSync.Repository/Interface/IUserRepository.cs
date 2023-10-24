using MongoDB.Driver;
using  ScheduleJob.Domain.Models;

namespace  ScheduleJob.Repository.Interface
{
    /// <summary>
    /// Interface for defining user repository.
    /// </summary>
    public interface IUserRepository : IEntityRepository<User>
    {
        /// <summary>
        /// Get user details from firstname.
        /// </summary>
        /// <param name="activeDirectoryId">Unique identifier of azure active directory</param>
        /// <returns>User document model</returns>
        public Task<User?> GetUser(string activeDirectoryId);
        /// <summary>
        /// Updates a portion of the document in collection.
        /// </summary>
        /// <param name="jsonString">JSON string containing the column for partial update.</param>
        /// <param name="id">Unique identifier of document to be updated</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public Task<UpdateResult> PartialUpdate(string jsonString, string id);

    }
}