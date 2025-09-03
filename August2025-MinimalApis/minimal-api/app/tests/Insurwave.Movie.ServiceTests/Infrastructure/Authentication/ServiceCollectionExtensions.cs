using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RemoveAuthentication(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicy(
                [new AllowAnonymousAuthorizationRequirement()],
                []));
        return services;
    }
}
