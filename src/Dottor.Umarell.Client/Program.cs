using Dottor.Umarell.Client;
using Dottor.Umarell.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddScoped<IMessageBoxService,    MessageBoxService>();
builder.Services.AddScoped<IBuildingSitesService, BuildingSitesProxyService>();
builder.Services.AddScoped<IWeatherProxyService,  WeatherProxyService>();

await builder.Build().RunAsync();
