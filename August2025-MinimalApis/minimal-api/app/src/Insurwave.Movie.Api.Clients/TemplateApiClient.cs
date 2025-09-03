using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Http;
using Insurwave.Http.Exceptions;

namespace Insurwave.Movie.Api.Clients;

/// <inheritdoc />
public class TemplateClient : ITemplateClient
{
    private readonly HttpClient _client;

    /// <summary>
    /// Construct the api client with it's dependencies
    /// </summary>
    /// <param name="client"></param>
    public TemplateClient(HttpClient client)
    {
        _client = client;
    }

    /// <inheritdoc />
    public async Task<int> Add(int a, int b, CancellationToken cancellationToken)
    {
        var query = new QueryStringBuilder()
            .WithValue("a", a.ToString())
            .WithValue("b", b.ToString());


        var response = await _client.PostAsync($"calculator/add{query}", new StringContent(string.Empty), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ExternalHttpRequestException(errorMessage, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
    }
}
