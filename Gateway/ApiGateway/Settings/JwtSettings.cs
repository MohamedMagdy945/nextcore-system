namespace ApiGateway.Settings
{
    public class JwtSettings
    {
        public string AccessTokenSecret { get; set; } = default!;
        public string RefreshTokenSecret { get; set; } = default!;
        public int AccessTokenExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
    }
}
