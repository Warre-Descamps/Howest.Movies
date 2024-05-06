using AutoMapper;
using Grpc.Core;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.WebApi.Extensions;
using Howest.Movies.WebApi.Grpc;

namespace Howest.Movies.WebApi.Services;

public class MovieGrpcService : MoviesService.MoviesServiceBase
{
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;

    public MovieGrpcService(IMapper mapper, IMovieService movieService)
    {
        _movieService = movieService;
        _mapper = mapper;
    }
    
    public override async Task<Response> GetMovies(GetMoviesRequest request, ServerCallContext context)
    {
        var filter = _mapper.Map<Dtos.Filters.MoviesFilter>(request.MoviesFilter);
        var pagination = _mapper.Map<Dtos.Filters.PaginationFilter>(request.PaginationFilter);
        var result = await _movieService.FindAsync(filter, pagination);
        
        var data = new PaginatedResponse();
        if (result.IsSuccess)
        {
            result.Data!.Items.UpdatePosterInfo(context);
            data.Movies.AddRange(result.Data!.Items.Select(m => new Movie
            {
                Id = m.Id.ToString(),
                Title = m.Title,
                Poster = m.Poster
            }));
        }
        
        return new Response
        {
            IsSuccess = result.IsSuccess,
            Messages = { result.Messages.Select(sm => new ResponseMessage
            {
                Message = sm.Message,
                Type = (uint)sm.Type,
                Code = sm.Code
            }) },
            Data = data
        };
    }
}