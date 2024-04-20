using AutoMapper;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;
using Howest.Movies.Services.Profiles.Converters;

namespace Howest.Movies.Services.Profiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Movie, MovieResult>()
            .ForMember(mr => mr.MoviePoster, opt => opt.ConvertUsing(new MoviePosterConverter(), m => m.Id));
        
        CreateMap<Movie, MovieDetailResult>()
            .IncludeBase<Movie, MovieResult>()
            .ForMember(mr => mr.AddedBy, opt => opt.MapFrom(m => m.AddedByUser));

        CreateMap<Genre, GenreResult>();
    }
}