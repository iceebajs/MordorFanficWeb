﻿using MordorFanficWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        //Users
        Task<IdentityResult> CreateUser(AppUserModel user, string password);
        Task CreateAdminOnInit(AppUserModel user, string password);
        Task DeleteUser(string id);
        Task<AppUserModel> GetUserByEmail(string email);
        Task<AppUserModel> GetUserById(string id);
        Task<List<AppUserModel>> GetUsersList();
        Task UpdateUser(AppUserModel user);
        Task<bool> VerifyUserPassowrd(AppUserModel user, string password);

        //Roles
        Task SetAsAdmin(string id);
        Task UnsetAsAdmin(string id);
        Task<IList<string>> GetUserRoles(string id);
    }
}
