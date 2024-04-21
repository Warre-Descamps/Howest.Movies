namespace Howest.Movies.WebApi.Services.Abstractions;

public interface IPosterManagementService
{
    Task<IResult> SavePoster(Guid id, IFormFile file);
    Task<IResult> GetPoster(Guid id);
    Task<IResult> GetPosterThumbnail(Guid id);
    void CleanPosters();
}