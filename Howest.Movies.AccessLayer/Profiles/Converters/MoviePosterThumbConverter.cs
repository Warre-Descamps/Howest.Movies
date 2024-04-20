using AutoMapper;

namespace Howest.Movies.Services.Profiles.Converters;

public class MoviePosterThumbConverter : IValueConverter<Guid, string>
{
    public string Convert(Guid sourceMember, ResolutionContext context)
    {
        return $"/movies/{sourceMember}/poster-thumbnail";
    }
}