namespace Insurwave.Movie.Persistence.Interfaces.Contracts.Filters;

public record MovieFilters
{
    public string? Title { get; set; }
    public int? YearOfRelease { get; set; }
}
