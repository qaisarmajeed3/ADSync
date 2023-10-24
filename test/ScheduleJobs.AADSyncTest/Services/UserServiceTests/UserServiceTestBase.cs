using Castle.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using  Message.Service.Producer.Model;
using  Message.Service.Producer;
using  ScheduleJob.AADSync.Service.Model;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Repository.Repository;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.Service.Service;
using MSConfig = Microsoft.Extensions.Configuration;
namespace  ScheduleJob.Test.Services.UserServiceTests
{
    public class UserServiceTestBase : UnitTestBase<User>
    {
        protected IUserService? userService;
        protected IOptions<AzureAdImportApp> _mockAzureAdImportApp;
        protected MSConfig.IConfiguration _mockConfiguration;
        protected User userModel;
        protected ILogger<UserService> _mockLogger;
        protected IMessageProducer<string, MessageModel> _messageProducer;

        /// <summary>
        /// Create seed data for test user service.
        /// </summary>
        protected override void Seed()
        {
            mockAsyncCursor?.
                SetupSequence
                (_ => _.Move (It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            userService = GetService();
            userModel = new User
            {
                Id = "101ee43495e04535b9a78b20",
                FirstName = "Test",
                LastName = "user",
                DisplayName = "Test user",
                ActiveDirectoryId = Guid.NewGuid().ToString(),
                CreatedBy = Guid.NewGuid().ToString(),
                ModifiedBy = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Mock user service.
        /// </summary>
        /// <returns></returns>
        public IUserService GetService()
        {
            var mockMongoProvider = base.SetUpMockDbContext();
            _mockAzureAdImportApp=Mock.Of<IOptions<AzureAdImportApp>>();
            _mockConfiguration=Mock.Of<MSConfig.IConfiguration>();
            _mockLogger=Mock.Of<ILogger<UserService>>();
            _messageProducer = Mock.Of<IMessageProducer<string, MessageModel>>();
            userService = new UserService(new UserRepository(mockMongoProvider),_mockAzureAdImportApp,_mockConfiguration, _mockLogger, _messageProducer);
            return userService;
        }
    }
}