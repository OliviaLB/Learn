using Insurwave.Extensions.Authentication;
using Insurwave.Movie.ServiceTests.Infrastructure.Constants;
using Insurwave.Movie.ServiceTests.Infrastructure.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Providers;

internal static class AuthorisationProvider
{
    public static Task<UserContext> SetRequestUserToBePartOfOrganisation(
        Guid organisationId,
        Guid requestId,
        IServiceProvider serviceProvider,
        string userId = UserIds.DefaultUserId)
    {
        var userContextProvider = (UserContextProviderStub)serviceProvider.GetRequiredService<IUserContextProvider>();
        var userContext = new UserContext { UserId = userId, OrganisationId = organisationId };
        userContextProvider.SetUserForRequest(requestId, userContext);
        return Task.FromResult(userContext);
    }
}
