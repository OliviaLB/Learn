namespace Insurwave.Movie.Api.Clients.Contracts;

public record UpdateMovieRequest
{
    public string? Title { get; set; }
    public int? YearOfRelease { get; set; }
}
