using Microsoft.AspNetCore.Http.HttpResults;

namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class GetMovieByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetMovieById(this IEndpointRouteBuilder app)
    {
        app.MapGet("{id:guid}", GetMovieById)
            .WithSummary("Retrieves a movie by id")
            .WithDescription("Retrieves a movie with the specified id belonging to the user organisation");
        return app;
    }

    private static async Task<Results<Ok<MovieResponse>, NotFound>> GetMovieById(
        [FromRoute] Guid id,
        [FromServices] IUserContextProvider userContextProvider,
        [FromServices] IMovieKeyedRetrievalService movieKeyedRetrievalService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var organisationId = userContextProvider.GetCurrent().OrganisationId;
        var movies = await movieKeyedRetrievalService.GetById(id, organisationId, cancellationToken);
        var response = mapper.MapToResponse(movies);

        return TypedResults.Ok(response);
    }
}
