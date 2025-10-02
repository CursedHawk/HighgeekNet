using HighgeekNet.Blazor.Client.Services.SignalR.Permissions;
using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HighgeekNet.Blazor.Client.Auth
{
    public class ClientPermissionsAuthorizationHandler : PermissionsAuthorizationHandler
    {
        private readonly PermissionService _permissionService;

        public ClientPermissionsAuthorizationHandler(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public override Task<bool> CheckPermission(PermissionsAuthorizeAttribute permission, string user)
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizeAttribute requirement)
        {
            ClaimsPrincipal claims = context.User;
            if(claims == null)
            {
                if(!await _permissionService.CheckGroupPermissionAsync("anonymous", requirement.Permission))
                {
                    context.Fail(new AuthorizationFailureReason(this, "Not logged in!"));
                    return;
                }
            }
            else
            {
                var uuid = claims.FindFirstValue("mcuuid");
                if (!string.IsNullOrEmpty(uuid))
                {
                    if (!await _permissionService.CheckUserPermissionAsync(uuid, requirement.Permission))
                    {
                        context.Fail(new AuthorizationFailureReason(this, "You do not have required permission to do that!"));
                        return;
                    }
                }
                else
                {
                    if (!await _permissionService.CheckGroupPermissionAsync("unlinked", requirement.Permission))
                    {
                        context.Fail(new AuthorizationFailureReason(this, "You do not have required permission to do that, try connecting your game account!"));
                        return;
                    }
                }
            }
            context.Succeed(requirement);
        }
    }
}
