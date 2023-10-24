using MongoDB.Driver;
using  ScheduleJob.Data.Interfaces;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Repository.Entity;
using  ScheduleJob.Repository.Interface;

namespace  ScheduleJob.Repository.Repository
{
    /// <summary>
    /// Class for user related repository operations.
    /// </summary>
    public class UserRepository : EntityRepository<User, IEntityProvider<User>>, IUserRepository
    {
        private readonly IEntityProvider<User> _userProvider;

        /// <summary>
        /// Constructor for User repository.
        /// </summary>
        /// <param name="userProvider"></param>
        public UserRepository(IEntityProvider<User> userProvider) : base(userProvider)
        {
            this._userProvider = userProvider;
        }

        /// <summary>
        /// Get user details from firstname.
        /// </summary>
        /// <param name="activeDirectoryId">Auzre active directory unique identifier</param>
        /// <returns>User docuemnt model</returns>
        public async Task<User?> GetUser(string activeDirectoryId)
        {
            var filter = Builders<User>.Filter.And(
                         Builders<User>.Filter.Eq(x => x.ActiveDirectoryId, activeDirectoryId)
                         );
            var result= await this._userProvider.Get(filter);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Updates a portion of the document in collection.
        /// </summary>
        /// <param name="entityModel">model containing the column for partial update.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public async Task<UpdateResult> PartialUpdate(string jsonString, string id)
        {
            return await this._userProvider.PartialUpdate(jsonString, id);
        }

    }
}