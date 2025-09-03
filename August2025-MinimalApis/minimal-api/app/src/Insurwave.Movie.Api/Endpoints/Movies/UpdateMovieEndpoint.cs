using Microsoft.AspNetCore.Http.HttpResults;

namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public static IEndpointRouteBuilder MapUpdateMovieEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("{id:guid}", UpdateMovie)
            .WithSummary("Update a movie")
            .WithDescription("Partially updates a movie with the supplied updates");
        return app;
    }

    private static async Task<Results<Ok<MovieResponse>, NotFound, BadRequest>> UpdateMovie(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieRequest updateMovieRequest,
        [FromServices] IUserContextProvider userContextProvider,
        [FromServices] IValidator<UpdateMovieRequest> updateMovieRequestValidator,
        [FromServices] IMovieUpdateService movieUpdateService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await updateMovieRequestValidator.ValidateAsync(updateMovieRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new MovieValidationException(validationResult);
        }

        var organisationId = userContextProvider.GetCurrent().OrganisationId;
        var upsertMovieModel = mapper.MapToDomain(updateMovieRequest);
        var movie = await movieUpdateService.Update(id, organisationId, upsertMovieModel, cancellationToken);

        var response = mapper.MapToResponse(movie);

        return TypedResults.Ok(response);
    }
}
