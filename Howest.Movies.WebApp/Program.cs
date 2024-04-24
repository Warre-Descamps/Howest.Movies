using Howest.Movies.Sdk;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Howest.Movies.WebApp;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMudServices()
    .InstallMoviesSdk(builder.Configuration);

await builder.Build().RunAsync();