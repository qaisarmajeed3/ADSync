

namespace  ScheduleJob.Service.Interface
{
    /// <summary>
    /// Interface for defining base service operations.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    public interface IEntityService<TE>
    {
        /// <summary>
        /// Creates an entity in collection.
        /// </summary>
        /// <param name="instance">Instance to be created</param>
        /// <returns><see cref="Task{TE}"/></returns>
        /// 
        Task<TE> Create(TE instance);

        /// <summary>
        /// Gets the entity based on selected id.
        /// </summary>
        /// <param name="id">Selectd entity id</param>
        /// <returns><see cref="Task{TE}"/></returns>
        /// 
        Task<TE> Get(string id);

        /// <summary>
        /// Updates selected document entity from collection.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// 
        Task<TE> Update(TE instance);

        /// <summary>
        /// Get all available documents from collection.
        /// </summary>
        /// <returns><see cref="List{TE}"/></returns>
        Task<IEnumerable<TE>> GetAll();
    }
}
