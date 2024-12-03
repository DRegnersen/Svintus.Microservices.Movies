using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Svintus.Movies.Integrations.RecommModel.Client.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Models;
using Svintus.Movies.Integrations.RecommModel.Models.Dto;
using Svintus.Movies.Integrations.RecommModel.Models.Dto.Serialization;

namespace Svintus.Movies.Integrations.RecommModel.Client;

internal sealed class RecommModelClient(HttpClient httpClient) : IRecommModelClient
{
    private const string CreateUserEndpoint = "/api/users/{0}";
    private const string UpdateUserEndpoint = "/api/users/{0}/rates";
    private const string GetRecommendationsEndpoint = "/api/recomms/{0}";
    
    public async Task CreateUserAsync(long userId, UserMovieRate[] rates)
    {
        var endpoint = string.Format(CreateUserEndpoint, userId);

        var request = new CreateUserRequestDto
        {
            Rates = rates.Select(Mapper.Map).ToArray()
        };

        var response = await httpClient.PutAsJsonAsync(endpoint, request, RecommModelJsonContext.Default.Options);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateUserAsync(long userId, UserMovieRate[] rates)
    {
        var endpoint = string.Format(UpdateUserEndpoint, userId);

        var request = new UpdateUserRequestDto
        {
            Rates = rates.Select(Mapper.Map).ToArray()
        };

        var response = await httpClient.PatchAsJsonAsync(endpoint, request, RecommModelJsonContext.Default.Options);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<MovieRecommendation[]> GetRecommendationsAsync(long userId, int? recommsNumber = null)
    {
        var endpoint = string.Format(GetRecommendationsEndpoint, userId);
        
        if (recommsNumber.HasValue)
        {
            endpoint = QueryHelpers.AddQueryString(endpoint, "size", recommsNumber.Value.ToString());
        }

        var response = await httpClient.GetFromJsonAsync<RecommsResponseDto>(endpoint, RecommModelJsonContext.Default.Options);
        return response?.MovieIds.Select(id => new MovieRecommendation(id)).ToArray() ?? [];
    }
}

#region Mappings

file static class Mapper
{
    public static UserMovieRateDto Map(UserMovieRate rateModel) => new()
    {
        MovieId = rateModel.MovieId,
        Rate = rateModel.Rate
    };
}

#endregion