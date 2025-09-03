using System.Collections.Generic;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Mappings;

public interface IMovieDomainMapper
{
    MovieModel MapToDomain(Persistence.Interfaces.Contracts.Movie movie);

    IEnumerable<MovieModel> MapToDomain(IEnumerable<Persistence.Interfaces.Contracts.Movie> movies);

    GetMoviesFilterModel MapToDomain(GetMoviesFilters filters);

    CreateMovieModel MapToDomain(CreateMovieRequest request);
    UpdateMovieModel MapToDomain(UpdateMovieRequest request);
}
