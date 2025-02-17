using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessEquipmentShop.Data.Seed
{
    public static class IdentityInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                string[] roleNames = { "Admin", "User" };

                // Ensure roles exist
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Check if the admin user already exists in the database
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser == null)
                {
                    var user = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        FullName = "Admin User",
                        EmailConfirmed = true
                    };

                    // Hash the password using PasswordHasher
                    var hasher = new PasswordHasher<User>();
                    user.PasswordHash = hasher.HashPassword(user, "Admin123!");

                    // Add the user to the database manually
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();

                    // Assign the Admin role
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    // If the admin user already exists, reset their password
                    string newPassword = "Admin123!";
                    var passwordHasher = new PasswordHasher<User>();
                    adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, newPassword);

                    dbContext.Users.Update(adminUser);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
