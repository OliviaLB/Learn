using Insurwave.Movie.Persistence.Interfaces.Exceptions;
using Insurwave.Movie.Persistence.Interfaces.Readers;
using Insurwave.Movie.Persistence.Interfaces.Writers;

namespace Insurwave.Movie.Persistence.Postgres.Writers;

public class MovieWriter : IMovieWriter
{
    private readonly IMovieReader _movieReader;
    private readonly MovieDbContext _dbContext;

    public MovieWriter(IMovieReader movieReader, MovieDbContext dbContext)
    {
        _movieReader = movieReader;
        _dbContext = dbContext;
    }

    public async Task Upsert(Interfaces.Contracts.Movie movie, CancellationToken cancellationToken)
    {
        var dbMovie = await _movieReader.GetById(movie.Id, movie.OwningOrganisationId, cancellationToken);
        if (dbMovie == null)
        {
            _dbContext.Movies.Add(movie);
        }
        else
        {
            if (dbMovie.ChangeTimestamp > movie.ChangeTimestamp)
            {
                // stale
                return;
            }

            _dbContext.Entry(dbMovie).CurrentValues.SetValues(movie);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, Guid owningOrganisationId, CancellationToken cancellationToken)
    {
        var entity = await _movieReader.GetById(id, owningOrganisationId, cancellationToken);
        if (entity == null)
        {
            throw new PersistenceEntityNotFoundException(nameof(Interfaces.Contracts.Movie), id, owningOrganisationId);
        }

        _dbContext.Movies.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
