using Microsoft.FeatureManagement;
using Svintus.Movies.Integrations.RecommModel.Client.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Models;

namespace Svintus.Movies.Integrations.RecommModel.Client;

internal sealed class RecommModelClientProxy(IRecommModelClient client, IFeatureManager featureManager) : IRecommModelClient
{
    private const string FeatureFlag = "RecommModelIsAvailable";
    
    public async Task CreateUserAsync(long userId, UserMovieRate[] rates)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            await client.CreateUserAsync(userId, rates);
        }
    }

    public async Task UpdateUserAsync(long userId, UserMovieRate[] rates)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            await client.UpdateUserAsync(userId, rates);
        }
    }

    public async Task<MovieRecommendation[]> GetRecommendationsAsync(long userId, int? recommsNumber = null)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            return await client.GetRecommendationsAsync(userId, recommsNumber);
        }
        
        throw new InvalidOperationException("Operation is not available");
    }
}