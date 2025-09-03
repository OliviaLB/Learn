namespace Insurwave.Movie.ServiceTests.Features.HealthChecks;

public partial class Health_Check_Feature : FeatureFixture
{
    private HttpResponseMessage? _response;
    private static HttpClient Client => ServiceWebApplicationFactory.Instance.CreateClient();


    private async Task the_readiness_status_endpoint_is_called()
    {
        _response = await Client.GetAsync("status/ready");
    }

    private async Task the_health_status_endpoint_is_called()
    {
        _response = await Client.GetAsync("status/health");
    }
}
