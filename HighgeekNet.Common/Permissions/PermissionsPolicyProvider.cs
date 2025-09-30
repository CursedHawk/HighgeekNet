using HighgeekNet.Common.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HighgeekNet.Common.Permissions
{
    public class PermissionsPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
                                FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
                                FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            try
            {
                var policy = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme);

                policy.AddRequirements(new PermissionsAuthorizeAttribute(policyName));

                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }
            catch (Exception ex)
            {
                return Task.FromResult<AuthorizationPolicy?>(null);
            }
        }
    }
}
