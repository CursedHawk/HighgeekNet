using HighgeekNet.Blazor.Client.Services.Authorization;
using HighgeekNet.Common.Permissions;
using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HighgeekNet.Common.Server.Permissions
{
    public class ServerPermissionsAuthorizationHandler : PermissionsAuthorizationHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LuckPermsService _luckPermsService;

        public ServerPermissionsAuthorizationHandler(UserManager<ApplicationUser> userManager, LuckPermsService luckPermsService)
        {
            _userManager = userManager;
            _luckPermsService = luckPermsService;
        }

        public override Task<bool> CheckPermission(PermissionsAuthorizeAttribute permission, string user)
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizeAttribute requirement)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                if (!await _luckPermsService.HasGroupNode(requirement.Permission, "anonymous"))
                {
                    context.Fail(new AuthorizationFailureReason(this, "Not logged in!"));
                    return;
                }
            }
            else
            {
                if(user.MinecraftUser != null)
                {
                    if(!await _luckPermsService.HasPermissionAsync(requirement.Permission, user.MinecraftUser.Uuid))
                    {
                        context.Fail(new AuthorizationFailureReason(this, "You do not have required permission to do that!"));
                        return;
                    }
                }
                else
                {
                    if (!await _luckPermsService.HasGroupNode(requirement.Permission, "unlinked"))
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
