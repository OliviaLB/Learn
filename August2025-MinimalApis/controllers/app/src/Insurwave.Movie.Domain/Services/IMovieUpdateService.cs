using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieUpdateService
{
    Task<MovieModel> Update(Guid id, Guid organisationId, UpdateMovieModel request,
        CancellationToken cancellationToken);
}
