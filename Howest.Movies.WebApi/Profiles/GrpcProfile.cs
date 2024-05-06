using AutoMapper;
using Howest.Movies.Dtos.Filters;

namespace Howest.Movies.WebApi.Profiles;

public class GrpcProfile : Profile
{
    public GrpcProfile()
    {
        CreateMap<MoviesFilter, Grpc.MoviesFilter>();
        CreateMap<PaginationFilter, Grpc.PaginationFilter>();
    }
}