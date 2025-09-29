using HighgeekNet.Common.Server.Data.Models.mcwebapp_application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HighgeekNet.Common.Server.Data.Contexts
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("public");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "user");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("userroles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("userclaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("userlogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("roleclaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("usertokens");
            });
            builder.Entity<MinecraftUser>(entity =>
            {
                entity.ToTable("minecraftuser");
            });
        }
    }
}
