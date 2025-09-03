using System;

namespace Insurwave.Movie.Api.Clients.Contracts;

public record MovieResponse
{
    public Guid Id { get; set; }
    public Guid OwningOrganisationId { get; set; }
    public string Title { get; set; }
    public int YearOfRelease { get; set; }
    public DateTime ChangeTimestamp { get; set; }
}

