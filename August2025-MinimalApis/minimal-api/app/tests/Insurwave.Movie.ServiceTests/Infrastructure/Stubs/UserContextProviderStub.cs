using System.Collections.Concurrent;
using System.Linq;
using Insurwave.Extensions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Insurwave.Movie.ServiceTests.Infrastructure.Stubs;

public class UserContextProviderStub : IUserContextProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private static ConcurrentDictionary<Guid, UserContext> UserContextsByRequestId { get; } = new();

    private static readonly UserContext s_fallbackUserContext = new()
    {
        UserId = Guid.NewGuid().ToString(),
        OrganisationId = Guid.NewGuid()
    };

    public UserContextProviderStub(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void SetUserForRequest(Guid requestId, UserContext user) => UserContextsByRequestId[requestId] = user;

    public UserContext GetCurrent()
    {
        var request = (_contextAccessor.HttpContext ?? new DefaultHttpContext()).Request;
        var requestId = GetRequestId(request);
        UserContextsByRequestId.TryGetValue(requestId ?? Guid.Empty, out var specifiedUserContext);
        return specifiedUserContext ?? s_fallbackUserContext;
    }

    private static Guid? GetRequestId(HttpRequest request)
    {
        var requestIdString = request.Headers.TryGetValue("X-Request-Id", out var values)
            ? values.FirstOrDefault()
            : null;
        return requestIdString is null ? null : Guid.Parse(requestIdString);
    }
}
