using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MordorFanficWeb.PresentationAdapters.AccountAdapter
{
    public class AccountAdapter : IAccountAdapter
    {
        private readonly IMapper mapper;
        private readonly IAccountService accService;

        public AccountAdapter(IMapper mapper, IAccountService accService)
        {
            this.mapper = mapper;
            this.accService = accService;
        }

        public async Task<IdentityResult> CreateUser(RegistrationViewModel user, string password)
        {
            return await accService.CreateUser(mapper.Map<AppUserModel>(user), password).ConfigureAwait(false);
        }

        public async Task DeleteUser(string id)
        {
            await accService.DeleteUser(id).ConfigureAwait(false);
        }

        public async Task<GetUserViewModel> GetUserByEmail(string email)
        {
            return mapper.Map<GetUserViewModel>(await accService.GetUserByEmail(email).ConfigureAwait(false));
        }

        public async Task<GetUserViewModel> GetUserById(string id)
        {
            return mapper.Map<GetUserViewModel>(await accService.GetUserById(id).ConfigureAwait(false));
        }

        public async Task<List<UsersListViewModel>> GetUsersList()
        {
            return mapper.Map<List<UsersListViewModel>>(await accService.GetUsersList().ConfigureAwait(false));
        }

        public async Task UpdateUser(UpdateUserViewModel user)
        {
            var userIdentity = await AssignUpdatedUserFields(user).ConfigureAwait(false);
            await accService.UpdateUser(userIdentity).ConfigureAwait(false);
        }

        public async Task<AppUserModel> AssignUpdatedUserFields(UpdateUserViewModel user)
        {
            var userIdentity = await accService.GetUserByEmail(user.Email).ConfigureAwait(false);
            userIdentity.UserName = user.UserName;
            userIdentity.FirstName = user.FirstName;
            userIdentity.LastName = user.LastName;
            return userIdentity;
        }

        public async Task<bool> VerifyUserPassword(AppUserModel user, string password)
        {
            return await accService.VerifyUserPassowrd(user, password).ConfigureAwait(false);
        }

        public async Task<AppUserModel> GetUserIdentity(string email)
        {
            return await accService.GetUserByEmail(email);
        }
    }
}
