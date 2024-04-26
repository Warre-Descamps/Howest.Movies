using Blazored.LocalStorage;
using Howest.Movies.Sdk;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Howest.Movies.WebApp;
using Howest.Movies.WebApp.Models;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMudServices(config =>
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    })
    .AddBlazoredLocalStorage()
    .InstallMoviesSdk<TokenStore>(builder.Configuration);

await builder.Build().RunAsync();