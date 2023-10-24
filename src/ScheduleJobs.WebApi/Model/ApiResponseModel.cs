using System.Net;
using System.Text.Json.Serialization;

namespace  ScheduleJob.WebApi.Model
{
    /// <summary>
    /// Model class for response with standard code,http status and data.
    /// </summary>
    public class ApiResponseModel
    {
        // Returns http staus code as response.
        public HttpStatusCode StatusCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        // Returns success or failure status as response.
        public ApiResponseStatus Status { get; set; } = default!;
        // Returns custom messages as reponse.
        public string? Message { get; set; }
        // Returns response data model as response.
        public dynamic? Data { get; set; }

    }
}
