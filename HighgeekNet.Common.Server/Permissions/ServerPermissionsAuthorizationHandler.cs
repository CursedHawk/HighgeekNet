using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace HighgeekNet.Common.Server.Permissions
{
    public class ServerPermissionsAuthorizationHandler : PermissionsAuthorizationHandler
    {
        public override Task<bool> CheckPermission(PermissionsAuthorizeAttribute permission, string user)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizeAttribute requirement)
        {

            return Task.CompletedTask;
        }
    }
}
