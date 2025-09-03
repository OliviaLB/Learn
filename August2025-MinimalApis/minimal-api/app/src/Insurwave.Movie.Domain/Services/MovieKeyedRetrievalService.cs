using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Exceptions;
using Insurwave.Movie.Domain.Mappings;
using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Readers;

namespace Insurwave.Movie.Domain.Services;

public class MovieKeyedRetrievalService : IMovieKeyedRetrievalService
{
    private readonly IMovieReader _movieReader;
    private readonly IMapper _mapper;

    public MovieKeyedRetrievalService(IMovieReader movieReader, IMapper mapper)
    {
        _movieReader = movieReader;
        _mapper = mapper;
    }

    public async Task<MovieModel> GetById(Guid id, Guid organisationId, CancellationToken cancellationToken)
    {
        var movie = await _movieReader.GetById(id, organisationId, cancellationToken);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        return _mapper.MapToDomain(movie);
    }
}
