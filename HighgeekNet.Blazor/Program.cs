using HighgeekNet.Blazor.Components;
using HighgeekNet.Common.Server.Config;
using HighgeekNet.Common.Server.Data.Contexts;
using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using HighgeekNet.Common.Server.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace HighgeekNet.Blazor
{
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Email, string Password, string Username);

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigProvider.Configure(builder.Configuration);

            var connectionStringPgsqlApplication = ConfigProvider.GetConnectionString("PostgresApplicationConnection");
            var connectionStringPgsqlKeys = ConfigProvider.GetConnectionString("PostgresKeysConnection");
            var connectionStringMysqlMain = ConfigProvider.GetConnectionString("MysqlMaindbConnection");


            builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionStringPgsqlApplication, b => b.MigrationsAssembly("Highgeek.Blazor")), ServiceLifetime.Scoped);
            builder.Services.AddDbContext<KeysContext>(options => options.UseNpgsql(connectionStringPgsqlKeys, b => b.MigrationsAssembly("Highgeek.Blazor")), ServiceLifetime.Scoped);
            builder.Services.AddDbContext<McserverMaindbContext>(options => options.UseMySql(connectionStringMysqlMain, MariaDbServerVersion.AutoDetect(connectionStringMysqlMain), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDataProtection().PersistKeysToDbContext<KeysContext>().SetApplicationName("mcWebApp");

            // Add MudBlazor services
            builder.Services.AddMudServices();
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddCascadingAuthenticationState();

            // Identity (no Razor Pages)
            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme,
        options =>
        {
            options.Cookie.Name = ".AspNet.SharedCookie";

            if (builder.Environment.IsProduction())
            {
                options.Cookie.Domain = ".highgeek.eu";
            }

        });


            builder.Services.AddAuthorization();
            //builder.Services.AddAuthorizationBuilder();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.UseIdentityEndpoints();

            app.Run();
        }
    }
}
