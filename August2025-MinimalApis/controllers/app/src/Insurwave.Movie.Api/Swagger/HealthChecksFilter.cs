using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Insurwave.Movie.Api.Swagger;

[ExcludeFromCodeCoverage]
public class HealthChecksFilter : IDocumentFilter
{
    private const string Tag = "Api Health";
    private readonly IConfiguration _configuration;

    public HealthChecksFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
    {
        openApiDocument?.Paths.Add(_configuration["HealthCheck:ReadyPath"], GetReadyPathItem());
        openApiDocument?.Paths.Add(_configuration["HealthCheck:StatusPath"], GetStatusPathItem());
    }

    private static OpenApiPathItem GetStatusPathItem()
    {
        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = Tag });
        operation.Responses.Add(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture),
            GetStatusResponse());
        var pathItem = new OpenApiPathItem();
        pathItem.AddOperation(OperationType.Get, operation);

        return pathItem;
    }

    private static OpenApiPathItem GetReadyPathItem()
    {
        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = Tag });
        operation.Responses.Add(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture), GetReadyResponse());
        var pathItem = new OpenApiPathItem();
        pathItem.AddOperation(OperationType.Get, operation);

        return pathItem;
    }

    private static OpenApiResponse GetStatusResponse()
    {
        var properties = new Dictionary<string, OpenApiSchema>
        {
            {"status", new OpenApiSchema {Type = "string"}},
            {"totalDuration", new OpenApiSchema {Type = "number"}},
            {"entries", new OpenApiSchema {Type = "object"}}
        };

        var response = new OpenApiResponse();
        response.Content.Add(MediaTypeNames.Application.Json,
            new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = true,
                    Properties = properties
                }
            });

        return response;
    }

    private static OpenApiResponse GetReadyResponse()
    {
        var response = new OpenApiResponse();
        response.Content.Add(MediaTypeNames.Application.Json,
            new OpenApiMediaType { Schema = new OpenApiSchema { Type = "string" } });
        return response;
    }
}
