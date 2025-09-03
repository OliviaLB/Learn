using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieUniqueCheckService
{
    Task<bool> IsUnique(string title, Guid organisationId, Guid? id, CancellationToken cancellationToken);
}
