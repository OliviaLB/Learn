namespace Insurwave.Movie.ServiceTests.Infrastructure.CommonSteps;

public static class ExceptionMessages
{
    public static string MovieNotFound(Guid movieId)
    {
        return $"Movie with id '{movieId}' was not found";
    }

    public static string UniqueMovie(string title)
    {
        return $"Movie already exists with title '{title}'";
    }

    public static string TitleNotEmpty => "'Title' must not be empty.";
    public static string InvalidYearOfRelease => "Year of release' must be on or after 1950.";
}
