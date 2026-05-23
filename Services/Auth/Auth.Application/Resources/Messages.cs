using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Auth.Application.Resources
{
    public static class Messages
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        private static IStringLocalizer<AuthSharedResource>? Localizer =>
            _httpContextAccessor?.HttpContext?.RequestServices.GetService<IStringLocalizer<AuthSharedResource>>();

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string UserNotFound => Localizer?["UserNotFound"] ?? "User not found";
        public static string InvalidPassword => Localizer?["InvalidPassword"] ?? "Invalid password";
    }
}
