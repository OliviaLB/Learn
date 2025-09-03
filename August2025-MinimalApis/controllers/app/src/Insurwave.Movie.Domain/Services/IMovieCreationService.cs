using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieCreationService
{
    Task<MovieModel> Create(Guid organisationId, CreateMovieModel request, CancellationToken cancellationToken);
}
