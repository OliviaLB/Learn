using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Mappings;
using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Readers;

namespace Insurwave.Movie.Domain.Services;

public class MovieRetrievalService : IMovieRetrievalService
{
    private readonly IMovieReader _movieReader;
    private readonly IMapper _mapper;

    public MovieRetrievalService(IMovieReader movieReader, IMapper mapper)
    {
        _movieReader = movieReader;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MovieModel>> Get(Guid organisationId, GetMoviesFilterModel filters, CancellationToken cancellationToken)
    {
        var persistenceFilters = _mapper.MapToPersistence(filters);

        var (movies, count) = await _movieReader.GetAll(organisationId, persistenceFilters, cancellationToken);
        return count == 0 ? [] : _mapper.MapToDomain(movies);
    }
}
