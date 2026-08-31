using Application.Core.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructrue.Identity
{
        public static class IdentitySeed
        {
            public static async Task SeedData(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                ApplicationRole role = new ApplicationRole
                {
                    Id = Guid.Parse("0713f0f4-118e-4727-80a2-f78bb6ecd6af"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "152bfcd6-7c66-4e21-a0e3-aa347341bd93",
                };
                ApplicationRole role2 = new ApplicationRole
                {
                    Id = Guid.Parse("f1d20429-94b3-438c-9bf5-6a818a26f5d5"),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "bf347256-14e9-4b13-a6d6-bf5207948ba6",
                };
            ApplicationRole role3 = new ApplicationRole
            {
                Id = Guid.Parse("ae78a325-6980-4c81-8fb2-61e41aa61b04"),
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                ConcurrencyStamp = "4e384757-74a7-441b-8329-b44c28ad7ad5",
            };
            if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(role);
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(role2);
                }
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await roleManager.CreateAsync(role3);
            }

            ApplicationUser? user = await userManager.FindByNameAsync("Admin");
                if (user == null)
                {
                    var hash = new PasswordHasher<ApplicationUser>();
                ApplicationUser Admin = new ApplicationUser
                {
                    Id = Guid.Parse("bf973477-e6d0-4994-a2f8-c9b4f662c8c0"),
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    RegistrationDate = DateTime.Now,
                    Name = "محمدرضا خدابخشی",
                    IsLogout = false,
                    IsRegisterAdmin = true,                
                };
                    var createResult = await userManager.CreateAsync(Admin,"Admin_1385");
                    if (!createResult.Succeeded)
                        throw new Exception("خطا در ساخت ادمین");
                    user = Admin;
                }
                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            if (!await userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }
        }
}

