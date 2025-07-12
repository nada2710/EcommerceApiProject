using EcommerceProjectDAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectDAL.SeedData
{
    public class SeedData
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];
            var adminUserName = "AdminNada";


            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }


            var adminUserByEmail = await userManager.FindByEmailAsync(adminEmail);
            var adminUserByName = await userManager.FindByNameAsync(adminUserName);

            if (adminUserByEmail == null && adminUserByName == null)
            {
                var user = new Admin
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    PhoneNumber = "01011571434",
                    EmailConfirmed = true,
                    IsActive = true,
                    VerificationCode = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create admin user: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists. Skipping seeding...");
            }
        }
    }
}
