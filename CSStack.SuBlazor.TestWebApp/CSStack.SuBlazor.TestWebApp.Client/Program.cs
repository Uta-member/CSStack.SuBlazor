using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static CSStack.SuBlazor.Bootstrap.SuBSServiceBuilder;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSuBlazorBootstrapService();

await builder.Build().RunAsync();
