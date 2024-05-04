using AutoMapper;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Models;
using Howest.Movies.Tests.Fakes;
using Moq;

namespace Howest.Movies.Tests.UnitTests;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<FakeUserManager> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _userManagerMock = new Mock<FakeUserManager>();
        _mapperMock = new Mock<IMapper>();
        _reviewService = new ReviewService(_mapperMock.Object, _reviewRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        _reviewRepositoryMock.Setup(rr => rr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Review);

        // Act
        var result = await _reviewService.UpdateAsync(Guid.NewGuid(), Guid.NewGuid(), new ReviewRequest
        {
            Rating = 0,
            Comment = null
        });

        // Assert
        Assert.False(result.IsSuccess);
    }
}