using FluentValidation;
using Insurwave.Movie.Api.Clients.Contracts;

namespace Insurwave.Movie.Api.Validation;

public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(request => request.Title).NotNull().NotEmpty();
        RuleFor(request => request.YearOfRelease).NotNull()
            .GreaterThanOrEqualTo(1950)
            .WithMessage("'Year of release' must be on or after 1950.");
    }
}
