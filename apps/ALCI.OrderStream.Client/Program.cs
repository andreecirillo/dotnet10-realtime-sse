using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ALCI.OrderStream.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

await builder.Build().RunAsync();