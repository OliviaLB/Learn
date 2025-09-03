using System.Net;
using LightBDD.Framework;

namespace Insurwave.Movie.ServiceTests.Features.HealthChecks;

[FeatureDescription(@"Check on service readiness and health status")]
public partial class Health_Check_Feature
{
    [Scenario]
    public async Task Ready_Status_Should_Succeed()
    {
        await Runner.RunScenarioAsync(
            when => the_readiness_status_endpoint_is_called(),
            then => HttpResponse.Content_Should_Not_Contain(_response, "Unhealthy"),
            then => HttpResponse.Status_code_is(_response!, HttpStatusCode.OK));
    }

    [Scenario]
    public async Task Health_Status_Should_Succeed()
    {
        await Runner.RunScenarioAsync(
            when => the_health_status_endpoint_is_called(),
            then => HttpResponse.Status_code_is(_response!, HttpStatusCode.OK));
    }
}
