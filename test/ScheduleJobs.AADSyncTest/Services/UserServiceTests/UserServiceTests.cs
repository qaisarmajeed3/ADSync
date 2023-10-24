using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using  ScheduleJob.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  ScheduleJob.Test.Services.UserServiceTests
{
    [TestClass]
    public class UserServiceTests : UserServiceTestBase
    {
        /// <summary>
        /// Test method for handling successfull creation of user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExpectResultAfterCreate()
        {
            userService = GetService();
            var response = await userService.Create(new User 
            { Id = "101ee43495e04535b9a78b20",
              FirstName = "Test",
              LastName = "user",
              DisplayName="Test user",
              ActiveDirectoryId=Guid.NewGuid().ToString(),
              CreatedBy=Guid.NewGuid().ToString(),
              ModifiedBy=Guid.NewGuid().ToString(),
              CreatedDate=DateTime.UtcNow,
              ModifiedDate=DateTime.UtcNow
            });
            Assert.IsNotNull(response);
            Assert.AreEqual("Test",response?.FirstName);
        }


        #region User Update
        /// <summary>
        /// Test case for user update success scenario.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        [TestMethod]
        public async Task ExpectResultAfterTaskTypeUpdate()
        {
            mockCollection?.Setup(_ => _.FindOneAndReplaceAsync(It.IsAny<FilterDefinition<User>>(),
                It.IsAny<User>(), It.IsAny<FindOneAndReplaceOptions<User>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User());

            userService = GetService();

            var response = await userService.Update(userModel);
            Assert.IsNotNull(response);
        }

        #endregion
    }
}
