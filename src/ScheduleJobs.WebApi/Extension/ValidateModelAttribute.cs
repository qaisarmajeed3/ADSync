using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using  ScheduleJob.Domain.Constants;
using  ScheduleJob.WebApi.Model;
using System.Net;

namespace  ScheduleJob.WebApi.Extension
{
    /// <summary>
    /// Validate model attribute
    /// </summary>
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation for action executed method as it is handled in controller level.
            return;
        }

        /// <summary>
        /// Validate each requests model
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(MessageConstants.InvalidRequest)
                {
                    Value = new ApiResponseModel
                    {
                        Status = ApiResponseStatus.FAILURE,
                        StatusCode = HttpStatusCode.BadRequest,
                        //Adding exact Error Message if available.
                        Message = context.ModelState.Values.FirstOrDefault()?.Errors?.FirstOrDefault()?.ErrorMessage ?? MessageConstants.InvalidRequest
                    },
                };
            }
        }
    }
}