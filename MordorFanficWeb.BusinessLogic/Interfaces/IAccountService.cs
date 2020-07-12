using MordorFanficWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUser(AppUserModel user, string password);
        Task DeleteUser(string id);
        Task<AppUserModel> GetUserById(string id);
        Task<List<AppUserModel>> GetUsersList();
        Task UpdateUser(AppUserModel user);
    }
}
