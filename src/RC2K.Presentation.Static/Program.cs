using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RC2K.Presentation.Static;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["AppBaseUrl"]) });

builder.Services.AddSingleton<RC2K.Presentation.Shared.ViewModels.HeaderViewModel>(); 
builder.Services.AddMudServices();

await builder.Build().RunAsync();
