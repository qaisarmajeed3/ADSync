using MongoDB.Bson;
using MongoDB.Driver;
using  ScheduleJob.Data.Interfaces;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.Domain.Interface;

namespace  ScheduleJob.Data.Provider
{/// <summary>
 /// This will communicate with the database and gives you results based on that.It will provide you basic CRUD functionality.
 /// </summary>
 /// <typeparam name="TEntity">Entity model type.</typeparam>
    public class MongoDbProvider<TEntity> : IEntityProvider<TEntity> where TEntity : IEntity
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDbProvider(IMongoDatabase database)
        {
            _database = database;
            this._collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// get the document based on the selected id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> Get(string? id)
        {
            var _id = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", _id);
            return (await _collection.FindAsync(filter)).SingleOrDefault();
        }

        /// <summary>
        /// Gets documents matching the filter.
        /// </summary>
        /// <param name="filters">Filter definition which applies on collection.</param>
        /// <returns>List of enitities matching filter definition.</returns>
        public async Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters)
        {
            var list = await this._collection.FindAsync(filters);
            return list.ToList();
        }

        /// <summary>
        /// Get document from collection matching filter definition and after sorting.
        /// </summary>
        /// <param name="filters">Filter definition which applies on collection.</param>
        /// <param name="sort">For sorting documents in collection.</param>
        /// <returns>List of enitities matching filter definition and find options.</returns>
        public async Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters, SortDefinition<TEntity> sort)
        {
            var list = await this._collection.FindAsync(filters, new FindOptions<TEntity, TEntity>()
            {
                Sort = sort
            });
            return list.ToList();
        }

        /// <summary>
        /// Get document from collection matching filter definition and find options.
        /// </summary>
        /// <param name="filters">Filter definition which applies on collection.</param>
        /// <param name="options">Options for finding documents in collection.</param>
        /// <returns>List of enitities matching filter definition and find options.</returns>
        public async Task<IEnumerable<TEntity>> Get(FilterDefinition<TEntity> filters, FindOptions<TEntity> options)
        {
            var list = await this._collection.FindAsync(filters, options);
            return list.ToList();
        }

        /// <summary>
        /// Gets all documents in collection.
        /// </summary>
        /// <returns>List of entities.</returns>
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var list = await this._collection.FindAsync(filter);
            return list.ToList();
        }

        /// <summary>
        /// Inserts a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to insert.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> Insert(TEntity entityObject)
        {
            await this._collection.InsertOneAsync(entityObject);
            return entityObject;
        }

        /// <summary>
        /// Get the count of document in collection.
        /// </summary>
        /// <returns><see cref="Task{long}"/>.</returns>
        public async Task<long> Count()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var count = await this._collection.CountDocumentsAsync(filter);
            return count;
        }

        /// <summary>
        /// Updates a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> Update(TEntity entityObject)
        {
            var id = new ObjectId(entityObject.Id);
            var filter = Builders<TEntity>.Filter.And(
                Builders<TEntity>.Filter.Eq("_id", id));
            // Concurrency check
            return await this._collection.FindOneAndReplaceAsync(filter, entityObject);

        }

        /// <summary>
        /// Upserts a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> Upsert(TEntity entityObject)
        {
            await this._collection.ReplaceOneAsync(doc => doc.Id.Equals(entityObject.Id), entityObject,
                new ReplaceOptions { IsUpsert = true });
            return entityObject;
        }

        /// <summary>
        /// ApplyAggregate a document in collection.
        /// </summary>
        /// <param name="pipelineDefinition">Pipeline Definition for creation aggregation.</param>
        /// <returns><see cref="Task{IEnumerable}"/>.</returns>
        public async Task<IEnumerable<Object>> ApplyAggregate(PipelineDefinition<TEntity, BsonDocument> pipelineDefinition)
        {
            IEnumerable<Object> ObjectsList = (await this._collection.AggregateAsync<BsonDocument>(pipelineDefinition)).ToList();
            return ObjectsList;
        }

        /// <summary>
        /// Updates a portion of the document in collection.
        /// </summary>
        /// <param name="fieldsForUpdate">contanis the fields to be updated.</param>
        /// <param name="Id">Id of the Entity document needed to be updated.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public async Task<UpdateResult> PartialUpdate(string fieldsForUpdate, string Id)
        {
            var id = new ObjectId(Id);
            var filter = Builders<TEntity>.Filter.And(
                Builders<TEntity>.Filter.Eq("_id", id));


            var changesDocument = BsonDocument.Parse(fieldsForUpdate);

            UpdateDefinition<TEntity> updateDefinition = null;
            foreach (var change in changesDocument)
            {
                if (updateDefinition == null)
                {
                    var builder = Builders<TEntity>.Update;
                    updateDefinition = builder.Set(change.Name, change.Value);
                }
                else
                {
                    updateDefinition = updateDefinition.Set(change.Name, change.Value);
                }
            }

            var result = await this._collection.UpdateOneAsync(filter, updateDefinition);

            return result;
        }
        /// <summary>
        /// Inserts a document in collection.
        /// </summary>
        /// <param name="entityObject">Entity document needed to insert.</param>
        /// <returns><see cref="Task{TEntity}"/>.</returns>
        public IEnumerable<TEntity> InsertMany(IEnumerable<TEntity> entityObject)
        {
            this._collection.InsertMany(entityObject, new InsertManyOptions { IsOrdered = false });
            return entityObject;
        }
    }
}
