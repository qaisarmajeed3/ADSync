using Microsoft.AspNetCore.Mvc;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.Domain.Models;
using  ScheduleJob.Interface;
using  ScheduleJob.Service.Interface;
using  ScheduleJob.WebApi.Model;
using System.Net;
using static  ScheduleJob.WebApi.Model.ExecuteJobModel;

namespace  ScheduleJob.WebApi.Controllers
{
    /// <summary>
    /// Job controller to execute the job
    /// </summary>
    [Route("job")]
    public class JobController : BaseController<User>
    {
        private readonly IUserService _userService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public JobController(IBackgroundTaskQueue backgroundTaskQueue, ILogger<JobController> logger,
            IUserService userService, IHttpContextAccessor accessor) : base(accessor, logger)
        {
            this._userService = userService;
            _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
        }

        /// <summary>
        /// Execute the schedule job
        /// </summary>
        /// <param name="jobModel">Job execution model</param>
        /// <returns>Response model</returns>
        [HttpPost("execute")]
        public async Task<ApiResponseModel> Post([FromBody] ExecuteJobModel jobModel)
        {
            bool isValidJob = ExecuteJob(jobModel.JobName);

            if (isValidJob)
            {
                return ApiResponse(HttpStatusCode.OK, ApiResponseStatus.SUCCESS, jobModel, MessageConstants.JobConstants.JobSuccessMessage);
            }
            else
            {
                return ApiResponse(HttpStatusCode.BadRequest, ApiResponseStatus.FAILURE, jobModel, MessageConstants.JobConstants.InvalidJobMessage);
            }
        }

        /// <summary>
        /// Execute scheduler jobs based on the selected condition.
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        private bool ExecuteJob(string jobName)
        {
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            string token=authHeader.ToString().Replace("Bearer ", string.Empty);
            Job batchJob;
            var isValidJob = Enum.TryParse<Job>(jobName, true, out batchJob);
            if (!isValidJob)
            {
                return false;
            }
            _backgroundTaskQueue.EnqueueTask(async (serviceScopeFactory, cancellationToken) =>
            {
                // Get services
                using var scope = serviceScopeFactory.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<JobController>>();
                var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                try
                {
                    logger.LogInformation("ExecuteJob start.");
                    switch (batchJob)
                    {
                        case Job.AADImportJob:
                            await userService.ExecuteJobAsync();
                            break;
                        case Job.EmailTaskJob:
                            await taskService.ExecuteJobAsync(token);
                            break;
                        case Job.QMSImportJob:
                            await patientService.GetHl7File();
                            break;
                        default:
                            break;
                    }
                    logger.LogInformation("ExecuteJob end.");

                }
                catch (Exception ex)
                {
                    logger.LogInformation("ExecuteJob exception.");
                    logger.LogError(ex, "Could not do something expensive");
                }
            });
            return true;

        }


    }
}