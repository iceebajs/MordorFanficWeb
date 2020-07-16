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
        Task DeleteUser(string id);
        Task<GetUserViewModel> GetUserByEmail(string email);
        Task<GetUserViewModel> GetUserById(string id);
        Task<List<UsersListViewModel>> GetUsersList();
        Task UpdateUser(UpdateUserViewModel user);
        Task UpdateUserStatus(UpdateUserStatusViewModel user);
        Task<bool> VerifyUserPassword(AppUserModel user, string password);
        Task<AppUserModel> GetUserIdentity(string email);

        //Roles
        Task SetAsAdmin(string id);
        Task UnsetAsAdmin(string id);
        Task<IList<string>> GetUserRoles(string id);
    }
}
