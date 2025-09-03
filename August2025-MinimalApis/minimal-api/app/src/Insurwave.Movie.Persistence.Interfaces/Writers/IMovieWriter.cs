namespace Insurwave.Movie.Persistence.Interfaces.Writers;

public interface IMovieWriter
{
    Task Upsert(Contracts.Movie movie, CancellationToken cancellationToken);
    Task Delete(Guid id, Guid owningOrganisationId, CancellationToken cancellationToken);
}
