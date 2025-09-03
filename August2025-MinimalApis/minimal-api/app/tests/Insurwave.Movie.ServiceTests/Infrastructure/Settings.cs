using System.IO;
using Insurwave.Movie.Api;
using Insurwave.Movie.ServiceTests.Infrastructure.Providers;
using Microsoft.Extensions.Configuration;

namespace Insurwave.Movie.ServiceTests.Infrastructure;

public static class Settings
{
    public static IConfiguration Configuration { get; }

    static Settings()
    {
        OverrideConfigurationSettings();

        var path = AppDomain.CurrentDomain.BaseDirectory;
        var appSettingsWebApp = Path.Combine(path, "appsettings.json");
        var appSettingsEnv = Path.Combine(path, $"appsettings.{ServiceTestEnvironment}.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(appSettingsWebApp, true, true)
            .AddJsonFile(appSettingsEnv, false, true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>()
            .Build();
        Configuration = config;
    }

    public static bool IsLocalEnvironment => ServiceTestEnvironment == "local";

    internal static bool IsCiEnvironment => ServiceTestEnvironment != "local";

    private static string ServiceTestEnvironment => Environment.GetEnvironmentVariable("SERVICE_TEST_ENV") ?? "local";

    private static void OverrideConfigurationSettings()
    {

        if (IsCiEnvironment)
        {
            var settingsProvider = new CiSettingsProvider();
            // update when more secrets are required
            var settingsValue = settingsProvider.GetSecrets();
        }
    }
}
