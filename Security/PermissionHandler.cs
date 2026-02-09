using Microsoft.AspNetCore.Authorization;

namespace pets.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Claim type = "permission"
            var has = context.User.Claims.Any(c =>
                c.Type == "permission" && c.Value == requirement.Permission);

            if (has)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
