using Microsoft.AspNetCore.Http;

namespace Auth.Application.Bases;

public class Result<T> : IResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public List<string>? Errors { get; init; }
    public T? Data { get; init; }
    public int StatusCode { get; init; }

    public Result() { }

    public static Result<T> Success(T data, string message = "Request successful", int statusCode = StatusCodes.Status200OK)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static Result<T> Failure(string message, List<string>? errors = null, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>(),
            StatusCode = statusCode,
        };
    }

    public static Result<T> NotFound(string message = "Resource not found", List<string>? errors = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>(),
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public static Result<T> Unauthorized(string message = "Unauthorized")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = new List<string>(),
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }
}