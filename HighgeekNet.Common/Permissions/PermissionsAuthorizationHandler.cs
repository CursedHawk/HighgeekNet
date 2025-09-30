using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighgeekNet.Common.Permissions
{
    public abstract class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsAuthorizeAttribute>
    {
        public abstract Task<bool> CheckPermission(PermissionsAuthorizeAttribute permission, string user);
    }
}
