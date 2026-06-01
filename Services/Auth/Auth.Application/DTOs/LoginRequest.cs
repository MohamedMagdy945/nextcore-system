namespace Auth.Application.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
    }
}
