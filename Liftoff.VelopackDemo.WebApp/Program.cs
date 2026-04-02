using Liftoff.VelopackDemo.Abstractions;
using Liftoff.VelopackDemo.Pages;
using Liftoff.VelopackDemo.WebApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ICounterViewModel, CounterViewModel>();

await builder.Build().RunAsync();
