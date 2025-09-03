using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.ServiceTests.Infrastructure.Clients;
using Insurwave.Movie.ServiceTests.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Create_Movie_Feature : FeatureFixture
{
    private readonly IFixture _fixture;
    private readonly Guid _organisationId;
    private readonly Guid _requestId;

    private MovieDbContext _dbContext;
    private readonly CreateMovieRequest _createMovieRequest;
    private MovieResponse _movieResponse;
    private HttpResponseMessage _httpResponse;

    private static IServiceProvider ServiceProvider => ServiceWebApplicationFactory.Instance.Services;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();

    public Create_Movie_Feature()
    {
        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
        _organisationId = Guid.NewGuid();
        _requestId = Guid.NewGuid();

        _fixture = new Fixture();

        _createMovieRequest = _fixture.Build<CreateMovieRequest>()
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .Create();
    }

    private async Task Movie_exists_in_the_database_with_title()
    {
        var movie = _fixture.Build<Persistence.Interfaces.Contracts.Movie>()
            .With(p => p.Title, _createMovieRequest.Title)
            .With(x => x.OwningOrganisationId, _organisationId)
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
            .Create();

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Create_movie_endpoint_is_called()
    {
        _httpResponse = await Client.CreateMovie(
            _requestId, _createMovieRequest);

        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
    }

    private async Task Movie_is_returned()
    {
        _movieResponse = (await _httpResponse.Content.ReadFromJsonAsync<MovieResponse>())!;
        _movieResponse.Should().NotBeNull();

        _movieResponse!.OwningOrganisationId.Should().Be(_organisationId);
        _movieResponse.Title.Should().Be(_createMovieRequest.Title);
        _movieResponse.YearOfRelease.Should().Be(_createMovieRequest.YearOfRelease);
        _movieResponse.ChangeTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    private async Task Movie_is_created_in_the_database()
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(p => p.Id == _movieResponse.Id);

        movie!.OwningOrganisationId.Should().Be(_organisationId);
        movie.Title.Should().Be(_createMovieRequest.Title);
        movie.YearOfRelease.Should().Be(_createMovieRequest.YearOfRelease);
        movie.ChangeTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
