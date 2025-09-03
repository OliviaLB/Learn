using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Insurwave.Movie.Api.Exceptions;
using Insurwave.Movie.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurwave.Movie.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occured, please try again later";

        switch (exception)
        {
            case MovieValidationException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case MovieNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case UniqueMovieException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;
        }

        var problemDetails = new ProblemDetails { Title = message, Status = (int)statusCode };
        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
