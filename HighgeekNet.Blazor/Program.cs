using HighgeekNet.Blazor.Components;
using HighgeekNet.Blazor.Services.SignalR;
using HighgeekNet.Common.Permissions;
using HighgeekNet.Common.Server.Config;
using HighgeekNet.Common.Server.Data.Contexts;
using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using HighgeekNet.Common.Server.Identity;
using HighgeekNet.Common.Server.Permissions;
using HighgeekNet.Common.Server.Services;
using HighgeekNet.Common.Server.Services.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace HighgeekNet.Blazor
{
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
            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<RedisListenerService>();
            builder.Services.AddHostedService(
                provider => provider.GetRequiredService<RedisListenerService>());
            builder.Services.AddSingleton<IRedisUpdateService, RedisUpdateService>();

            builder.Services.AddSingleton<ISnackService, SnackService>();

            // Identity
            builder.Services.AddCascadingAuthenticationState();
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
            options.LoginPath = new PathString("/login");
            options.LogoutPath = new PathString("/logout");
            options.AccessDeniedPath = new PathString("/denied");
            options.Cookie.Name = ".AspNet.SharedCookie";

            if (builder.Environment.IsProduction())
            {
                options.Cookie.Domain = ".highgeek.eu";
            }

        });

            builder.Services.AddSingleton<LuckPermsService>();

            builder.Services.AddScoped<IAuthorizationHandler, ServerPermissionsAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("group.sa", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.sa")));
                options.AddPolicy("group.default", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("group.default")));
                options.AddPolicy("connectedaccount", policy => policy.Requirements.Add(new PermissionsAuthorizeAttribute("connectedaccount")));
            });

            //builder.Services.AddAuthorizationBuilder();
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });


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
            
            
            using (var scope = app.Services.CreateScope())
            {
                var snackService = scope.ServiceProvider.GetRequiredService<ISnackService>();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.UseRequestAntiforgery();
            app.UseIdentityEndpoints();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapControllers();

            app.MapHub<SnackHub>("/hubs/snack");

            app.Run();
        }
    }
}
