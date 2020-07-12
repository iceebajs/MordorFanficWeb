﻿using Microsoft.AspNetCore.Identity;
using MordorFanficWeb.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.AccountAdapter
{
    public interface IAccountAdapter
    {
        Task<IdentityResult> CreateUser(RegistrationViewModel user, string password);
        Task DeleteUser(string id);
        Task<GetUserViewModel> GetUserById(string id);
        Task<List<UsersListViewModel>> GetUsersList();
        Task UpdateUser(UpdateUserViewModel user);        
    }
}