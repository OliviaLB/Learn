using Insurwave.Http;
using Insurwave.Movie.Api.Clients.Contracts;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Clients;

public static class MovieApiClient
{
    private const string BaseUrl = "movies";

    public static async Task<HttpResponseMessage> GetMovieById(
        this HttpClient client,
        Guid movieId,
        Guid requestId)
    {
        var request = BaseApiClient.GetBaseRequest(HttpMethod.Get, $"{BaseUrl}/{movieId}", requestId);
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> GetMovies(
        this HttpClient client,
        Guid requestId,
        GetMoviesFilters filters)
    {

        var query = new QueryStringBuilder()
            .WithValue("title", filters.Title)
            .WithValue("yearOfRelease", filters.YearOfRelease?.ToString())
            .ToQueryString();

        var request = BaseApiClient.GetBaseRequest(HttpMethod.Get, $"{BaseUrl}{query}", requestId);
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> CreateMovie(
        this HttpClient client,
        Guid requestId,
        CreateMovieRequest createMovieRequest)
    {
        var request = BaseApiClient.GetBaseRequest(HttpMethod.Post, $"{BaseUrl}", requestId, createMovieRequest);
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> UpdateMovie(
        this HttpClient client,
        Guid movieId,
        Guid requestId,
        UpdateMovieRequest updateMovieRequest)
    {
        var request = BaseApiClient.GetBaseRequest(HttpMethod.Patch, $"{BaseUrl}/{movieId}", requestId, updateMovieRequest);
        return await client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> DeleteMovie(
        this HttpClient client,
        Guid movieId,
        Guid requestId)
    {
        var request = BaseApiClient.GetBaseRequest(HttpMethod.Delete, $"{BaseUrl}/{movieId}", requestId);
        return await client.SendAsync(request);
    }
}
