namespace  ScheduleJob.Repository.Interface
{
    /// <summary>
    /// Interface for base repository operations.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    public interface IEntityRepository<TE>
    {
        /// <summary>
        /// Create a new documentin the collection.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Task<TE> Create(TE instance);

        /// <summary>
        /// Get the document from collection by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TE> Get(string id);

        /// <summary>
        /// Update the extisting document.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Task<TE> Update(TE instance);

        /// <summary>
        /// Get all available document from collection.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TE>> GetAll();
    }
}
