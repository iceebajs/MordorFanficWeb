using MordorFanficWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MordorFanficWeb.ViewModels;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        //Users
        Task<IdentityResult> CreateUser(AppUserModel user, string password);
        Task CreateAdminOnInit(AppUserModel user, string password);
        Task<bool> DeleteUser(string id);
        Task<AppUserModel> GetUserByEmail(string email);
        Task<AppUserModel> GetUserById(string id);
        Task<List<AppUserModel>> GetUsersList();
        Task UpdateUser(AppUserModel user);
        Task<bool> VerifyUserPassowrd(AppUserModel user, string password);
        Task<IdentityResult> ChangeUserPassword(ChangeUserPasswordViewModel userData);

        //Roles
        Task SetAsAdmin(string id);
        Task<bool> UnsetAsAdmin(string id);
        Task<IList<string>> GetUserRoles(string id);
    }
}
