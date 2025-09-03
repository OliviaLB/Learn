using System.Net;
using Insurwave.Movie.Api.Clients.Contracts;

namespace Insurwave.Movie.ServiceTests.Features.Movies;

public partial class Get_All_Movies_Feature
{
    [Scenario]
    public async Task Get_Movies_Returns_All_Movies_For_User_Organisation()
    {
        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movies_exists_in_the_database_for_organisation(_organisationId),
            and => Movies_exists_in_the_database_for_organisation(Guid.NewGuid()),
            when => Get_movies_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.OK),
            and => Movies_for_user_organisation_returned());
    }

    [Scenario]
    public async Task Get_Movies_Returns_All_Movies_Containing_The_Title_For_User_Organisation()
    {
        _getMoviesFilters = new GetMoviesFilters { Title = Guid.NewGuid().ToString() };

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movies_exists_in_the_database_for_organisation(_organisationId),
            and => Movies_exists_in_the_database_for_organisation(Guid.NewGuid()),
            when => Get_movies_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.OK),
            and => Movies_for_user_organisation_returned());
    }

    [Scenario]
    public async Task Get_Movies_Returns_All_Movies_Matching_The_Year_Of_Release_For_User_Organisation()
    {
        _getMoviesFilters = new GetMoviesFilters { YearOfRelease = DateTime.Today.Year + 10 };

        await Runner.RunScenarioAsync(
            given => Authorisation.A_user_context_for_a_user_belonging_to_organisation(_organisationId, _requestId, ServiceProvider),
            and => Movies_exists_in_the_database_for_organisation(_organisationId),
            and => Movies_exists_in_the_database_for_organisation(Guid.NewGuid()),
            when => Get_movies_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_httpResponse, HttpStatusCode.OK),
            and => Movies_for_user_organisation_returned());
    }
}
