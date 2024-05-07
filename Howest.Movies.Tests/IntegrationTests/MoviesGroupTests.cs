using System.Net;
using Howest.Movies.Tests.Factories;

namespace Howest.Movies.Tests.IntegrationTests;

public class MoviesGroupTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public MoviesGroupTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMovies_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/movie");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}