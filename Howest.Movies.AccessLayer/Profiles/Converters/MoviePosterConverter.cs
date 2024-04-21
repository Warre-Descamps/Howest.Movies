using AutoMapper;

namespace Howest.Movies.AccessLayer.Profiles.Converters;

public class MoviePosterConverter : IValueConverter<Guid, string>
{
    public string Convert(Guid sourceMember, ResolutionContext context)
    {
        return $"/{sourceMember}/poster";
    }
}