using System;
using System.Threading;
using System.Threading.Tasks;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieDeletionService
{
    Task Delete(Guid id, Guid organisationId, CancellationToken cancellationToken);
}
