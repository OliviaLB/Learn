using System.Linq;
using Insurwave.Extensions.Authentication;
using Insurwave.Logging.Testing;
using Insurwave.Movie.Api;
using Insurwave.Movie.ServiceTests.Infrastructure.Authentication;
using Insurwave.Movie.ServiceTests.Infrastructure.Stubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Insurwave.Movie.ServiceTests.Infrastructure;

public class ServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private static Exception s_exception;
    private static ServiceWebApplicationFactory s_instance;

    //NOTE: DO NOT REMOVE
    // This constructor is added to reinforce that this class should NOT be instantiated directly
    private ServiceWebApplicationFactory() { }

    public TestLogger TestLogger => Services.GetRequiredService<TestLogger>();

    public MovieDbContext GetDbContext()
    {
        var factory = Services.GetRequiredService<IDbContextFactory<MovieDbContext>>();
        return factory.CreateDbContext();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environment.GetEnvironmentVariable("SERVICE_TEST_ENV") ?? "local")
            .UseConfiguration(Settings.Configuration)
            .ConfigureServices(services =>
            {
                services.RemoveAuthentication();
                services.AddDbContextFactory<MovieDbContext>();
                services.AddHeaderPropagation();
                services.AddSingleton<IUserContextProvider, UserContextProviderStub>();
                services.AddTestLoggerDependencies();
            });

        builder.ConfigureTestLogger();
    }

    public static ServiceWebApplicationFactory Instance
    {
        get
        {
            if (s_instance is not null)
                return s_instance;

            throw s_exception ?? new Exception("server failed to initialise");
        }
    }

    public static void Initialise(Action<IServiceProvider> initialisationActions)
    {

        try
        {
            s_instance = new();
            // The WebApplicationFactory has a problem that until CreateDefaultClient() is called, no underlying TestServer instance is created.
            // Also, the underlying TestServer creation has a race condition, calling it from tests running in parallel may cause multiple servers to be spawned.
            s_instance.CreateDefaultClient();
            initialisationActions(s_instance.Services);
        }
        catch (Exception exception)
        {
            s_exception = new Exception("Initialisation failed", exception);
        }
    }

    public static void Terminate()
    {
        s_instance?.Dispose();
    }

    public static void Terminate(Action<IServiceProvider> terminationActions)
    {

        try
        {
            terminationActions(s_instance.Services);
            s_instance?.Dispose();
        }
        catch (Exception exception)
        {
            s_exception = new Exception("Termination failed", exception);
        }
    }
}
