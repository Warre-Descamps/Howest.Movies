using System.Linq.Expressions;
using AutoMapper;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;
using Howest.Movies.Tests.Fakes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Howest.Movies.Tests.UnitTests;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly Mock<FakeUserManager> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _userManagerMock = new Mock<FakeUserManager>();
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _mapperMock = new Mock<IMapper>();
        _movieService = new MovieService(_mapperMock.Object, _movieRepositoryMock.Object, _genreRepositoryMock.Object, _reviewRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsMovieDetailResult()
    {
        // Arrange
        var movie = new Movie
        {
            Title = "Test",
            Description = null,
            ReleaseDate = default,
            Director = null
        };
        var movieDetailResult = new MovieDetailResult
        {
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            Director = movie.Director,
            Poster = null
        };
        _movieRepositoryMock.Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(movie);
        _mapperMock.Setup(m => m.Map<MovieDetailResult>(It.IsAny<Movie>())).Returns(movieDetailResult);
        
        // Act
        var result = await _movieService.FindByIdAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task CreateAsync_ReturnsBadRequest_WhenTitleIsNullOrWhiteSpace()
    {
        // Arrange
        var request = new MovieRequest
        {
            Title = "",
            Description = null,
            ReleaseDate = default,
            Director = null
        };

        // Act
        var result = await _movieService.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.Contains(result.Messages, m => m is { Code: nameof(ServiceResultExtensions.BadRequest) });
    }

    [Fact]
    public async Task CreateAsync_ReturnsAlreadyExists_WhenMovieAlreadyExists()
    {
        // Arrange
        var request = new MovieRequest
        {
            Title = "Test",
            Description = null,
            ReleaseDate = default,
            Director = null
        };
        var existingMovie = new Movie
        {
            Title = request.Title,
            Description = null,
            ReleaseDate = default,
            Director = null
        };
        _movieRepositoryMock.Setup(mr => mr.FindAsync(request.Title)).ReturnsAsync(existingMovie);

        // Act
        var result = await _movieService.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.Contains(result.Messages, m => m is { Code: nameof(ServiceResultExtensions.AlreadyExists) });
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedMovie_WhenMovieDoesNotExist()
    {
        // Arrange
        var request = new MovieRequest
        {
            Title = "Test",
            Description = null,
            ReleaseDate = default,
            Director = null
        };
        var newMovie = new Movie
        {
            Title = request.Title,
            Description = null,
            ReleaseDate = default,
            Director = null
        };
        _movieRepositoryMock.Setup(mr => mr.FindAsync(request.Title)).ReturnsAsync(null as Movie);
        _movieRepositoryMock.Setup(mr => mr.AddAsync(It.IsAny<Movie>())).ReturnsAsync(newMovie);
        _mapperMock.Setup(m => m.Map<MovieDetailResult>(It.IsAny<Movie>())).Returns(new MovieDetailResult
        {
            Title = request.Title,
            Description = null,
            ReleaseDate = default,
            Director = null,
            Poster = null
        });

        // Act
        var result = await _movieService.CreateAsync(request, Guid.NewGuid());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Title, result.Data!.Title);
    }
    
    [Fact]
    public async Task GetReviewsAsync_ReturnsListOfReviews()
    {
        // Arrange
        var reviews = new List<Review>
        { 
            new()
            {
                Comment = "Great movie!",
                Rating = 0
            },
            new()
            {
                Comment = "Not bad",
                Rating = 0
            }
        };
        _reviewRepositoryMock.Setup(rr => rr.FindAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(reviews);

        // Act
        var result = await _movieService.GetReviewsAsync(Guid.NewGuid(), new PaginationFilter
        {
            From = 0,
            Size = reviews.Count
        });

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(reviews.Count, result.Data.Size);
        _reviewRepositoryMock.Verify(rr => rr.FindAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        var request = new ReviewRequest { Rating = 5, Comment = "Great movie!" };
        var user = new User { Id = Guid.NewGuid() };
        _movieRepositoryMock.Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Movie);

        // Act
        var result = await _movieService.AddReviewAsync(Guid.NewGuid(), request, user.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Messages, m => m.Code == nameof(ServiceResultExtensions.NotFound));
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsAlreadyExists_WhenReviewAlreadyExists()
    {
        // Arrange
        var request = new ReviewRequest { Rating = 5, Comment = "Great movie!" };
        var existingReview = new Review { Rating = request.Rating, Comment = request.Comment };
        var user = new User { Id = Guid.NewGuid() };
        _movieRepositoryMock.Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Movie
        {
            Title = null,
            Description = null,
            ReleaseDate = default,
            Director = null
        });
        _reviewRepositoryMock.Setup(rr => rr.GetByUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(existingReview);

        // Act
        var result = await _movieService.AddReviewAsync(Guid.NewGuid(), request, user.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Messages, m => m.Code == nameof(ServiceResultExtensions.AlreadyExists));
    }
}