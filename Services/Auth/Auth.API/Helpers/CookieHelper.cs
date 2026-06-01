namespace Auth.API.Helpers
{
    public static class CookieHelper
    {
        public static void SetRefreshTokenCookie(
            HttpResponse response,
            string refreshToken,
            DateTime expires,
            bool isSecure = true)
        {
            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = expires
            });
        }

        public static void DeleteRefreshTokenCookie(HttpResponse response)
        {
            response.Cookies.Delete("refreshToken");
        }
    }
}
