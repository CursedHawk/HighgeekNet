using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace HighgeekNet.Blazor.Client.Auth
{

    public class PermisssionAuthorizeRouteView : RouteView
    {
        [Inject] private IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Parameter] public RenderFragment<AuthenticationState>? NotAuthorized { get; set; }
        [Parameter] public RenderFragment? Authorizing { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorizeAttr = RouteData.PageType.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .Cast<AuthorizeAttribute>()
                .ToArray();
            var allowAnonymous = RouteData.PageType.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
            var permissionAuthorize = RouteData.PageType.GetCustomAttributes(typeof(PermissionsAuthorizeAttribute), true)
                .Cast<PermissionsAuthorizeAttribute>()
                .ToArray();

            if (allowAnonymous || (!authorizeAttr.Any() && !permissionAuthorize.Any()))
            {
                // No restrictions
                base.Render(builder);
                return;
            }

            // We need async check
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)((builder2) =>
            {
                builder2.OpenComponent<PermissionAuthorizeRouteViewCore>(0);
                builder2.AddAttribute(1, "RouteData", RouteData);
                builder2.AddAttribute(2, "DefaultLayout", DefaultLayout);
                builder2.AddAttribute(3, "AuthorizeAttributes", authorizeAttr);
                builder2.AddAttribute(4, "PermissionAuthorizeAttributes", permissionAuthorize);
                builder2.AddAttribute(5, "NotAuthorized", NotAuthorized);
                builder2.AddAttribute(6, "Authorizing", Authorizing);
                builder2.CloseComponent();
            }));
            builder.CloseComponent();
        }


        private async Task<bool> CheckAuthorizationAsync(AuthenticationState authState)
        {
            // Get all attributes
            var authorizeAttributes = RouteData.PageType
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
                .Cast<AuthorizeAttribute>()
                .ToList();

            var customAttributes = RouteData.PageType
                .GetCustomAttributes(typeof(PermissionsAuthorizeAttribute), inherit: true)
                .Cast<PermissionsAuthorizeAttribute>()
                .ToList();

            if (!authorizeAttributes.Any() && !customAttributes.Any())
                return true;

            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
                return false;


            foreach (var attr in authorizeAttributes)
            {
                if (!string.IsNullOrEmpty(attr.Policy))
                {
                    var result = await AuthorizationService.AuthorizeAsync(user, attr.Policy);
                    if (!result.Succeeded) return false;
                }

                if (!string.IsNullOrEmpty(attr.Roles))
                {
                    if (!attr.Roles.Split(',').Any(role => user.IsInRole(role.Trim())))
                        return false;
                }
            }


            foreach (var custom in customAttributes)
            {
                var result = await AuthorizationService.AuthorizeAsync(
                    user,
                    null,
                    custom);

                if (!result.Succeeded)
                    return false;
            }

            return true;
        }
    }
}