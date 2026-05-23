using Microsoft.AspNetCore.Http;

namespace Auth.Application.Bases
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }

        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public Response() { }

        public Response(T data, string message = "Request successful", int statusCode = 200)
        {
            IsSuccess = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
        public Response(string message, Dictionary<string, List<string>>? errors = null, int statusCode = 400)
        {
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
        }
        public static Response<T> Success(T data, string message = "Request successful", int statusCode = StatusCodes.Status200OK)
        {
            return new Response<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Response<T> Failure(string message,
            List<string>? errors = null,
            int statusCode = StatusCodes.Status400BadRequest,
                string? correlationId = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode,
            };
        }

        public static Response<T> NotFound(string message = "Resource not found",
            List<string>? errors = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        public static Response<T> Unauthorized(string message = "Unauthorized")
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string>(),
                StatusCode = 401
            };
        }
    }
}
