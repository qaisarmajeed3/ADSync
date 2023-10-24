using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Interface;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.WebApi.Controllers;
using System.Net.Http;

namespace  ScheduleJob.Test.Controllers.UserControllerTests
{
    public class JobControllerTestBase:UnitTestBase<User>
    {
        protected ILogger<JobController> _logger;
        protected IHttpContextAccessor _httpContextAccessor;
        protected Mock<IUserService> _mockUserService;
        protected JobController jobController;
        protected IBackgroundTaskQueue _mockbackgroundTaskQueue;
        protected Mock<ITaskService> _mockTaskService;
        protected Mock<IPatientService> _mockPatientService;
        /// <summary>
        /// Create seed data for executing test cases.
        /// </summary>
        protected override void Seed()
        {
            _logger = Mock.Of<ILogger<JobController>>();
            _httpContextAccessor = Mock.Of<IHttpContextAccessor>();
            _mockUserService = new Mock<IUserService>();
            _mockbackgroundTaskQueue = Mock.Of<IBackgroundTaskQueue>();
            _mockTaskService = new Mock<ITaskService>();
            _mockPatientService = new Mock<IPatientService>();
            GetController();
        }

        /// <summary>
        /// Mock controller.
        /// </summary>
        /// <returns></returns>
        public JobController GetController()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Authorization", "Bearer test");
            var controller = new JobController(_mockbackgroundTaskQueue, _logger, _mockUserService.Object, _httpContextAccessor)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };
            return controller;
        }
    }
}