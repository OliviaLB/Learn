using System.Net;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Create_Movie_Feature
{
    [Scenario]
    public async Task Create_Movie_Creates_The_Movie_In_Database_And_Returns()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.Created),
            and => Movie_is_returned(),
            and => Movie_is_created_in_the_database());
    }

    [Scenario]
    public async Task Create_Movie_With_Null_Title_Returns_Bad_Request()
    {
        _createMovieRequest.Title = null;

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.TitleNotEmpty));
    }

    [Scenario]
    public async Task Create_Movie_With_Empty_Title_Returns_Bad_Request()
    {
        _createMovieRequest.Title = string.Empty;

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.TitleNotEmpty));
    }

    [Scenario]
    public async Task Create_Movie_With_Whitespace_Title_Returns_Bad_Request()
    {
        _createMovieRequest.Title = "         ";

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.TitleNotEmpty));
    }

    [Scenario]
    public async Task Create_Movie_With_Invalid_YearOfRelease_Returns_Bad_Request()
    {
        _createMovieRequest.YearOfRelease = 1949;

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.BadRequest),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.InvalidYearOfRelease));
    }

    [Scenario]
    public async Task Create_Movie_With_Movie_With_Existing_Title_Returns_Conflict()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movie_exists_in_the_database_with_title(),
            and => Create_movie_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.Conflict),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.UniqueMovie(_createMovieRequest.Title)));
    }
}
