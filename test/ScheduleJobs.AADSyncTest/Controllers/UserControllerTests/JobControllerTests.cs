using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Service.Model;
using  ScheduleJob.Service.Models;
using  ScheduleJob.WebApi.Model;
using System.Net;
using static  ScheduleJob.WebApi.Model.ExecuteJobModel;

namespace  ScheduleJob.Test.Controllers.UserControllerTests
{
    [TestClass]
    public class JobControllerTests: JobControllerTestBase
    {
        /// <summary>
        /// Test method for handling successfull job creation.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExpectOkResponseWhenServiceReturnsValidData_Execute()
        {
            _mockUserService.Setup(m => m.Create(It.IsAny<User>()));
            var controller = GetController();
            var response = await controller.Post(new ExecuteJobModel {JobName= Job.AADImportJob.ToString()});
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        /// <summary>
        /// Test method for handling successfull job creation for email work item.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExpectOkResponseWhenServiceReturnsValidData_ExecuteEmailWorkItemJob()
        {
            _mockTaskService.Setup(m => m.ExecuteJobAsync("test"));
            var controller = GetController();
            var response = await controller.Post(new ExecuteJobModel { JobName = Job.EmailTaskJob.ToString() });
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        /// <summary>
        /// Test method for handling successfull job creation for reading qms h1l7 data
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExpectOkResponseWhenQMSHl7fileisRead()
        {
            _mockPatientService.Setup(m => m.GetHl7File());
            var controller = GetController();
            var response = await controller.Post(new ExecuteJobModel { JobName = Job.QMSImportJob.ToString() });
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

    }
}