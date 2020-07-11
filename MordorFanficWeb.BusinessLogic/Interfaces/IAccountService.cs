using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CreateUser(AppUserModel user, string password);
        Task DeleteUser(string id);
        Task<AppUserModel> GetUserById(string id);
        Task<List<AppUserModel>> GetUsersList();
        Task UpdateUser(AppUserModel user);
    }
}
