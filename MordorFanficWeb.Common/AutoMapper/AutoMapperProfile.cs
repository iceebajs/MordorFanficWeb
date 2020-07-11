using AutoMapper;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;

namespace MordorFanficWeb.Common.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationViewModel, AppUserModel>();
            CreateMap<UsersListViewModel, AppUserModel>().ReverseMap();
            CreateMap<UpdateUserViewModel, AppUserModel>();
        }
    }
}
