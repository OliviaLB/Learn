using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Contracts.Filters;

namespace Insurwave.Movie.Domain.Mappings;

public partial class Mapper
{
    public MovieFilters MapToPersistence(GetMoviesFilterModel filters)
    {
        return new MovieFilters { Title = filters.Title, YearOfRelease = filters.YearOfRelease, };
    }

    public Persistence.Interfaces.Contracts.Movie MapToPersistence(CreateMovieModel createMovieModel)
    {
        return new Persistence.Interfaces.Contracts.Movie
        {
            Title = createMovieModel.Title!,
            YearOfRelease = createMovieModel.YearOfRelease
        };
    }
}
