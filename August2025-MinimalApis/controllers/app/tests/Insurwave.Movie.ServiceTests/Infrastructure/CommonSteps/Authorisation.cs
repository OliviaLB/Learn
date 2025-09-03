using Insurwave.Movie.ServiceTests.Infrastructure.Providers;

namespace Insurwave.Movie.ServiceTests.Infrastructure.CommonSteps;

public static class Authorisation
{
    public static async Task A_user_context_for_a_user_belonging_to_organisation(
        Guid organisationId,
        Guid requestId,
        IServiceProvider serviceProvider)
    {
        await AuthorisationProvider.SetRequestUserToBePartOfOrganisation(
            organisationId,
            requestId,
            serviceProvider);
    }
}
