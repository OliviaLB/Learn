using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.ServiceTests.Infrastructure.Clients;
using Insurwave.Movie.ServiceTests.Infrastructure.Extensions;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Get_All_Movies_Feature : FeatureFixture
{
    private readonly IFixture _fixture;
    private readonly Guid _organisationId;
    private readonly Guid _requestId;
    private GetMoviesFilters _getMoviesFilters;

    private readonly MovieDbContext _dbContext;
    private readonly List<Persistence.Interfaces.Contracts.Movie> _movies;
    private HttpResponseMessage _httpResponse;

    private static IServiceProvider ServiceProvider => ServiceWebApplicationFactory.Instance.Services;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();

    public Get_All_Movies_Feature()
    {
        _dbContext = ServiceWebApplicationFactory.Instance.GetDbContext();
        _organisationId = Guid.NewGuid();
        _requestId = Guid.NewGuid();

        _fixture = new Fixture();

        _movies = [];
        _getMoviesFilters = new();
    }

    private async Task Movies_exists_in_the_database_for_organisation(Guid organisationId)
    {
        List<Persistence.Interfaces.Contracts.Movie> movies = [];
        movies.AddRange(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
            .With(x => x.OwningOrganisationId, organisationId)
            .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
            .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
            .CreateMany());

        if (!string.IsNullOrWhiteSpace(_getMoviesFilters.Title))
        {
            movies.Add(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
                .With(p => p.Title, $"{_getMoviesFilters.Title}")
                .With(x => x.OwningOrganisationId, organisationId)
                .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
                .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
                .Create());

            movies.Add(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
                .With(p => p.Title, $"{Guid.NewGuid()}{_getMoviesFilters.Title}")
                .With(x => x.OwningOrganisationId, organisationId)
                .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
                .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
                .Create());

            movies.Add(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
                .With(p => p.Title, $"{_getMoviesFilters.Title}{Guid.NewGuid()}")
                .With(x => x.OwningOrganisationId, organisationId)
                .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
                .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
                .Create());

            movies.Add(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
                .With(p => p.Title, $"{Guid.NewGuid()}{_getMoviesFilters.Title}{Guid.NewGuid()}")
                .With(x => x.OwningOrganisationId, organisationId)
                .With(p => p.YearOfRelease, Random.Shared.Next(1950, DateTime.Today.Year + 5))
                .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
                .Create());
        }

        if (_getMoviesFilters.YearOfRelease is not null)
        {
            movies.Add(_fixture.Build<Persistence.Interfaces.Contracts.Movie>()
                .With(x => x.OwningOrganisationId, organisationId)
                .With(p => p.YearOfRelease, _getMoviesFilters.YearOfRelease)
                .With(x => x.ChangeTimestamp, DateTime.UtcNow.TruncateToMicroseconds())
                .Create());
        }

        _dbContext.Movies.AddRange(movies);
        await _dbContext.SaveChangesAsync();

        _movies.AddRange(movies);
    }

    private async Task Get_movies_endpoint_is_called()
    {
        _httpResponse = await Client.GetMovies(
            _requestId,
            _getMoviesFilters);
    }

    private async Task Movies_for_user_organisation_returned()
    {
        var movieResponse = await _httpResponse.Content.ReadFromJsonAsync<List<MovieResponse>>();
        movieResponse.Should().NotBeNullOrEmpty();

        var expectedMovies = _movies.Where(x => x.OwningOrganisationId == _organisationId);

        if (!string.IsNullOrWhiteSpace(_getMoviesFilters.Title))
        {
            expectedMovies = expectedMovies.Where(x => x.Title.Contains(_getMoviesFilters.Title));
        }

        if (_getMoviesFilters.YearOfRelease is not null)
        {
            expectedMovies = expectedMovies.Where(x => x.YearOfRelease == _getMoviesFilters.YearOfRelease);
        }

        var expectedMovieResponses = expectedMovies
            .Select(p => new MovieResponse
            {
                Id = p.Id,
                Title = p.Title,
                ChangeTimestamp = p.ChangeTimestamp,
                OwningOrganisationId = p.OwningOrganisationId,
                YearOfRelease = p.YearOfRelease,
            });

        movieResponse.Should().BeEquivalentTo(expectedMovieResponses);
    }
}
