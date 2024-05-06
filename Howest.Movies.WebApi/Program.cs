using Howest.Movies.Data;
using Howest.Movies.WebApi.Extensions;
using Howest.Movies.WebApi.Groups;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthorization()
    .AddAuthentication();

builder.Services
    .AddAutoMapper(typeof(Program), typeof(Howest.Movies.AccessLayer.Installer))
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
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "Bearer {token}",
            Scheme = "Bearer",
        });
        
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowAll", cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    })
    .AddOutputCache(options =>
    {
        options.AddBasePolicy(ocpb =>
        {
            ocpb.Expire(TimeSpan.FromSeconds(10));
        });
    })
    .AddGrpc();

var app = builder.Build();

await app.Services.SetupDatabaseAsync();

// Add routes to the app.
app.AddApiGroup();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program();
