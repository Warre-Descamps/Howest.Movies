using AutoMapper;

namespace Howest.Movies.AccessLayer.Profiles.Converters;

public class MoviePosterThumbConverter : IValueConverter<Guid, string>
{
    public string Convert(Guid sourceMember, ResolutionContext context)
    {
        return $"/poster-thumbnail/{sourceMember}";
    }
}