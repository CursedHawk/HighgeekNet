using Microsoft.AspNetCore.Authorization;

namespace HighgeekNet.Common.Permissions
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionsAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        public PermissionsAuthorizeAttribute(string perm) => Permission = perm;
        public string Permission { get; set; }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }
    }
}
