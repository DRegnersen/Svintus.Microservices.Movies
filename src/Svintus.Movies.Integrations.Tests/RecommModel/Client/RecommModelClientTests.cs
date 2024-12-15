using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using RichardSzalay.MockHttp;
using Svintus.Movies.Integrations.RecommModel.Client;
using Svintus.Movies.Integrations.RecommModel.Models;
using Svintus.Movies.Integrations.RecommModel.Models.Dto;
using Svintus.Movies.XunitKit.AutoFixture.Moq;
using Xunit;

namespace Svintus.Movies.Integrations.Tests.RecommModel.Client;

public sealed class RecommModelClientTests
{
    [Theory]
    [AutoMoqData]
    internal async Task CreateUserAsync_UnsuccessfulStatusCode_ThrowsException(long userId, UserMovieRate[] rates)
    {
        // Arrange
        var sut = new RecommModelClient(HttpClientMock.Setup(
            HttpMethod.Put,
            $"/api/users/{userId}",
            HttpStatusCode.BadRequest
        ));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.CreateUserAsync(userId, rates));
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task CreateUserAsync_EverythingIsOk_AwaitsClient(long userId, UserMovieRate[] rates)
    {
        // Arrange
        var sut = new RecommModelClient(HttpClientMock.Setup(
            HttpMethod.Put,
            $"/api/users/{userId}",
            HttpStatusCode.OK
        ));

        // Act & Assert
        await sut.CreateUserAsync(userId, rates);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task UpdateUserAsync_UnsuccessfulStatusCode_ThrowsException(long userId, UserMovieRate[] rates)
    {
        // Arrange
        var sut = new RecommModelClient(HttpClientMock.Setup(
            HttpMethod.Patch,
            $"/api/users/{userId}/rates",
            HttpStatusCode.BadRequest
        ));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.UpdateUserAsync(userId, rates));
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task UpdateUserAsync_EverythingIsOk_AwaitsClient(long userId, UserMovieRate[] rates)
    {
        // Arrange
        var sut = new RecommModelClient(HttpClientMock.Setup(
            HttpMethod.Patch,
            $"/api/users/{userId}/rates",
            HttpStatusCode.OK
        ));

        // Act & Assert
        await sut.UpdateUserAsync(userId, rates);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendationsAsync_EverythingIsOk_ReturnsRecommendations(
        long userId, 
        int recommsNumber,
        RecommsResponseDto clientResponse)
    {
        // Arrange
        var sut = new RecommModelClient(HttpClientMock.Setup(
            HttpMethod.Get,
            QueryHelpers.AddQueryString($"/api/recomms/{userId}", "size", recommsNumber.ToString()),
            HttpStatusCode.OK,
            clientResponse 
        ));

        // Act
        var result = await sut.GetRecommendationsAsync(userId, recommsNumber);
        
        // Assert
        result.Select(r => r.MovieId).Should().BeEquivalentTo(clientResponse.MovieIds);
    }
}

#region Fixtures

file static class HttpClientMock
{
    private const string BaseAddress = "http://test.com";

    public static HttpClient Setup(HttpMethod verb, string endpoint, HttpStatusCode statusCode)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(verb, endpoint).Respond(_ => new HttpResponseMessage(statusCode));

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        return client;
    }

    public static HttpClient Setup<TResponse>(HttpMethod verb, string endpoint, HttpStatusCode statusCode,
        TResponse response)
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(verb, endpoint).Respond(_ => new HttpResponseMessage(statusCode)
        {
            Content = JsonContent.Create(response),
        });

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        return client;
    }
}

#endregion