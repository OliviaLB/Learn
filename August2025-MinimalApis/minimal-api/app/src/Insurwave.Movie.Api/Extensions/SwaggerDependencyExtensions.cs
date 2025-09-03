using System.Collections.Generic;
using Insurwave.Movie.Api.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Insurwave.Movie.Api.Extensions;

public static class SwaggerDependencyExtensions
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Insurwave Movie Service Api",
                    Version = "v1",
                    Description = "Insurwave Movie Service Api",
                    TermsOfService = new Uri("https://www.insurwave.com"),
                    Contact = new OpenApiContact { Name = "Insurwave", Email = "tbc@insurwave.com" }
                });
            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme}
                    },
                    new List<string>()
                }
            });
            options.EnableAnnotations();
            options.DocumentFilter<HealthChecksFilter>(configuration);
        });
    }
}
