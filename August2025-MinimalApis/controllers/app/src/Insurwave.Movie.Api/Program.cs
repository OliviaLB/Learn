using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Insurwave.Extensions.Authentication.Extensions;
using Insurwave.Extensions.Telemetry;
using Insurwave.HealthChecks;
using Insurwave.Movie.Api.Extensions;
using Insurwave.Movie.Api.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Insurwave.Movie.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder
            .Configuration
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();

        builder
            .WebHost
            .UseSentry(o => o.TracesSampler = context =>
            {
                return context.CustomSamplingContext.GetValueOrDefault("__HttpPath") switch
                {
                    "/status/health" => 0.0,
                    "/status/ready" => 0.0,
                    _ => 0.2
                };
            });

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.ConfigureHealthChecks(builder.Configuration);
        builder.Services.AddJwtBearerAuthentication(builder.Configuration);
        builder.Services.AddDefaultMvc()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });
        builder.Services.AddHeaderPropagation(options => { options.Headers.Add("Authorization"); });
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddSwagger(builder.Configuration);
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Services.AddInsurwaveDefaultTelemetry(builder.Configuration);
        builder.Services.AddCors();
        builder.Services.AddServiceDependencies()
            .AddPersistenceDependencies(builder.Configuration, builder.Environment);

        var app = builder.Build();

        app.UseExceptionHandler();

        var applicationName = builder.Configuration.GetValue<string>("Application:Name");
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                if (!builder.Environment.IsDevelopmentEnvironment())
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = $"https://{httpReq.Host.Value}/{applicationName}" } };
                }
            });
        });
        app.UseSwaggerUI(c =>
        {
            var swaggerEndpoint = builder.Environment.IsDevelopmentEnvironment() ? "/swagger/v1/swagger.json" : "swagger/v1/swagger.json";
            c.RoutePrefix = builder.Environment.IsDevelopmentEnvironment() ? "swagger" : string.Empty;
            c.SwaggerEndpoint(swaggerEndpoint, "Insurwave Movie Service v1");
        });
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseInsurwaveAuthorization();
        app.UseHeaderPropagation();

        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.MapControllers();

        app.UseInsurwaveHealthChecks(builder.Configuration);

        app.Run();
    }
}
