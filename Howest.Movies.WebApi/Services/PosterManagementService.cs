using System.Globalization;
using System.Reflection;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.WebApi.Services.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Howest.Movies.WebApi.Services;

public class PosterManagementService : IPosterManagementService
{
    private static readonly string PostersPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "posters");
    private const string FileFormat = "yyyy-MM-ddTHHmmss.fffffff";
    private const string Thumbnail = "-thumbnail";
    
    private readonly IMovieService _movieService;

    public PosterManagementService(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public static string GetTimeStamp(Guid id)
    {
        var result = GetFileInfo(id);
        
        return result.IsSuccess
            ? result.Data.fileTime.Ticks.ToString()
            : string.Empty;
    }
    
    private static ServiceResult<(string fullPath, string fileName, DateTime fileTime, string fileExtension)> GetFileInfo(Guid id)
    {
        var path = Path.Combine(PostersPath, id.ToString());
        if (!Directory.Exists(path))
            return new ServiceResult<(string, string, DateTime, string)>().NotFound();
        
        var fileInfo = Directory.GetFiles(path)
            .Select(f => new FileInfo(f))
            .Where(t => !t.Name.EndsWith(Thumbnail+t.Extension))
            .Select(f => (
                fullPath: f.FullName,
                fileName: f.Name[..^f.Extension.Length],
                fileTime: DateTime.ParseExact(f.Name[..^f.Extension.Length], FileFormat, CultureInfo.InvariantCulture),
                fileExtension: f.Extension
            ))
            .MaxBy(s => s.fileTime);
        
        return fileInfo.fileName is null
            ? new ServiceResult<(string, string, DateTime, string)>().NotFound()
            : fileInfo;
    }
    
    private async Task<ServiceResult<(string fullPath, string fileName, string fileExtension)>> GetFileInfoAsync(Guid id)
    {
        if (!await _movieService.ExistsAsync(id))
            return new ServiceResult<(string, string, string)>().NotFound();

        var info = GetFileInfo(id);
        return info.IsSuccess
            ? new ServiceResult<(string, string, string)>((info.Data.fullPath, info.Data.fileName, info.Data.fileExtension))
            : new ServiceResult<(string, string, string)>().NotFound();
    }

    private bool IsFileLocked(FileInfo file)
    {
        FileStream? stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
        }
        catch (IOException)
        {
            return true;
        }
        finally
        {
            stream?.Close();
        }

        return false;
    }

    public async Task<IResult> GetPosterThumbnail(Guid id)
    {
        var fileInfo = await GetFileInfoAsync(id);
        if (!fileInfo.IsSuccess)
            return Results.File(Path.Combine(PostersPath, "default.jpg"), "image/jpg", "default.jpg");
        
        var file = File.OpenRead(fileInfo.Data.fullPath[..^fileInfo.Data.fileExtension.Length] + Thumbnail + fileInfo.Data.fileExtension);
        return Results.File(file, $"image/{fileInfo.Data.fileExtension[1..]}", id + Thumbnail + fileInfo.Data.fileExtension);
    }

    public async Task<IResult> GetPoster(Guid id)
    {
        var fileInfo = await GetFileInfoAsync(id);
        if (!fileInfo.IsSuccess)
            return Results.File(Path.Combine(PostersPath, "default.jpg"), "image/jpg", "default.jpg");
        
        var file = File.OpenRead(fileInfo.Data.fullPath);
        return Results.File(file, $"image/{fileInfo.Data.fileExtension[1..]}", id + fileInfo.Data.fileExtension);
    }

    public async Task<IResult> SavePoster(Guid id, IFormFile file)
    {
        if (!await _movieService.ExistsAsync(id))
            return Results.NotFound(new ServiceResult().NotFound());
        
        if (file.Length == 0)
            return Results.BadRequest(new ServiceResult().BadRequest("File is empty!"));
        var fileInfo = new FileInfo(file.FileName);
        if (fileInfo.Extension != ".jpg")
            return Results.BadRequest(new ServiceResult().BadRequest("Invalid file extension. Only .jpg is allowed!"));
        
        var path = Path.Combine(PostersPath, id.ToString());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path = Path.Combine(path, DateTime.UtcNow.ToString(FileFormat) + fileInfo.Extension);
        await using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        
        using (var image = await Image.LoadAsync(path))
        {
            if (image.Size is { Width: <= 200, Height: <= 300 }) return Results.Ok();
                    
            var ratio = image.Size.Height / (image.Size.Width * 1f);
            image.Mutate(x => x.Resize(200, (int) (200 * ratio)));
            await image.SaveAsync(path[..^fileInfo.Extension.Length] + Thumbnail + fileInfo.Extension);
        }
        
        return Results.Created($"/poster/{id}", new ServiceResult());
    }

    public void CleanPosters()
    {
        if (!Directory.Exists(PostersPath))
            return;

        foreach (var directory in Directory.GetDirectories(PostersPath))
        {
            var info = new DirectoryInfo(directory);
            var max = info
                .EnumerateFiles()
                .Where(f => !f.Name.EndsWith(Thumbnail + f.Extension))
                .MaxBy(f => DateTime.ParseExact(f.Name[..^f.Extension.Length], FileFormat, CultureInfo.InvariantCulture));
            if (max is null)
                continue;
            foreach (var file in info
                         .EnumerateFiles()
                         .Where(f => !IsFileLocked(f))
                         .Where(f => f.Name != max.Name &&
                                     f.Name != max.Name[..^f.Extension.Length] + Thumbnail + f.Extension))
            {
                file.Delete();
            }
        }
    }
}
