using  ScheduleJob.Data.Interfaces;
using  ScheduleJob.Domain.Interface;
using  ScheduleJob.Repository.Interface;

namespace  ScheduleJob.Repository.Entity
{
    /// <summary>
    /// Class for handling base repository operations.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="MDE"></typeparam>
    public abstract class EntityRepository<TE, MDE> : IEntityRepository<TE>
        where TE : IEntity
        where MDE : IEntityProvider<TE>
    {
        protected MDE _provider;

        /// <summary>
        /// Constructor for entity repository.
        /// </summary>
        /// <param name="provider"></param>
        protected EntityRepository(MDE provider)
        {
            this._provider = provider;
        }

        /// <summary>
        /// Create a document in collection.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<TE> Create(TE instance)
        {
            return await this._provider.Insert(instance);
        }

        /// <summary>
        /// Get a document from collection by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TE> Get(string id)
        {
            return await this._provider.Get(id);
        }

        /// <summary>
        /// Get all documents available in the collection.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TE>> GetAll()
        {
            return await this._provider.GetAll();
        }

        /// <summary>
        /// Update existing document in the collection.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<TE> Update(TE instance)
        {
            return await this._provider.Update(instance);
        }
    }
}
