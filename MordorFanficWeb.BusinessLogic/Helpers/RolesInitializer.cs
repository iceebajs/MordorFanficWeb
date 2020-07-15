using MordorFanficWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MordorFanficWeb.BusinessLogic.Interfaces;
using System;

namespace MordorFanficWeb.BusinessLogic.Helpers
{
    public static class RolesInitializer
    {
        public static async Task InitializeAsync(IAccountService accountService, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.com";
            string defaultPassword = "Qwerty123";

            if (await roleManager.FindByNameAsync("admin").ConfigureAwait(false) == null)
                await roleManager.CreateAsync(new IdentityRole("admin")).ConfigureAwait(false);
            if (await roleManager.FindByNameAsync("user").ConfigureAwait(false) == null)
                await roleManager.CreateAsync(new IdentityRole("user")).ConfigureAwait(false);
            if(await accountService.GetUserByEmail(adminEmail).ConfigureAwait(false) == null)
            {
                AppUserModel admin = new AppUserModel
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "admin",
                    LastName = "admin",
                    CreationDate = DateTime.UtcNow.ToString("g"),
                    LastVisit = DateTime.UtcNow.ToString("g"),
                    AccountStatus = true,
                    IsMasterAdmin = true
                };

                await accountService.CreateAdminOnInit(admin, defaultPassword).ConfigureAwait(false);
            }
        }
    }
}
