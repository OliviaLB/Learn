using System.Net;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Get_Movie_By_Id_Feature
{
    [Scenario]
    public async Task Get_Movie_By_Id_Returns_Movie()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movie_exists_in_the_database(),
            when => Get_movie_by_id_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.OK),
            and => Movie_is_returned());
    }

    [Scenario]
    public async Task Get_Movie_By_Id_Returns_Not_Found_When_Movie_Not_Found()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            when => Get_movie_by_id_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.NotFound),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.MovieNotFound(_movieId)));
    }
}
