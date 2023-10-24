using Microsoft.AspNetCore.Mvc;
using  ScheduleJob.Service.Model;
using  ScheduleJob.WebApi.Extension;
using  ScheduleJob.WebApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace  ScheduleJob.WebApi.Controllers
{
    /// <summary>
    /// Base controller class from which other controllers will be inhirited
    /// </summary>
    /// <typeparam name="T">Generic type of Controller</typeparam>
    [CustomAuthorizeAttribute]
    [ValidateModel]
    [ApiController]
    public abstract class BaseController<T> : Microsoft.AspNetCore.Mvc.Controller
     
    {
        protected IHttpContextAccessor? _httpContextAccessor;
        private readonly ILogger<BaseController<T>> _logger;

        protected BaseController(IHttpContextAccessor accessor, ILogger<BaseController<T>> logger)
        {
            _httpContextAccessor = accessor;
            _logger = logger;
        }

        /// <summary>
        /// Prepare API response for each requests
        /// </summary>
        /// <typeparam name="T1">Generic class of response</typeparam>
        /// <param name="statusCode">Http Status coe</param>
        /// <param name="status">Success or failure status</param>
        /// <param name="responseData">Response object</param>
        /// <param name="message">Message related to response</param>
        /// <returns></returns>
        protected ApiResponseModel ApiResponse<T1>(HttpStatusCode statusCode, ApiResponseStatus status, T1 responseData, string? message = null)
        {
            ApiResponseModel response = new()
            {
                StatusCode = statusCode,
                Status = status,
                Message = message,
                Data = responseData
            };
            return response;
        }
    }
}