using BlazoredLocalStorageService = Blazored.LocalStorage.ILocalStorageService;
using CustomLocalStorageService = OnlineCoursesPlatform.CLIENT.Services.ILocalStorageService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineCoursesPlatform.CLIENT;
using OnlineCoursesPlatform.CLIENT.Services;
using OnlineCoursesPlatform.CLIENT.Services.Impl;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registrar AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddBlazoredLocalStorage();

// Usa los alias definidos anteriormente para evitar ambigüedad
builder.Services.AddScoped<CustomLocalStorageService, LocalStorageService>();

await builder.Build().RunAsync();