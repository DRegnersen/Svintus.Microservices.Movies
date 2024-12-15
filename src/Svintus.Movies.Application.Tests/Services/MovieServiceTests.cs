using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Svintus.Movies.Application.Models;
using Svintus.Movies.Application.Models.Options;
using Svintus.Movies.Application.Models.Results;
using Svintus.Movies.Application.Services;
using Svintus.Movies.DataAccess.Models;
using Svintus.Movies.DataAccess.Services.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Client.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Models;
using Svintus.Movies.XunitKit.AutoFixture.Moq;
using Xunit;

namespace Svintus.Movies.Application.Tests.Services;

public sealed class MovieServiceTests
{
    [Theory]
    [AutoMoqData]
    internal async Task GetRandomMoviesAsync_MoviesNumberNotPassed_ReturnsDefaultNumber(
        Movie[] movies,
        [Frozen] MovieServiceOptions options,
        [Frozen] Mock<IMovieRepository> movieRepository,
        MovieService sut)
    {
        // Arrange
        movieRepository
            .Setup(m => m.GetMoviesAsync(It.IsAny<int?>()))
            .ReturnsAsync(movies.ToList());

        // Act
        var result = await sut.GetRandomMoviesAsync();

        // Assert
        result.Select(m => m.Id).Should().BeEquivalentTo(movies.Select(m => m.MovieId));
        movieRepository.Verify(m => m.GetMoviesAsync(options.DefaultRandomMoviesNumber), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetRandomMoviesAsync_MoviesNumberPassed_ReturnsMoviesNumber(
        int moviesNumber,
        Movie[] movies,
        [Frozen] Mock<IMovieRepository> movieRepository,
        MovieService sut)
    {
        // Arrange
        movieRepository
            .Setup(m => m.GetMoviesAsync(It.IsAny<int?>()))
            .ReturnsAsync(movies.ToList());

        // Act
        var result = await sut.GetRandomMoviesAsync(moviesNumber);

        // Assert
        result.Select(m => m.Id).Should().BeEquivalentTo(movies.Select(m => m.MovieId));
        movieRepository.Verify(m => m.GetMoviesAsync(moviesNumber), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task RateMoviesAsync_RatingNotFound_CreateRating(
        long chatId,
        MovieRateModel[] rates,
        UserRating createdRating,
        [Frozen] Mock<IRatingRepository> ratingRepository,
        [Frozen] Mock<IRecommModelClient> modelClient,
        MovieService sut)
    {
        // Arrange
        ratingRepository
            .Setup(m => m.FindRatingAsync(It.IsAny<long>()))
            .ReturnsAsync((UserRating?)null);

        ratingRepository
            .Setup(m => m.CreateRatingAsync(It.IsAny<long>(), It.IsAny<MovieRate[]>()))
            .ReturnsAsync(createdRating);

        modelClient
            .Setup(m => m.CreateUserAsync(It.IsAny<long>(), It.IsAny<UserMovieRate[]>()))
            .Returns(Task.CompletedTask);
        
        // Act
        await sut.RateMoviesAsync(chatId, rates);

        // Assert
        ratingRepository.Verify(m => m.CreateRatingAsync(It.IsAny<long>(), It.IsAny<MovieRate[]>()), Times.Once);
        modelClient.Verify(m => m.CreateUserAsync(It.IsAny<long>(), It.IsAny<UserMovieRate[]>()), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task RateMoviesAsync_RatingFound_UpdateRating(
        long chatId,
        MovieRateModel[] rates,
        UserRating foundRating,
        UserRating createdRating,
        [Frozen] Mock<IRatingRepository> ratingRepository,
        [Frozen] Mock<IRecommModelClient> modelClient,
        MovieService sut)
    {
        // Arrange
        ratingRepository
            .Setup(m => m.FindRatingAsync(It.IsAny<long>()))
            .ReturnsAsync(foundRating);

        ratingRepository
            .Setup(m => m.AddRatesAsync(It.IsAny<long>(), It.IsAny<MovieRate[]>()))
            .ReturnsAsync(createdRating);

        modelClient
            .Setup(m => m.UpdateUserAsync(It.IsAny<long>(), It.IsAny<UserMovieRate[]>()))
            .Returns(Task.CompletedTask);
        
        // Act
        await sut.RateMoviesAsync(chatId, rates);

        // Assert
        ratingRepository.Verify(m => m.AddRatesAsync(It.IsAny<long>(), It.IsAny<MovieRate[]>()), Times.Once);
        modelClient.Verify(m => m.UpdateUserAsync(It.IsAny<long>(), It.IsAny<UserMovieRate[]>()), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_RatingNotFound_ReturnsError(
        long chatId,
        int moviesNumber,
        [Frozen] Mock<IRatingRepository> ratingRepository,
        MovieService sut)
    {
        // Arrange
        ratingRepository
            .Setup(m => m.FindRatingAsync(It.IsAny<long>()))
            .ReturnsAsync((UserRating?)null);
        
        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId, moviesNumber);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Code.Should().Be(ResultCode.ChatIdNotFound);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_MoviesNumberNotPassed_ReturnsDefaultNumber(
        long chatId,
        UserRating foundRating,
        MovieRecommendation[] recommendations,
        Movie[] movies,
        [Frozen] MovieServiceOptions options,
        [Frozen] Mock<IRatingRepository> ratingRepository,
        [Frozen] Mock<IRecommModelClient> modelClient,
        [Frozen] Mock<IMovieRepository> movieRepository,
        MovieService sut)
    {
        // Arrange
        ratingRepository
            .Setup(m => m.FindRatingAsync(It.IsAny<long>()))
            .ReturnsAsync(foundRating);

        modelClient
            .Setup(m => m.GetRecommendationsAsync(It.IsAny<long>(), It.IsAny<int?>()))
            .ReturnsAsync(recommendations);

        movieRepository
            .Setup(m => m.GetMoviesAsync(It.IsAny<long[]>()))
            .ReturnsAsync(movies.ToList());
        
        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Select(m => m.Id).Should().BeEquivalentTo(movies.Select(m => m.MovieId));
        
        modelClient.Verify(m => m.GetRecommendationsAsync(It.IsAny<long>(), options.DefaultRecommendedMoviesNumber), Times.Once);
    }
    
    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_MoviesNumberPassed_ReturnsMoviesNumber(
        long chatId,
        int moviesNumber,
        UserRating foundRating,
        MovieRecommendation[] recommendations,
        Movie[] movies,
        [Frozen] Mock<IRatingRepository> ratingRepository,
        [Frozen] Mock<IRecommModelClient> modelClient,
        [Frozen] Mock<IMovieRepository> movieRepository,
        MovieService sut)
    {
        // Arrange
        ratingRepository
            .Setup(m => m.FindRatingAsync(It.IsAny<long>()))
            .ReturnsAsync(foundRating);

        modelClient
            .Setup(m => m.GetRecommendationsAsync(It.IsAny<long>(), It.IsAny<int?>()))
            .ReturnsAsync(recommendations);

        movieRepository
            .Setup(m => m.GetMoviesAsync(It.IsAny<long[]>()))
            .ReturnsAsync(movies.ToList());
        
        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId, moviesNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Select(m => m.Id).Should().BeEquivalentTo(movies.Select(m => m.MovieId));
        
        modelClient.Verify(m => m.GetRecommendationsAsync(It.IsAny<long>(), moviesNumber), Times.Once);
    }
}