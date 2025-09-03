namespace Insurwave.Movie.Api.Clients.Contracts;

public record GetMoviesFilters
{
    public string? Title { get; set; }
    public int? YearOfRelease { get; set; }
}
