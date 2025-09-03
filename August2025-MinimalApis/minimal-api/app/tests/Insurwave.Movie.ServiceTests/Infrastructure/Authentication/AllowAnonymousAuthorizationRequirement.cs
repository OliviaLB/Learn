using Microsoft.AspNetCore.Authorization;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Authentication;

public class AllowAnonymousAuthorizationRequirement :
    AuthorizationHandler<AllowAnonymousAuthorizationRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AllowAnonymousAuthorizationRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

