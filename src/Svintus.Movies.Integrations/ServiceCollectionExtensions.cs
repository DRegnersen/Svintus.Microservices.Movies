using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Svintus.Movies.Integrations.RecommModel.Client;
using Svintus.Movies.Integrations.RecommModel.Client.Abstractions;

namespace Svintus.Movies.Integrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();

        services
            .AddHttpClient<RecommModelClient>(client =>
            {
                client.BaseAddress = configuration.GetValue<Uri>("RestServices:RecommModel:Url");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("svintus");
            });

        services.AddScoped<IRecommModelClient>(sp => new RecommModelClientProxy(
            sp.GetRequiredService<RecommModelClient>(),
            sp.GetRequiredService<IFeatureManager>()
        ));

        return services;
    }
}