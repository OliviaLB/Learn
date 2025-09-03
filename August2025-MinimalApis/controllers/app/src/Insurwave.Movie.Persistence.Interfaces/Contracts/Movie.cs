namespace Insurwave.Movie.Persistence.Interfaces.Contracts;

public record Movie
{
    public Guid Id { get; set; }
    public Guid OwningOrganisationId { get; set; }
    public string Title { get; set; }
    public int YearOfRelease { get; set; }
    public DateTime ChangeTimestamp { get; set; }
}
