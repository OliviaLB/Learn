using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Insurwave.Movie.Api.Exceptions;

public class MovieValidationException : Exception
{
    public MovieValidationException(ValidationResult validationResult) : base(FlattenErrors(validationResult.ToDictionary())) { }

    private static string FlattenErrors(IDictionary<string, string[]> errors)
    {
        return string.Join("\n", errors.Select(x => $"{x.Key} : '{x.Value.FirstOrDefault()}'"));
    }
}
