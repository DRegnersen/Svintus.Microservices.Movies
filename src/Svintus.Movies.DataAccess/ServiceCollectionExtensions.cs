using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Svintus.Movies.DataAccess.Models.Options;
using Svintus.Movies.DataAccess.Services;
using Svintus.Movies.DataAccess.Services.Abstractions;

namespace Svintus.Movies.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");
        
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        
        services.Configure<MongoRepositoryOptions>(configuration.GetSection("MongoDB"));

        services
            .AddSingleton<IMovieRepository, MovieRepository>()
            .AddSingleton<IRatingRepository, RatingRepository>();

        return services;
    }
}