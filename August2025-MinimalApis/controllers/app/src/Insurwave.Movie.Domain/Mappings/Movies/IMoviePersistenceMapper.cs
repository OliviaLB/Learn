using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Contracts.Filters;

namespace Insurwave.Movie.Domain.Mappings;

public interface IMoviePersistenceMapper
{
    MovieFilters MapToPersistence(GetMoviesFilterModel filters);

    Persistence.Interfaces.Contracts.Movie MapToPersistence(CreateMovieModel createMovieModel);
}
