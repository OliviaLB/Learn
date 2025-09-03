using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieKeyedRetrievalService
{
    Task<MovieModel> GetById(Guid id, Guid organisationId, CancellationToken cancellationToken);
}
