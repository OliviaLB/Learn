using Insurwave.Movie.Api.Endpoints.Movies;

namespace Insurwave.Movie.Api.Endpoints;

public static class EndpointMapper
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMovieEndpoints();
        return app;
    }
}
