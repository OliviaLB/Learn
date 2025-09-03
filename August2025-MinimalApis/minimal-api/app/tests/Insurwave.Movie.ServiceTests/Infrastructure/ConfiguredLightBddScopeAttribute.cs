using System.Diagnostics;
using System.Threading;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

[assembly: ConfiguredLightBddScope]
[assembly: ClassCollectionBehavior(AllowTestParallelization = true)]

namespace Insurwave.Movie.ServiceTests.Infrastructure;

internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
{
    private IContainer _postgresDockerContainer;

    protected override void OnSetUp()
    {
        _postgresDockerContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithPortBinding(5432, 5432)
            .Build();

        ServiceWebApplicationFactory.Initialise(serviceProvider =>
        {
            if (Settings.IsLocalEnvironment)
            {
                _postgresDockerContainer.StartAsync().Wait();
            }

            var timer = Stopwatch.StartNew();
            CreateDatabase(serviceProvider);
            timer.Stop();
            Thread.Sleep(10000);
            Console.Out.WriteLine($"Test databases created [{timer.ElapsedMilliseconds}ms]");
        });
    }

    private static void CreateDatabase(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MovieDbContext>();
        context.Database.EnsureCreated();
    }

    protected override void OnTearDown()
    {
        if (Settings.IsLocalEnvironment)
        {
            _postgresDockerContainer
                .StopAsync()
                .GetAwaiter()
                .GetResult();
        }


        ServiceWebApplicationFactory.Terminate(serviceProvider =>
        {
            if (!Settings.IsLocalEnvironment)
            {
                var context = serviceProvider.GetRequiredService<MovieDbContext>();
                context.Database.EnsureDeleted();
            }
        });
    }
}
