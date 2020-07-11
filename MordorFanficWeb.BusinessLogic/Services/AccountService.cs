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

        public async Task<bool> CreateUser(AppUserModel user, string password)
        {
            var identityResult = await userManager.CreateAsync(user, password).ConfigureAwait(false);
            return identityResult.Succeeded;
        }

        public async Task DeleteUser(string id)
        {
            var user = GetUserById(id).Result;
            if (user != null)
                await userManager.DeleteAsync(user).ConfigureAwait(false);
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

        public void Dispose()
        {
            userManager.Dispose();
        }
    }
}
