using Insurwave.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurwave.Movie.Api.Extensions;

public static class HealthCheckServiceCollectionExtensions
{
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureInsurwaveHealthChecks();

        return services;
    }
}
