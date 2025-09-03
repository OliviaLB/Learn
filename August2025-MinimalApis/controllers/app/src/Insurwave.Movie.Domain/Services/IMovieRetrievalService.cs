using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Services;

public interface IMovieRetrievalService
{
    Task<IEnumerable<MovieModel>> Get(Guid organisationId, GetMoviesFilterModel filters, CancellationToken cancellationToken);
}
