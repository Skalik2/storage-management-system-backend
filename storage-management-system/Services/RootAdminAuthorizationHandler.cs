using Microsoft.AspNetCore.Authorization;

namespace storage_management_system.Services
{
    public class RootAdminAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if (context.User.IsInRole("RootAdmin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
