using System.Net;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Delete_Movie_Feature
{
    [Scenario]
    public async Task Delete_Movie_By_Id_Deletes_Movie()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movie_exists_in_the_database(),
            when => Delete_movie_by_id_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.NoContent),
            and => Movie_is_deleted_in_the_database());
    }

    [Scenario]
    public async Task Delete_Movie_Returns_Not_Found_When_Movie_Does_Not_Exist()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            when => Delete_movie_by_id_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.NotFound),
            and => HttpResponse.Is_Problem_Details_With(_httpResponse, ExceptionMessages.MovieNotFound(_movieId)));
    }
}
