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
        Task<IdentityResult> CreateUser(AppUser user, string password);
        Task CreateAdminOnInit(AppUser user, string password);
        Task<bool> DeleteUser(string id);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> GetUserById(string id);
        Task<List<AppUser>> GetUsersList();
        Task UpdateUser(AppUser user);
        Task<bool> VerifyUserPassowrd(AppUser user, string password);
        Task<IdentityResult> ChangeUserPassword(ChangeUserPasswordViewModel userData);
        Task<int> GetAccountId(string id);

        //Roles
        Task SetAsAdmin(string id);
        Task<bool> UnsetAsAdmin(string id);
        Task<IList<string>> GetUserRoles(string id);
    }
}
