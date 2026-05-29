using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.AuthenticationConfig
{
    public class PermissionAuthorizationHandler
          : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissions = context.User.FindAll("permission")
                .Select(x => x.Value);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
