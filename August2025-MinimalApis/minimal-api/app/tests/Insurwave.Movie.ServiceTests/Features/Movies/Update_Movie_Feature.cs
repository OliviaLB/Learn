using System.Net;
using Insurwave.Movie.Api.Clients.Contracts;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Update_Movie_Feature
{
    [Scenario]
    public async Task Update_Movie_Updates_The_Movie_In_Database_And_Returns()
    {
        await RunUpdateMovieScenario(_updateMovieRequest);
    }

    [Scenario]
    public async Task Update_Movie_Updates_The_Movie_In_Database_And_Returns_When_Title_Is_The_Same()
    {
        var updateRequest = new UpdateMovieRequest
        {
            Title = _movie.Title,
            YearOfRelease = null
        };

        await RunUpdateMovieScenario(updateRequest);
    }

    [Scenario]
    public async Task Update_Movie_Updates_The_Movie_In_Database_And_Returns_When_Only_Title_Is_Supplied()
    {
        var updateRequest = new UpdateMovieRequest
        {
            Title = $"{Guid.NewGuid()}",
            YearOfRelease = null
        };

        await RunUpdateMovieScenario(updateRequest);
    }

    [Scenario]
    public async Task Update_Movie_Updates_The_Movie_In_Database_And_Returns_When_Only_YearOfRelease_Is_Supplied()
    {
        var updateRequest = new UpdateMovieRequest
        {
            Title = null,
            YearOfRelease = Random.Shared.Next(1950, DateTime.Today.Year + 5)
        };

        await RunUpdateMovieScenario(updateRequest);
    }

    private Task RunUpdateMovieScenario(UpdateMovieRequest updateMovieRequest)
    {
        _updateMovieRequest = updateMovieRequest;
        return Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movie_exists_in_the_database(),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.OK),
            and => Movie_is_returned(),
            and => Movie_is_updated_in_the_database());
    }

    [Scenario]
    public async Task Update_Movie_With_Empty_Title_Returns_Bad_Request()
    {
        _updateMovieRequest.Title = string.Empty;

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.TitleNotEmpty));
    }

    [Scenario]
    public async Task Update_Movie_With_Whitespace_Title_Returns_Bad_Request()
    {
        _updateMovieRequest.Title = "         ";

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.TitleNotEmpty));
    }

    [Scenario]
    public async Task Update_Movie_With_Invalid_YearOfRelease_Returns_Bad_Request()
    {
        _updateMovieRequest.YearOfRelease = 1949;

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.InvalidYearOfRelease));
    }

    [Scenario]
    public async Task Update_Movie_When_Movie_Does_Not_Exist_Returns_Not_Found()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.NotFound),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.MovieNotFound(_movieId)));
    }

    [Scenario]
    public async Task Update_Movie_With_Existing_Movie_With_Updated_Title_Returns_Conflict()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movie_exists_in_the_database(),
            and => Movie_exists_in_the_database_with_title(),
            and => Update_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.Conflict),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.UniqueMovie(_updateMovieRequest.Title)));
    }
}
