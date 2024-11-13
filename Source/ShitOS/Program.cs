using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShitOS.Application;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<ApplicationRouting>(".app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
