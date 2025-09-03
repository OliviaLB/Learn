using Microsoft.Extensions.Hosting;

namespace Insurwave.Movie.Api.Extensions;

public static class IHostEnvironmentExtensions
{
    public static bool IsDevelopmentEnvironment(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsDevelopment() || hostEnvironment.IsEnvironment("local");
    }
}
