using AutoMapper;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;
using Moq;

namespace Howest.Movies.Tests.UnitTests;

public class GenreServiceTests
{
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GenreService _genreService;

    public GenreServiceTests()
    {
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _mapperMock = new Mock<IMapper>();
        _genreService = new GenreService(_mapperMock.Object, _genreRepositoryMock.Object);
    }

    [Fact]
    public async Task FindAsync_ReturnsListOfGenreNames()
    {
        // Arrange
        var genres = new List<Genre> { new() { Name = "Action" }, new() { Name = "Comedy" } };
        _genreRepositoryMock.Setup(gr => gr.FindAsync()).ReturnsAsync(genres);

        // Act
        var result = await _genreService.FindAsync();

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(genres.Select(g => g.Name), result.Data);
        _genreRepositoryMock.Verify(gr => gr.FindAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ReturnsBadRequest_WhenNameIsNullOrWhiteSpace()
    {
        // Arrange
        const string name = "";

        // Act
        var result = await _genreService.CreateAsync(name);

        // Assert
        Assert.Contains(result.Messages, m => m is { Code: nameof(ServiceResultExtensions.BadRequest) });
    }

    [Fact]
    public async Task CreateAsync_ReturnsAlreadyExists_WhenGenreAlreadyExists()
    {
        // Arrange
        const string name = "Test";
        var existingGenre = new Genre { Name = name };
        _genreRepositoryMock.Setup(gr => gr.FindAsync(name)).ReturnsAsync(existingGenre);

        // Act
        var result = await _genreService.CreateAsync(name);

        // Assert
        Assert.Contains(result.Messages, m => m is { Code: nameof(ServiceResultExtensions.AlreadyExists) });
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedGenre_WhenGenreDoesNotExist()
    {
        // Arrange
        const string name = "Test";
        var newGenre = new Genre { Name = name };
        _genreRepositoryMock.Setup(gr => gr.FindAsync(name)).ReturnsAsync(null as Genre);
        _genreRepositoryMock.Setup(gr => gr.AddAsync(It.IsAny<Genre>())).ReturnsAsync(newGenre);
        _mapperMock.Setup(m => m.Map<GenreResult>(It.IsAny<Genre>())).Returns(new GenreResult { Name = name });

        // Act
        var result = await _genreService.CreateAsync(name);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(name, result.Data!.Name);
        _genreRepositoryMock.Verify(gr => gr.AddAsync(It.IsAny<Genre>()), Times.Once);
        _mapperMock.Verify(m => m.Map<GenreResult>(It.IsAny<Genre>()), Times.Once);
    }
}