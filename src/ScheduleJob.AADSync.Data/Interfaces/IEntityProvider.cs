using MongoDB.Bson;
using MongoDB.Driver;

namespace  ScheduleJob.Data.Interfaces
{
    /// <summary>
    /// This will communicate with the database and gives you results based on that.It will provide you basic CRUD functionality.
    /// </summary>
    /// <typeparam name="TEntity">Entity model type.</typeparam>
    public interface IEntityProvider<TEntity>
    {
        /// <summary>
        /// Gets all documents in collection.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// Gets document from collection with respect to id.
        /// </summary>
        /// <param name="objectId">Id of the entity document.</param>
        /// <returns>Entity document.</returns>
        Task<TEntity> Get(string id);

        /// <summary>
        /// Gets documents matching the filter.
        /// </summary>
        /// <param name="filters">Filter definition which applies on collection.</param>
        /// <returns>List of enitities matching filter definition.</returns>
        Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters);

        /// <summary>
        /// Get document from collection matching filter definition and find options.
        /// </summary>
        /// <param name="filters">Filter definition which applies on collection.</param>
        /// <param name="options">Options for finding documents in collection.</param>
        /// <returns>List of enitities matching filter definition and find options.</returns>
        Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters, FindOptions<TEntity> options);

        /// <summary>
        /// Inserts a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to insert.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> Insert(TEntity entityObject);

        /// <summary>
        /// Updates a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> Update(TEntity entityObject);

        /// <summary>
        /// Upserts a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> Upsert(TEntity entityObject);

        /// <summary>
        /// Get the count of document in collection.
        /// </summary>
        /// <returns><see cref="Task{long}"/>.</returns>
        Task<long> Count();
        /// <summary>
        /// Get a document in collection.
        /// </summary>
        /// <param name="filters">Filters to apply in a document.</param>
        /// <param name="sort">Sort to apply in a document.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters, SortDefinition<TEntity> sort);
        /// <summary>
        /// Apply aggregation in a collection.
        /// </summary>
        /// <param name="pipelineDefinition">Pipeline Definition to apply in a document.</param>
        /// <returns><see cref="Task{IEnumerable}"/>.</returns>
        Task<IEnumerable<Object>> ApplyAggregate(PipelineDefinition<TEntity, BsonDocument> pipelineDefinition);
        IEnumerable<TEntity> InsertMany(IEnumerable<TEntity> entityObject);

        /// <summary>
        /// Updates a portion of the document in collection.
        /// </summary>
        /// <param name="fieldsForUpdate">contanis the fields to be updated.</param>
        /// <param name="Id">Id of the Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        Task<UpdateResult> PartialUpdate(string fieldsForUpdate, string Id);
    }
}
