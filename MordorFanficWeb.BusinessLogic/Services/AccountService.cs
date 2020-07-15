using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.BusinessLogic.Interfaces;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class AccountService : IAccountService, IDisposable
    {
        protected readonly UserManager<AppUserModel> userManager;

        public AccountService(UserManager<AppUserModel> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateUser(AppUserModel user, string password)
        {             
            return await userManager.CreateAsync(user, password).ConfigureAwait(false); ;
        }

        public async Task DeleteUser(string email)
        {
            var user = GetUserByEmail(email).Result;
            if (user != null)
                await userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        public async Task<AppUserModel> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email).ConfigureAwait(false);
        }
        
        public async Task<AppUserModel> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<List<AppUserModel>> GetUsersList()
        {
            return await userManager.Users.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateUser(AppUserModel user)
        {
            await userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<bool> VerifyUserPassowrd(AppUserModel user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
        }

        public void Dispose()
        {
            userManager.Dispose();
        }
    }
}
