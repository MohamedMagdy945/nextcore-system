namespace Auth.Application.Bases
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Message { get; }
        List<string>? Errors { get; }
        int StatusCode { get; }
    }
}
