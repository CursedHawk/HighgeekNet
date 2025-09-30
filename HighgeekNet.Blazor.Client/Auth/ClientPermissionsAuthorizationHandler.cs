using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace HighgeekNet.Blazor.Client.Auth
{
    public class ClientPermissionsAuthorizationHandler : PermissionsAuthorizationHandler
    {
        public override Task<bool> CheckPermission(PermissionsAuthorizeAttribute permission, string user)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizeAttribute requirement)
        {
            throw new NotImplementedException();
        }
    }
}
