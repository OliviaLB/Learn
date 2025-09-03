using Insurwave.Movie.Persistence.Interfaces.Contracts.Filters;

namespace Insurwave.Movie.Persistence.Interfaces.Readers;

public interface IMovieReader
{
    Task<Contracts.Movie?> GetSingle(string title, Guid organisationId, CancellationToken cancellationToken);

    Task<Contracts.Movie?> GetById(Guid id, Guid owningOrganisationId, CancellationToken cancellationToken);

    Task<(IEnumerable<Contracts.Movie> Movies, int Count)> GetAll(
        Guid owningOrganisationId,
        MovieFilters filters,
        CancellationToken cancellationToken);
}
