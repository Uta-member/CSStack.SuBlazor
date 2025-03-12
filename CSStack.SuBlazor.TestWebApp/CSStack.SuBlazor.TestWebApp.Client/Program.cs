using static CSStack.SuBlazor.SuServiceBuilder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSuBlazorService(dialogOptions: new CSStack.SuBlazor.SuDialogService.Options() { BackgroundStyle = "background-color: rgba(0, 0, 0, 0.3);" } );

await builder.Build().RunAsync();
