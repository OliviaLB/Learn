using System.Threading;
using System.Threading.Tasks;

namespace Insurwave.Movie.Api.Clients;

/// <summary>
/// Client interaction with the template service
/// </summary>
public interface ITemplateClient
{
    /// <summary>
    /// Add the two numbers together
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> Add(int a, int b, CancellationToken cancellationToken);
}

