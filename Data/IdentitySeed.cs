using Microsoft.AspNetCore.Identity;
using pets.Models;
using pets.Security;
using System.Security.Claims;

namespace pets.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // 1) Create roles
            var roles = new[] { "Admin", "Manager", "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // 2) Assign permissions to roles (Claims)
            await AddPermissionsToRole(roleManager, "Admin", Permissions.All);

            await AddPermissionsToRole(roleManager, "Manager", new[]
            {
                Permissions.Dashboard_View,
                Permissions.Tools_View, Permissions.Tools_Edit,
                Permissions.Appointments_View,
                Permissions.Pets_View,
                Permissions.Owners_View
            });

            await AddPermissionsToRole(roleManager, "User", new[]
            {
                Permissions.Dashboard_View,
                Permissions.Tools_View
            });

            // 3) Create admin user (username=admin password=admin)
            var admin = await userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                admin = new User
                {
                    UserName = "admin",
                    FullName = "System Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var create = await userManager.CreateAsync(admin, "admin");
                if (create.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // 4) Create demo user (username=demo password=demo) (اختياري)
            var demo = await userManager.FindByNameAsync("demo");
            if (demo == null)
            {
                demo = new User
                {
                    UserName = "demo",
                    FullName = "Demo User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var create = await userManager.CreateAsync(demo, "demo");
                if (create.Succeeded)
                {
                    await userManager.AddToRoleAsync(demo, "User");
                }
            }
        }

        private static async Task AddPermissionsToRole(
            RoleManager<IdentityRole> roleManager,
            string roleName,
            IEnumerable<string> permissions)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null) return;

            var existing = await roleManager.GetClaimsAsync(role);

            foreach (var p in permissions)
            {
                if (!existing.Any(c => c.Type == "Permission" && c.Value == p))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", p));
                }
            }
        }
    }
}
