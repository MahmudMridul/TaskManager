using System.Net;
using TaskManagerApi.Models;

namespace TaskManagerApi.Helpers
{
    public class ResponseHelper
    {
        public static ApiResponse CreateResponse(HttpStatusCode statusCode, object ? data = default, bool success = false, string msg = "", List<string>? errors = null)
        {
            return new ApiResponse
            {
                Data = data,
                Success = success,
                Message = msg,
                StatusCode = statusCode,
                Errors = errors
            };
        }
    }
}
