using AutoFixture;
using FluentAssertions;
using Insurwave.Movie.ServiceTests.Infrastructure.Clients;
using Insurwave.Movie.ServiceTests.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Delete_Movie_Feature : FeatureFixture
{
    private readonly Guid _organisationId;
    private readonly Guid _requestId;
    private readonly Guid _movieId;

    private readonly MovieDbContext _dbContext;
    private readonly Persistence.Interfaces.Contracts.Movie _movie;
    private HttpResponseMessage _httpResponse;

    private static IServiceProvider ServiceProvider => ServiceWebApplicationFactory.Instance.Services;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();

    public Delete_Movie_Feature()
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

    private async Task Delete_movie_by_id_endpoint_is_called()
    {
        _httpResponse = await Client.DeleteMovie(
            _movieId,
            _requestId);
    }

    private async Task Movie_is_deleted_in_the_database()
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(p => p.Id == _movieId);
        movie.Should().BeNull();
    }
}
