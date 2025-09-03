using System.Collections.Generic;
using System.Linq;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Mappings;

public partial class Mapper
{
    public MovieModel MapToDomain(Persistence.Interfaces.Contracts.Movie movie)
    {
        return new MovieModel
        {
            Id = movie.Id,
            OwningOrganisationId = movie.OwningOrganisationId,
            Title = movie.Title,
            ChangeTimestamp = movie.ChangeTimestamp,
            YearOfRelease = movie.YearOfRelease,
        };
    }

    public IEnumerable<MovieModel> MapToDomain(IEnumerable<Persistence.Interfaces.Contracts.Movie> movies)
    {
        return movies?.Select(MapToDomain);
    }

    public GetMoviesFilterModel MapToDomain(GetMoviesFilters filters)
    {
        return new GetMoviesFilterModel { Title = filters.Title, YearOfRelease = filters.YearOfRelease, };
    }

    public CreateMovieModel MapToDomain(CreateMovieRequest request)
    {
        return new CreateMovieModel { Title = request.Title, YearOfRelease = request.YearOfRelease };
    }

    public UpdateMovieModel MapToDomain(UpdateMovieRequest request)
    {
        return new UpdateMovieModel { Title = request.Title, YearOfRelease = request.YearOfRelease };
    }
}
