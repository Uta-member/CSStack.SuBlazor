using static CSStack.SuBlazor.SuServiceBuilder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSuBlazorService();

await builder.Build().RunAsync();
