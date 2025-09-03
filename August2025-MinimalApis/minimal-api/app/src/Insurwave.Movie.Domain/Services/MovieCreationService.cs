using System;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Domain.Exceptions;
using Insurwave.Movie.Domain.Mappings;
using Insurwave.Movie.Domain.Models;
using Insurwave.Movie.Persistence.Interfaces.Writers;

namespace Insurwave.Movie.Domain.Services;

public class MovieCreationService : IMovieCreationService
{
    private readonly IMovieUniqueCheckService _movieUniqueCheckService;
    private readonly IMovieWriter _movieWriter;
    private readonly IMapper _mapper;

    public MovieCreationService(IMovieUniqueCheckService movieUniqueCheckService, IMovieWriter movieWriter, IMapper mapper)
    {
        _movieUniqueCheckService = movieUniqueCheckService;
        _movieWriter = movieWriter;
        _mapper = mapper;
    }

    public async Task<MovieModel> Create(Guid organisationId, CreateMovieModel request, CancellationToken cancellationToken)
    {
        var isUnique = await _movieUniqueCheckService.IsUnique(request.Title!, organisationId, null, cancellationToken);
        if (!isUnique)
        {
            throw new UniqueMovieException(request.Title!);
        }

        var movie = _mapper.MapToPersistence(request);
        movie.Id = Guid.NewGuid();
        movie.OwningOrganisationId = organisationId;
        movie.ChangeTimestamp = DateTime.UtcNow;

        await _movieWriter.Upsert(movie, cancellationToken);

        return _mapper.MapToDomain(movie);
    }
}
