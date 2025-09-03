namespace Insurwave.Movie.Api.Endpoints.Movies;

public static class MovieEndpointMapper
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointGroup = app.MapGroup("movies")
            .RequireAuthorization()
            .WithTags(OpenApiTags.Movies);

        endpointGroup
            .MapCreateMovieEndpoint()
            .MapGetMoviesEndpoint()
            .MapGetMovieById()
            .MapUpdateMovieEndpoint()
            .MapDeleteMovieEndpoint();

        return app;
    }
}
