using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Exceptions;
using Insurwave.Movie.Domain.Mappings;
using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Readers;
using Insurwave.Movie.Persistence.Interfaces.Writers;

namespace Insurwave.Movie.Domain.Services;

public class MovieUpdateService : IMovieUpdateService
{
    private readonly IMovieReader _movieReader;
    private readonly IMovieUniqueCheckService _movieUniqueCheckService;
    private readonly IMovieWriter _movieWriter;
    private readonly IMapper _mapper;

    public MovieUpdateService(IMovieReader movieReader, IMovieUniqueCheckService movieUniqueCheckService, IMovieWriter movieWriter, IMapper mapper)
    {
        _movieReader = movieReader;
        _movieUniqueCheckService = movieUniqueCheckService;
        _movieWriter = movieWriter;
        _mapper = mapper;
    }

    public async Task<MovieModel> Update(Guid id, Guid organisationId, UpdateMovieModel request, CancellationToken cancellationToken)
    {
        var movie = await _movieReader.GetById(id, organisationId, cancellationToken);
        if (movie is null)
        {
            throw new MovieNotFoundException(id);
        }

        var updatedMovie = new Persistence.Interfaces.Contracts.Movie()
        {
            Id = movie.Id,
            Title = request.Title ?? movie.Title,
            OwningOrganisationId = movie.OwningOrganisationId,
            YearOfRelease = request.YearOfRelease ?? movie.YearOfRelease,
            ChangeTimestamp = DateTime.UtcNow,
        };

        var isUnique = await _movieUniqueCheckService.IsUnique(updatedMovie.Title, organisationId, id, cancellationToken);
        if (!isUnique)
        {
            throw new UniqueMovieException(updatedMovie.Title);
        }

        await _movieWriter.Upsert(updatedMovie, cancellationToken);
        return _mapper.MapToDomain(movie);
    }
}
