using FluentValidation;
using Insurwave.Movie.Api.Clients.Contracts;

namespace Insurwave.Movie.Api.Validation;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        When(p => p.Title is not null, () =>
        {
            RuleFor(request => request.Title).NotEmpty();
        });

        When(p => p.YearOfRelease is not null, () =>
        {
            RuleFor(request => request.YearOfRelease)
                .GreaterThanOrEqualTo(1950)
                .WithMessage("'Year of release' must be on or after 1950.");
        });
    }
}
