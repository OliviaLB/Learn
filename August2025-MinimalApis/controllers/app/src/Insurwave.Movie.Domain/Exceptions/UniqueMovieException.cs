using System;

namespace Insurwave.Movie.Domain.Exceptions;

public class UniqueMovieException : Exception
{
    public UniqueMovieException(string title)
    {
        Title = title;
    }

    private string Title { get; }

    public override string Message => $"Movie already exists with title '{Title}'";
}
