using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.Movies.Application.Models.Options;
using Svintus.Movies.Application.Services;
using Svintus.Movies.Application.Services.Abstractions;
using Svintus.Movies.DataAccess;
using Svintus.Movies.Integrations;

namespace Svintus.Movies.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccess(configuration);
        
        services.AddIntegrations(configuration);
        
        services
            .Configure<MovieServiceOptions>(configuration.GetSection("Services:MovieService"))
            .AddScoped<IMovieService, MovieService>();

        return services;
    }
}