using System.Collections.Generic;
using Insurwave.Logging.Testing;
using Microsoft.Extensions.Logging;

namespace Insurwave.Movie.ServiceTests.Infrastructure.CommonSteps;

public static class Logs
{
    public static Task There_should_be_a_log_with(
        LogLevel logLevel,
        string expectedMessage,
        TestLogger logger)
    {
        logger.Should().HaveLogsAtLevel(logLevel)
            .WithMessage(expectedMessage);

        return Task.CompletedTask;
    }

    public static Task There_should_be_a_log_with(
        LogLevel logLevel,
        string expectedMessage,
        IDictionary<string, object> expectedScopeValues,
        TestLogger logger)
    {
        logger.Should().HaveLogsAtLevel(logLevel)
            .WithMessage(expectedMessage)
            .WithScopeValues(expectedScopeValues);

        return Task.CompletedTask;
    }

    public static Task There_should_not_be_a_log_with(
        LogLevel logLevel,
        string expectedMessage,
        IDictionary<string, object> expectedScopeValues,
        TestLogger logger)
    {
        logger.Should().NotHaveLogWith(logLevel, expectedMessage, expectedScopeValues);
        return Task.CompletedTask;
    }
}
