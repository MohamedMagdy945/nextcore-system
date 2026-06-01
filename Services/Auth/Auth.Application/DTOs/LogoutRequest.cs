namespace Auth.Application.DTOs
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}
