using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Insurwave.Extensions.Authentication;
using Insurwave.Movie.Api.Clients.Contracts;
using Insurwave.Movie.Api.Exceptions;
using Insurwave.Movie.Domain.Mappings;
using Insurwave.Movie.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurwave.Movie.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IUserContextProvider _userContextProvider;
    private readonly IMovieCreationService _movieCreationService;
    private readonly IValidator<CreateMovieRequest> _createMovieRequestValidator;
    private readonly IMovieRetrievalService _movieRetrievalService;
    private readonly IMovieKeyedRetrievalService _movieKeyedRetrievalService;
    private readonly IMovieUpdateService _movieUpdateService;
    private readonly IValidator<UpdateMovieRequest> _updateMovieRequestValidator;
    private readonly IMovieDeletionService _movieDeletionService;
    private readonly IMapper _mapper;

    public MoviesController(IUserContextProvider userContextProvider, IMovieCreationService movieCreationService, IValidator<CreateMovieRequest> createMovieRequestValidator, IMovieRetrievalService movieRetrievalService, IMovieKeyedRetrievalService movieKeyedRetrievalService, IMovieUpdateService movieUpdateService, IValidator<UpdateMovieRequest> updateMovieRequestValidator, IMovieDeletionService movieDeletionService, IMapper mapper)
    {
        _userContextProvider = userContextProvider;
        _movieCreationService = movieCreationService;
        _createMovieRequestValidator = createMovieRequestValidator;
        _movieRetrievalService = movieRetrievalService;
        _movieKeyedRetrievalService = movieKeyedRetrievalService;
        _movieUpdateService = movieUpdateService;
        _updateMovieRequestValidator = updateMovieRequestValidator;
        _movieDeletionService = movieDeletionService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest createMovieRequest, CancellationToken cancellationToken)
    {
        var validationResult = await _createMovieRequestValidator.ValidateAsync(createMovieRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new MovieValidationException(validationResult);
        }

        var organisationId = _userContextProvider.GetCurrent().OrganisationId;
        var upsertMovieModel = _mapper.MapToDomain(createMovieRequest);
        var movie = await _movieCreationService.Create(organisationId, upsertMovieModel, cancellationToken);

        var response = _mapper.MapToResponse(movie);

        return Created($"movies/{movie.Id}", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMoviesFilters filters, CancellationToken cancellationToken)
    {
        var organisationId = _userContextProvider.GetCurrent().OrganisationId;
        var domainFilters = _mapper.MapToDomain(filters);
        var movies = await _movieRetrievalService.Get(organisationId, domainFilters, cancellationToken);
        var response = _mapper.MapToResponse(movies);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var organisationId = _userContextProvider.GetCurrent().OrganisationId;
        var movies = await _movieKeyedRetrievalService.GetById(id, organisationId, cancellationToken);
        var response = _mapper.MapToResponse(movies);

        return Ok(response);
    }


    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieRequest updateMovieRequest, CancellationToken cancellationToken)
    {
        var validationResult = await _updateMovieRequestValidator.ValidateAsync(updateMovieRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new MovieValidationException(validationResult);
        }

        var organisationId = _userContextProvider.GetCurrent().OrganisationId;
        var upsertMovieModel = _mapper.MapToDomain(updateMovieRequest);
        var movie = await _movieUpdateService.Update(id, organisationId, upsertMovieModel, cancellationToken);

        var response = _mapper.MapToResponse(movie);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var organisationId = _userContextProvider.GetCurrent().OrganisationId;
        await _movieDeletionService.Delete(id, organisationId, cancellationToken);
        return NoContent();
    }
}
