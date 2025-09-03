using Insurwave.Extensions.Postgres.Extensions;
using Insurwave.Movie.Api.Validation;
using Insurwave.Movie.Persistence.Interfaces.Readers;
using Insurwave.Movie.Persistence.Interfaces.Writers;
using Insurwave.Movie.Persistence.Postgres;
using Insurwave.Movie.Persistence.Postgres.Readers;
using Insurwave.Movie.Persistence.Postgres.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Insurwave.Movie.Api.Extensions;

public static class ServiceDependencyExtensions
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();

        services.AddScoped<IMovieCreationService, MovieCreationService>();
        services.AddScoped<IMovieRetrievalService, MovieRetrievalService>();
        services.AddScoped<IMovieKeyedRetrievalService, MovieKeyedRetrievalService>();
        services.AddScoped<IMovieUpdateService, MovieUpdateService>();
        services.AddScoped<IMovieDeletionService, MovieDeletionService>();

        services.AddScoped<IMovieUniqueCheckService, MovieUniqueCheckService>();

        services.AddScoped<IValidator<CreateMovieRequest>, CreateMovieRequestValidator>();
        services.AddScoped<IValidator<UpdateMovieRequest>, UpdateMovieRequestValidator>();

        return services;
    }

    public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.UsePostgres<MovieDbContext>(configuration, hostEnvironment);

        services.AddScoped<IMovieReader, MovieReader>();
        services.AddScoped<IMovieWriter, MovieWriter>();

        return services;
    }
}
