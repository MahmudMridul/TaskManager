using System.Net;

namespace TaskManagerApi.Models
{
    public class ApiResponse
    {
        public object? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public List<string>? Errors { get; set; }

    }
}
