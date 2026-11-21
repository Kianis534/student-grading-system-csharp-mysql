using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentGradingSystem.Web;
using StudentGradingSystem.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// In WebAssembly a singleton lives exactly as long as the browser tab: one private demo per visitor.
builder.Services.AddSingleton<DemoSession>();
builder.Services.AddSingleton<DialogService>();

await builder.Build().RunAsync();
