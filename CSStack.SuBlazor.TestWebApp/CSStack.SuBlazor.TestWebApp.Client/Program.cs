using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static CSStack.SuBlazor.SuServiceBuilder;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSuBlazorService();

await builder.Build().RunAsync();
