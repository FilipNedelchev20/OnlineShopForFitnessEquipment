using FitnessEquipmentShop.Data;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
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
                var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<User>>();

                string[] roleNames = { "Admin", "User" };

                // Ensure roles exist
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Check if the admin user already exists
                var admin = await userManager.FindByEmailAsync("admin@gmail.com");
                if (admin == null)
                {
                    User admin2 = new User();
                    admin2.FullName = "Tomaaaaaaaaa";
                    await userStore.SetUserNameAsync(admin2, "admin@gmail.com", default);
                    await ((IUserEmailStore<User>)userStore).SetEmailAsync(admin2, "admin@gmail.com", default);
                    await ((IUserEmailStore<User>)userStore).SetEmailConfirmedAsync(admin2, true, default);
                    await userManager.CreateAsync(admin2, "Pr0toty1pe@1");
                    await userManager.AddToRoleAsync(admin2, "Admin");
                }
                else
                {
                    // Reset password manually
                    string newPassword = "Admin123!";
                    var passwordHasher = new PasswordHasher<User>();
                    admin.PasswordHash = passwordHasher.HashPassword(admin, newPassword);

                    dbContext.Users.Update(admin);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
