using AutoMapper;
using Howest.Movies.AccessLayer.Profiles.Converters;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;

namespace Howest.Movies.AccessLayer.Profiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Movie, MovieResult>()
            .ForMember(mr => mr.Poster, opt => opt.ConvertUsing(new MoviePosterThumbConverter(), m => m.Id));
        
        CreateMap<Movie, MovieDetailResult>()
            .ForMember(mr => mr.Poster, opt => opt.ConvertUsing(new MoviePosterConverter(), m => m.Id))
            .ForMember(mr => mr.AddedBy, opt => opt.MapFrom(m => m.AddedBy))
            .ForMember(mr => mr.Genres, opt => opt.MapFrom(m => m.Genres.Where(mg => mg.Genre != null).Select(g => g.Genre!.Name).ToList()));

        CreateMap<User, UserResult>();

        CreateMap<Genre, GenreResult>();

        CreateMap<Review, ReviewResult>()
            .ForMember(rr => rr.Reviewer, opt => opt.MapFrom(r => r.Reviewer!.UserName));

    }
}