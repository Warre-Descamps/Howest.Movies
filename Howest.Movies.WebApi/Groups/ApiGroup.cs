namespace Howest.Movies.WebApi.Groups;

public static class ApiGroup
{
    public static WebApplication AddApiGroup(this WebApplication app)
    {
        var api = app.MapGroup("/api");
        api.AddMovies();
        api.AddGenres();
        api.AddIdentity();

        return app;
    }
}