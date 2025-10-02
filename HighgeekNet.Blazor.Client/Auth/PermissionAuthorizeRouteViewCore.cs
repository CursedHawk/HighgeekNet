using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

namespace HighgeekNet.Blazor.Client.Auth
{
    public class PermissionAuthorizeRouteViewCore : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        [Inject] private IAuthorizationService AuthorizationService { get; set; } = default!;
        [Parameter] public RouteData RouteData { get; set; } = default!;
        [Parameter] public Type? DefaultLayout { get; set; }
        [Parameter] public AuthorizeAttribute[] AuthorizeAttributes { get; set; } = Array.Empty<AuthorizeAttribute>();
        [Parameter] public PermissionsAuthorizeAttribute[] PermissionAuthorizeAttributes { get; set; } = Array.Empty<PermissionsAuthorizeAttribute>();
        [Parameter] public RenderFragment<AuthenticationState>? NotAuthorized { get; set; }
        [Parameter] public RenderFragment? Authorizing { get; set; }

        private AuthenticationState? _authState;
        private bool _authorizing;
        private bool _authorized;

        protected override async Task OnParametersSetAsync()
        {
            _authorizing = true;
            _authState = await AuthStateTask;
            var user = _authState.User;

                // Built-in AuthorizeAttribute checks
                bool passedAuthorize = true;
                foreach (var attr in AuthorizeAttributes)
                {
                    if (!string.IsNullOrEmpty(attr.Policy))
                    {
                        var result = await AuthorizationService.AuthorizeAsync(user, null, attr.Policy);
                        if (!result.Succeeded)
                        {
                            passedAuthorize = false;
                            break;
                        }
                    }
                    else if (!string.IsNullOrEmpty(attr.Roles))
                    {
                        if (!attr.Roles.Split(',').Any(user.IsInRole))
                        {
                            passedAuthorize = false;
                            break;
                        }
                    }
                }

                // Custom checks
                bool passedCustom = true;
                foreach (var custom in PermissionAuthorizeAttributes)
                {
                    // TODO: improve custom check here
                    var result = await AuthorizationService.AuthorizeAsync(user, null, custom);
                    if (!result.Succeeded)
                    {
                        passedCustom = false;
                        break;
                    }
                }

                _authorized = passedAuthorize && passedCustom;

            _authorizing = false;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_authorizing)
            {
                if (Authorizing != null)
                    Authorizing(builder);
                return;
            }

            if (_authorized && _authState != null)
            {
                builder.OpenComponent<RouteView>(0);
                builder.AddAttribute(1, "RouteData", RouteData);
                builder.AddAttribute(2, "DefaultLayout", DefaultLayout);
                builder.CloseComponent();
            }
            else if (_authState != null)
            {
                if (NotAuthorized != null)
                    NotAuthorized(_authState)(builder);
            }
        }
    }
}
