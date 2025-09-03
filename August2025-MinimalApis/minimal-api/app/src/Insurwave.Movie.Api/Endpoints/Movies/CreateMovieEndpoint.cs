using Microsoft.AspNetCore.Http.HttpResults;

namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public static IEndpointRouteBuilder MapCreateMovieEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(string.Empty, CreateMovie)
            .WithSummary("Creates a new movie")
            .WithDescription("Creates a new movie owned by the user organisation");
        return app;
    }

    private static async Task<Results<Created<MovieResponse>, BadRequest>> CreateMovie(
        [FromBody] CreateMovieRequest createMovieRequest,
        [FromServices] IUserContextProvider userContextProvider,
        [FromServices] IValidator<CreateMovieRequest> createMovieRequestValidator,
        [FromServices] IMovieCreationService movieCreationService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var validationResult =
            await createMovieRequestValidator.ValidateAsync(createMovieRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new MovieValidationException(validationResult);
        }

        var organisationId = userContextProvider.GetCurrent().OrganisationId;
        var upsertMovieModel = mapper.MapToDomain(createMovieRequest);
        var movie = await movieCreationService.Create(organisationId, upsertMovieModel, cancellationToken);

        var response = mapper.MapToResponse(movie);

        return TypedResults.Created($"movies/{movie.Id}", response);
    }
}
