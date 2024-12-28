using AppLidra.Client;
using AppLidra.Client.Services;
using AppLidra.Client.Handlers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationHandler();
builder.Services.AddBlazoredLocalStorage();



await builder.Build().RunAsync();
