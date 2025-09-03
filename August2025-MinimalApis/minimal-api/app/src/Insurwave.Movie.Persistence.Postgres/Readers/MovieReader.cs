using Insurwave.Extensions.LINQ.Extensions;
using Insurwave.Movie.Persistence.Interfaces.Contracts.Filters;
using Insurwave.Movie.Persistence.Interfaces.Readers;
using Microsoft.EntityFrameworkCore;

namespace Insurwave.Movie.Persistence.Postgres.Readers;

public class MovieReader : IMovieReader
{
    private readonly MovieDbContext _dbContext;

    public MovieReader(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Interfaces.Contracts.Movie?> GetSingle(string title, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(p => p.OwningOrganisationId == organisationId
            && p.Title == title, cancellationToken);
    }

    public async Task<Interfaces.Contracts.Movie?> GetById(Guid id, Guid owningOrganisationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(p => p.Id == id && p.OwningOrganisationId == owningOrganisationId, cancellationToken);
    }

    public async Task<(IEnumerable<Interfaces.Contracts.Movie> Movies, int Count)> GetAll(
        Guid owningOrganisationId,
        MovieFilters filters,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies
            .WhereMatches(p => p.OwningOrganisationId, owningOrganisationId)
            .WhereMatches(p => p.YearOfRelease, filters.YearOfRelease);

        if (!string.IsNullOrWhiteSpace(filters.Title))
        {
            query = query.Where(p => p.Title.Contains(filters.Title));
        }

        var count = await query.CountAsync(cancellationToken);

        if (count == 0)
        {
            return ([], 0);
        }

        var results = await query.ToListAsync(cancellationToken);

        return (results, count);
    }
}
