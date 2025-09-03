using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Providers;

public sealed class CiSettingsProvider
{
    private readonly SecretClientOptions _options;

    public CiSettingsProvider()
    {
        _options = new SecretClientOptions
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(15),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };
    }

    public string GetSecrets()
    {
        var client = new SecretClient(new Uri($"https://kv-np-neu-iw-ci.vault.azure.net"), new DefaultAzureCredential(), _options);
        // add all secrets here and return with one call to ci client
        return string.Empty;
    }
}
