namespace Auth.Application.Common
{
    public class AccessTokenResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiration { get; set; }
    }
}
