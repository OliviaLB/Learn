using Microsoft.AspNetCore.Http.HttpResults;

namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public static IEndpointRouteBuilder MapDeleteMovieEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("{id:guid}", DeleteMovie)
            .WithSummary("Deletes a movie")
            .WithDisplayName("Delete movie matching the requested id for the user organisation");
        return app;
    }

    private static async Task<Results<NoContent, NotFound>> DeleteMovie(
        [FromRoute] Guid id,
        [FromServices] IUserContextProvider userContextProvider,
        [FromServices] IMovieDeletionService movieDeletionService,
        CancellationToken cancellationToken)
    {
        var organisationId = userContextProvider.GetCurrent().OrganisationId;
        await movieDeletionService.Delete(id, organisationId, cancellationToken);

        return TypedResults.NoContent();
    }
}
