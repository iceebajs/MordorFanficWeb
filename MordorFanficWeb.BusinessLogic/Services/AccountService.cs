using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.ViewModels;
using MordorFanficWeb.Persistence.AppDbContext;
using System.Security.Cryptography.X509Certificates;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class AccountService : IAccountService, IDisposable
    {
        protected readonly UserManager<AppUser> userManager;
        protected readonly RoleManager<IdentityRole> roleManager;
        protected readonly AppDbContext appDbContext;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appDbContext = appDbContext;
        }

        // Users
        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            IdentityResult identityResult = await userManager.CreateAsync(user, password).ConfigureAwait(false);
            if (identityResult.Succeeded) 
            { 
                await userManager.AddToRoleAsync(user, "user").ConfigureAwait(false);
                await appDbContext.AccountUsers.AddAsync(new AccountUser { IdentityId = user.Id }).ConfigureAwait(false);
                await appDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            return identityResult;
        }

        public async Task CreateAdminOnInit(AppUser user, string password)
        {
            await userManager.CreateAsync(user, password).ConfigureAwait(false);
            await userManager.AddToRoleAsync(user, "admin").ConfigureAwait(false);
            await appDbContext.AccountUsers.AddAsync(new AccountUser { IdentityId = user.Id }).ConfigureAwait(false);
            await appDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = GetUserById(id).Result;
            var userToDelete = await appDbContext.AccountUsers.FirstOrDefaultAsync(x => x.IdentityId == id);
            if (user.IsMasterAdmin == true)
                return false;
            appDbContext.AccountUsers.Remove(userToDelete);
            await appDbContext.SaveChangesAsync().ConfigureAwait(false);
            await userManager.DeleteAsync(user).ConfigureAwait(false);
            return true;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email).ConfigureAwait(false);
        }
        
        public async Task<AppUser> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<List<AppUser>> GetUsersList()
        {
            return await userManager.Users.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateUser(AppUser user)
        {
            await userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<bool> VerifyUserPassowrd(AppUser user, string password)
        {
            bool result = await userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
            if (result)
            {
                user.LastVisit = DateTime.UtcNow.ToString("g");
                await userManager.UpdateAsync(user).ConfigureAwait(false);
            }                
            return result;
        }
        
        public async Task<IdentityResult> ChangeUserPassword(ChangeUserPasswordViewModel userData)
        {
            var user = await GetUserById(userData.Id).ConfigureAwait(false);
            return await userManager.ChangePasswordAsync(user, userData.OldPassword, userData.NewPassword).ConfigureAwait(false);
        }

        // Roles
        public async Task SetAsAdmin(string id)
        {
            await userManager.AddToRoleAsync(await userManager.FindByIdAsync(id).ConfigureAwait(false), "admin").ConfigureAwait(false);
        }

        public async Task<bool> UnsetAsAdmin(string id)
        {
            var userIdentity = await userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (userIdentity.IsMasterAdmin == true)
                return false;
            await userManager.RemoveFromRoleAsync(userIdentity, "admin").ConfigureAwait(false);
            return true;
        }

        public async Task<IList<string>> GetUserRoles(string id)
        {
            return await userManager.GetRolesAsync(await userManager.FindByIdAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            userManager.Dispose();
        }
    }
}
