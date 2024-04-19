using Howest.Movies.Data;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Models;
using Howest.Movies.Services;
using Howest.Movies.Services.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddJwtBearer();

builder.Services
    .AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<MovieDbContext>();

builder.Services
    .AddAutoMapper(typeof(Program), typeof(Installer))
    .InstallServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

builder.Services.AddCors();

var app = builder.Build();

await app.Services.SeedDbAsync();

// Add routes to the app.
app.UsePathBase(new PathString("/api"));

app.MapGroup("/identity").MapIdentityApi<User>();

app.MapGet("/movies", async ([FromQuery] MoviesFilter? moviesFilter, [FromQuery] PaginationFilter? paginationFilter, HttpRequest request, IMovieService movieService) =>
{
    var filter = new MoviesFilter
    {
        Query = request.Query["query"],
        Genres = request.Query["genres"].OfType<string>().ToArray(),
    };
    var pagination = new PaginationFilter();
    if (request.Query.ContainsKey("from") && int.TryParse(request.Query["from"], out var from))
        pagination.From = from;
    if (request.Query.ContainsKey("size") && int.TryParse(request.Query["size"], out var size))
        pagination.Size = size;
    
    var movies = await movieService.FindAsync(filter, pagination);
    
    return Results.Ok(movies);
});

app.MapGet("/movies/{id:guid}", async () =>
{
    
});

app.MapGet("/movies/{id:guid}/poster", async () =>
{

});

app.MapGet("/movies/top", async () =>
{

});

app.MapPost("/movies", async () =>
{

});

app.MapPost("/movies/{id:guid}/rate", async () =>
{

});

app.MapPost("/genre", async () =>
{

});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
