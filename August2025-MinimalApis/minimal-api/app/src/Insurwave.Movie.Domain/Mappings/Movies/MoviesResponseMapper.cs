using System.Collections.Generic;
using System.Linq;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.Domain.Models;

namespace Insurwave.Movie.Domain.Mappings;

public partial class Mapper
{
    public MovieResponse MapToResponse(MovieModel movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            OwningOrganisationId = movie.OwningOrganisationId,
            ChangeTimestamp = movie.ChangeTimestamp,
            YearOfRelease = movie.YearOfRelease,
        };
    }

    public IEnumerable<MovieResponse> MapToResponse(IEnumerable<MovieModel> movies)
    {
        return movies?.Select(MapToResponse);
    }
}
