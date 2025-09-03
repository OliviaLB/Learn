using System.Collections.Generic;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Mappings;

public interface IMoviesResponseMapper
{
    MovieResponse MapToResponse(MovieModel movie);

    IEnumerable<MovieResponse> MapToResponse(IEnumerable<MovieModel> movies);
}
