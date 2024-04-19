using Howest.Movies.Data;
using Howest.Movies.Models;
using Howest.Movies.Services;
using Howest.Movies.WebApi.Groups;
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
app.AddApiGroup();

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
