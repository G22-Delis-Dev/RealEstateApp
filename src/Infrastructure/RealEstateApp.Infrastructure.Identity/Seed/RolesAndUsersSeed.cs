using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seed;

public static class RolesAndUsersSeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // ── Crear roles ──
        string[] roles = { "Admin", "Agent", "Client", "Developer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // ── Usuario Admin semilla ──
        await CreateUserIfNotExists(userManager,
            new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Default",
                UserName = "adminuser",
                Email = "admin@realestate.com",
                EmailConfirmed = true,
                IsActive = true,
                IdCard = "000-0000000-0"
            },
            password: "Admin123!",
            role: "Admin");

        // ── Usuario Developer semilla ──
        await CreateUserIfNotExists(userManager,
            new ApplicationUser
            {
                FirstName = "Developer",
                LastName = "Default",
                UserName = "devuser",
                Email = "dev@realestate.com",
                EmailConfirmed = true,
                IsActive = true
            },
            password: "Dev123!",
            role: "Developer");
    }

    private static async Task CreateUserIfNotExists(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string password,
        string role)
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email!);
        if (existingUser != null) return;

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
