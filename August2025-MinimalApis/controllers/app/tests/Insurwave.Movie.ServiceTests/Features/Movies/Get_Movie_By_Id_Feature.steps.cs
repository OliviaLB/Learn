using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.ServiceTests.Infrastructure.Clients;
using Insurwave.Movie.ServiceTests.Infrastructure.Extensions;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Get_Movie_By_Id_Feature : FeatureFixture
{
    private readonly Guid _organisationId;
    private readonly Guid _requestId;
    private readonly Guid _movieId;

    private readonly MovieDbContext _dbContext;
    private readonly Persistence.Interfaces.Contracts.Movie _movie;
    private HttpResponseMessage _httpResponse;

    private static IServiceProvider ServiceProvider => ServiceWebApplicationFactory.Instance.Services;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();

    public Get_Movie_By_Id_Feature()
    {
        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
        _organisationId = Guid.NewGuid();
        _requestId = Guid.NewGuid();
        _movieId = Guid.NewGuid();

        var fixture = new Fixture();

        _movie = fixture.Build<Persistence.Interfaces.Contracts.Movie>()
            .With(x => x.Id, _movieId)
            .With(x => x.OwningOrganisationId, _organisationId)
            .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
            .Create();
    }

    private async Task Movie_exists_in_the_database()
    {
        _dbContext.Movies.Add(_movie);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Get_movie_by_id_endpoint_is_called()
    {
        _httpResponse = await Client.GetMovieById(
            _movieId,
            _requestId);
    }

    private async Task Movie_is_returned()
    {
        var movieResponse = await _httpResponse.Content.ReadFromJsonAsync<MovieResponse>();
        movieResponse.Should().NotBeNull();

        movieResponse!.OwningOrganisationId.Should().Be(_movie.OwningOrganisationId);
        movieResponse.Title.Should().Be(_movie.Title);
        movieResponse.YearOfRelease.Should().Be(_movie.YearOfRelease);
        movieResponse.ChangeTimestamp.Should().Be(_movie.ChangeTimestamp);
    }
}
