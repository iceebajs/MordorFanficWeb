using Microsoft.AspNetCore.Identity;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.AccountAdapter
{
    public interface IAccountAdapter
    {
        Task<IdentityResult> CreateUser(RegistrationViewModel user, string password);
        Task<bool> DeleteUser(string id);
        Task<AppUserViewModel> GetUserByEmail(string email);
        Task<AppUserViewModel> GetUserById(string id);
        Task<List<UsersListViewModel>> GetUsersList();
        Task UpdateUser(UpdateUserViewModel user);
        Task<bool> UpdateUserStatus(UpdateUserStatusViewModel user);
        Task<bool> VerifyUserPassword(AppUser user, string password);
        Task<AppUser> GetUserIdentity(string email);
        Task<IdentityResult> ChangeUserPassword(ChangeUserPasswordViewModel userData);

        //Roles
        Task SetAsAdmin(string id);
        Task<bool> UnsetAsAdmin(string id);
        Task<IList<string>> GetUserRoles(string id);
    }
}
