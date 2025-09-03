using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Persistence.Interfaces.Readers;

namespace Insurwave.Movie.Domain.Services;

public class MovieUniqueCheckService : IMovieUniqueCheckService
{
    private readonly IMovieReader _movieReader;

    public MovieUniqueCheckService(IMovieReader movieReader)
    {
        _movieReader = movieReader;
    }

    public async Task<bool> IsUnique(string title, Guid organisationId, Guid? id, CancellationToken cancellationToken)
    {
        var movie = await _movieReader.GetSingle(title, organisationId, cancellationToken);

        if (!id.HasValue)
        {
            return movie is null;
        }

        if (movie is null)
        {
            return true;
        }

        return movie.Id == id;
    }
}
