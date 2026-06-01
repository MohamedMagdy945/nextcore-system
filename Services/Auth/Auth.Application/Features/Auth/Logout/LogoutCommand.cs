namespace Auth.Application.Features.Auth.Logout
{
    public class LogoutCommand
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
