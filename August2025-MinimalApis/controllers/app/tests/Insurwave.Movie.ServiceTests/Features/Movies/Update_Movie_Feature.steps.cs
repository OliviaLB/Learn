using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.ServiceTests.Infrastructure.Clients;
using Insurwave.Movie.ServiceTests.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Update_Movie_Feature : FeatureFixture
{
    private readonly IFixture _fixture;
    private readonly Guid _organisationId;
    private readonly Guid _requestId;
    private readonly Guid _movieId;

    private MovieDbContext _dbContext;
    private UpdateMovieRequest _updateMovieRequest;
    private Persistence.Interfaces.Contracts.Movie _movie;
    private HttpResponseMessage _httpResponse;

    private static IServiceProvider ServiceProvider => ServiceWebApplicationFactory.Instance.Services;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();

    public Update_Movie_Feature()
    {
        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
        _organisationId = Guid.NewGuid();
        _requestId = Guid.NewGuid();
        _movieId = Guid.NewGuid();

        _fixture = new Fixture();

        _movie = _fixture.Build<Persistence.Interfaces.Contracts.Movie>()
            .With(p => p.Id, _movieId)
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .With(x => x.OwningOrganisationId, _organisationId)
            .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
            .Create();

        _updateMovieRequest = _fixture.Build<UpdateMovieRequest>()
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .Create();
    }

    private async Task Movie_exists_in_the_database()
    {
        _dbContext.Movies.Add(_movie);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Movie_exists_in_the_database_with_title()
    {
        var movie = _fixture.Build<Persistence.Interfaces.Contracts.Movie>()
            .With(p => p.Title, _updateMovieRequest.Title)
            .With(x => x.OwningOrganisationId, _organisationId)
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
            .Create();

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Update_movie_endpoint_is_called()
    {
        _httpResponse = await Client.UpdateMovie(_movieId,
            _requestId, _updateMovieRequest);

        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
    }

    private async Task Movie_is_returned()
    {
        var movieResponse = (await _httpResponse.Content.ReadFromJsonAsync<MovieResponse>())!;
        movieResponse.Should().NotBeNull();

        movieResponse!.OwningOrganisationId.Should().Be(_organisationId);
        movieResponse.Title.Should().Be(_updateMovieRequest.Title ?? _movie.Title);
        movieResponse.YearOfRelease.Should().Be(_updateMovieRequest.YearOfRelease ?? _movie.YearOfRelease);
        movieResponse.ChangeTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    private async Task Movie_is_updated_in_the_database()
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(p => p.Id == _movieId);

        movie!.OwningOrganisationId.Should().Be(_organisationId);
        movie.Title.Should().Be(_updateMovieRequest.Title ?? _movie.Title);
        movie.YearOfRelease.Should().Be(_updateMovieRequest.YearOfRelease ?? _movie.YearOfRelease);
        movie.ChangeTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
