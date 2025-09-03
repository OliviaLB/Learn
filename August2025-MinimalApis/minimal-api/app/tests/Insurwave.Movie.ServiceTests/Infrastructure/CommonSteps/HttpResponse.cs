using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Insurwave.Movie.ServiceTests.Infrastructure.CommonSteps;

public static class HttpResponse
{
    public static Task Status_code_is(
        HttpResponseMessage responseMessage,
        HttpStatusCode statusCode)
    {
        responseMessage.StatusCode.Should().Be(statusCode);

        return Task.CompletedTask;
    }

    public static async Task Content_Should_Not_Contain(
        HttpResponseMessage responseMessage,
        string expectedMessage)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        Regex.Unescape(content).Should().NotContain(expectedMessage);
    }

    public static async Task Is_Problem_Details_With(
        HttpResponseMessage responseMessage,
        string expectedMessage)
    {
        var content = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        content.Should().NotBeNull();
        content!.Title.Should().Contain(expectedMessage);
    }
}
