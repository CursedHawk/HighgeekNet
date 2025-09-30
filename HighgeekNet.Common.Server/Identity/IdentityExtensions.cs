using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace HighgeekNet.Common.Server.Identity
{
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Email, string Password, string Username);

    public static class IdentityExtensions
    {

        public static IEndpointRouteBuilder UseIdentityEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/identity/account/login", async (
                HttpContext ctx,
                IAntiforgery antiforgery,
                LoginDto login,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager) =>
            {
                await antiforgery.ValidateRequestAsync(ctx);

                ApplicationUser user = await userManager.FindByEmailAsync(login.Email);
                if (user is null)
                {
                    user = await userManager.FindByNameAsync(login.Email);
                }
                if (user is null)
                {
                    return Results.Unauthorized();
                }

                var result = await signInManager.PasswordSignInAsync(user.UserName, login.Password, false, false);
                return result.Succeeded ? Results.Ok() : Results.Unauthorized();
            });

            app.MapPost("/identity/account/register", async (
                HttpContext ctx,
                IAntiforgery antiforgery,
                RegisterDto model,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager) =>
            {
                await antiforgery.ValidateRequestAsync(ctx);

                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return Results.BadRequest(result.Errors);

                await signInManager.SignInAsync(user, isPersistent: false);
                return Results.Ok();
            });

            app.MapPost("/identity/account/logout", async (
                HttpContext ctx,
                IAntiforgery antiforgery,
                SignInManager<ApplicationUser> signInManager
                ) =>
            {
                await antiforgery.ValidateRequestAsync(ctx);

                await signInManager.SignOutAsync();
                return Results.Ok();
            });

            app.MapGet("/identity/account/me", (
                ClaimsPrincipal user) =>
            {
                if (user.Identity?.IsAuthenticated ?? false)
                {
                    return Results.Ok(new
                    {
                        Name = user.Identity.Name,
                        Claims = user.Claims.Select(c => new { c.Type, c.Value })
                    });
                }

                return Results.Unauthorized();
            });

            return app;
        }

        public static void UseRequestAntiforgery(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

                if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    context.Request.Path.StartsWithSegments("/identity"))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
                        new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });
                }

                await next();
            });
        }
    }
}
