using System.Collections.Generic;
using System.Threading.Tasks;
using MordorFanficWeb.BusinessLogic.Services;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using AutoMapper;

namespace MordorFanficWeb.PresentationAdapters.AccountAdapter
{
    public class AccountAdapter : IAccountAdapter
    {
        private readonly IMapper mapper;
        private readonly AccountService accService;

        public AccountAdapter(IMapper mapper, AccountService accService)
        {
            this.mapper = mapper;
            this.accService = accService;
        }

        public async Task<bool> CreateUser(RegistrationViewModel user, string password)
        {
            return await accService.CreateUser(mapper.Map<AppUserModel>(user), password).ConfigureAwait(false);
        }

        public async Task DeleteUser(string id)
        {
            await accService.DeleteUser(id).ConfigureAwait(false);
        }

        public async Task<AppUserModel> GetUserById(string id)
        {
            return await accService.GetUserById(id).ConfigureAwait(false);
        }

        public async Task<List<UsersListViewModel>> GetUsersList()
        {
            return mapper.Map<List<UsersListViewModel>>(await accService.GetUsersList().ConfigureAwait(false));
        }

        public async Task UpdateUser(UpdateUserViewModel user)
        {
            await accService.UpdateUser(mapper.Map<AppUserModel>(user)).ConfigureAwait(false);
        }
    }
}
