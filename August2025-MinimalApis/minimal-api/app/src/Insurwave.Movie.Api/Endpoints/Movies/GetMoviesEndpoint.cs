using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class GetMoviesEndpoint
{
    public static IEndpointRouteBuilder MapGetMoviesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(string.Empty, GetMovies)
            .WithSummary("Retrieve all movies")
            .WithDescription("Retrieves all movies that are owned by the organisation");
        return app;
    }

    private static async Task<Ok<IEnumerable<MovieResponse>>> GetMovies(
        [AsParameters] GetMoviesFilters filters,
        [FromServices] IUserContextProvider userContextProvider,
        [FromServices] IMovieRetrievalService movieRetrievalService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var organisationId = userContextProvider.GetCurrent().OrganisationId;
        var domainFilters = mapper.MapToDomain(filters);
        var movies = await movieRetrievalService.Get(organisationId, domainFilters, cancellationToken);
        var response = mapper.MapToResponse(movies);

        return TypedResults.Ok(response);
    }
}
