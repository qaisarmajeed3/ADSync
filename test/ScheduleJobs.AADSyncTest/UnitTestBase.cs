using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using  ScheduleJob.Data.Provider;
using  ScheduleJob.Domain.Interface;

namespace  ScheduleJob.Test
{
    /// <summary>
    /// Class that defines test base.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    public class UnitTestBase<TE> where TE : IEntity
    {
        public Mock<IAsyncCursor<TE>>? mockAsyncCursor;
        public Mock<IMongoCollection<TE>>? mockCollection;
        public Mock<IMongoDatabase>? mockDatabase;

        /// <summary>
        /// Mock Cursor, collection and database.
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            mockAsyncCursor = new Mock<IAsyncCursor<TE>>();
            mockCollection = new Mock<IMongoCollection<TE>>();
            mockDatabase = new Mock<IMongoDatabase>();
            Seed();
        }

        protected virtual void Seed()
        {
        }

        /// <summary>
        /// Set up mock collection, mock database.
        /// </summary>
        /// <returns></returns>
        protected virtual MongoDbProvider<TE> SetUpMockDbContext()
        {
          
            mockCollection?.Setup(_ => _.FindAsync(It.IsAny<FilterDefinition<TE>>(), It.IsAny<FindOptions<TE, TE>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockAsyncCursor?.Object);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mockDatabase?.Setup(_ => _.GetCollection<TE>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return new MongoDbProvider<TE>(mockDatabase.Object);
#pragma warning restore CS8602 // Dereference of a possibly null reference.


        }
       
    }
}
