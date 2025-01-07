using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ModularShell;
using ModularShell.AssemblyLoading;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Module services
builder.Services.AddTransient<AssemblyLoader>();
builder.Services.AddSingleton<AssemblyPersistence>();

var app =  builder.Build();

// Load modules here
var loader = app.Services.GetRequiredService<AssemblyLoader>();
var assemblies = await loader.LoadAdditionalAssemblies();

// Store loaded assemblies, used in App.razor to add them as AdditionalAssemblies for router.
var persistence = app.Services.GetRequiredService<AssemblyPersistence>();
persistence.Assemblies.AddRange(assemblies);

await app.RunAsync();