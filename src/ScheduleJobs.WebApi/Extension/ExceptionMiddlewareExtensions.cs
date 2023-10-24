using Microsoft.AspNetCore.Diagnostics;
using  ScheduleJob.WebApi.Model;
using System.Net;
using System.Text.Json;

namespace  ScheduleJob.WebApi.Extension
{
    /// <summary>
    /// Exception handler middleware to log exception
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// This handles all the exceptions in the application.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponseModel
                        {
                            Status = ApiResponseStatus.FAILURE,
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = contextFeature.Error.InnerException != null ? contextFeature.Error.InnerException.Message : contextFeature.Error.Message
                        }));
                    }
                });
            });
        }
    }
}
