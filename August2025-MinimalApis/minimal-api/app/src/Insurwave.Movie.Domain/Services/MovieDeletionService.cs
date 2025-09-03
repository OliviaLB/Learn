using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Exceptions;
using Insurwave.Movie.Persistence.Interfaces.Exceptions;
using Insurwave.Movie.Persistence.Interfaces.Writers;

namespace Insurwave.Movie.Domain.Services;

public class MovieDeletionService : IMovieDeletionService
{
    private readonly IMovieWriter _movieWriter;

    public MovieDeletionService(IMovieWriter movieWriter)
    {
        _movieWriter = movieWriter;
    }

    public async Task Delete(Guid id, Guid organisationId, CancellationToken cancellationToken)
    {
        try
        {
            await _movieWriter.Delete(id, organisationId, cancellationToken);
        }
        catch (PersistenceEntityNotFoundException)
        {
            throw new MovieNotFoundException(id);
        }
    }
}
