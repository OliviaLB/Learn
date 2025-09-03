using System;

namespace Insurwave.Movie.Domain.Exceptions;

public class MovieNotFoundException : Exception
{
    public MovieNotFoundException(Guid id)
    {
        Id = id;
    }

    private Guid Id { get; }

    public override string Message => $"Movie with id '{Id}' was not found";
}
