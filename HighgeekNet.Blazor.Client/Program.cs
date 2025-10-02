using HighgeekNet.Blazor.Client.Auth;
using HighgeekNet.Blazor.Client.Services.SignalR;
using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace HighgeekNet.Blazor.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddMudServices();
            builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? builder.HostEnvironment.BaseAddress)
    });

            builder.Services.AddScoped<SnackService>();

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("group.sa", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.sa")));
                options.AddPolicy("group.default", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.default")));
                options.AddPolicy("connectedaccount", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("connectedaccount")));
            });
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, ClientPermissionsAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();
            builder.Services.AddCascadingAuthenticationState();

            await builder.Build().RunAsync();
        }
    }
}
