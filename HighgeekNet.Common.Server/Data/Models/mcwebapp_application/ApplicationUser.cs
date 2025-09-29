using Microsoft.AspNetCore.Identity;

namespace HighgeekNet.Common.Server.Data.Models.mcwebapp_application
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public MinecraftUser? MinecraftUser { get; set; }
    }

}
