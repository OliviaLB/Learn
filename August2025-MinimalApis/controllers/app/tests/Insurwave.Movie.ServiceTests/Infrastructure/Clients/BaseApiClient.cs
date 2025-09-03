using System.Text;
using System.Text.Json;
using Insurwave.Http;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Clients;

public static class BaseApiClient
{
    public static HttpRequestMessage GetBaseRequest(
        HttpMethod method,
        string requestUri,
        Guid requestId,
        object? requestObject = null)
    {
        var request = new HttpRequestMessage(method, requestUri);
        request.Headers.Add("X-Request-Id", requestId.ToString());

        if (requestObject != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(requestObject, JsonSerializationOptions.Options), Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        return request;
    }
}
