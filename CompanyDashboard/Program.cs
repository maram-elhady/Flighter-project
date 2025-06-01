using CompanyDashboard.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;

namespace CompanyDashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddTransient<CookieHandler>();

            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
            
            builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

            
            builder.Services.AddScoped<IAdminManagement, AdminManagement>();


            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7284/api/") });

            //builder.Services.AddHttpClient("Auth", options =>
            //options.BaseAddress = new Uri("https://localhost:7284") //api url
            //)
            //.AddHttpMessageHandler<CookieHandler>();

            var environment = builder.HostEnvironment.IsProduction() ? "Production" : "Development";


            using var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var config = await client.GetFromJsonAsync<AppConfig>($"Json/appsettings.{environment}.json");

            if (config == null || string.IsNullOrEmpty(config.ApiBaseUrl))
            {
                throw new ArgumentNullException($"ApiBaseUrl is missing from appsettings.{environment}.json");
            }

            builder.Services.AddHttpClient("Auth", options =>
            {
                options.BaseAddress = new Uri(config.ApiBaseUrl);
            })
            .AddHttpMessageHandler<CookieHandler>();


            await builder.Build().RunAsync();
        }
    }
}
