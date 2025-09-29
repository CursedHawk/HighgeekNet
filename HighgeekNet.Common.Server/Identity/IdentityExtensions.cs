using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HighgeekNet.Common.Server.Identity
{
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Email, string Password, string Username);

    public static class IdentityExtensions
    {

        public static IEndpointRouteBuilder UseIdentityEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/identity/account/login", async (
                LoginDto login,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager) =>
            {
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
                RegisterDto model,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager) =>
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return Results.BadRequest(result.Errors);

                await signInManager.SignInAsync(user, isPersistent: false);
                return Results.Ok();
            });

            app.MapPost("/identity/account/logout", async (SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            });

            app.MapGet("/identity/account/me", (ClaimsPrincipal user) =>
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
    }
}
