using AutoMapper;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using MordorFanficWeb.ViewModels.CompositionViewModels;

namespace MordorFanficWeb.Common.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationViewModel, AppUserModel>();
            CreateMap<UsersListViewModel, AppUserModel>().ReverseMap();
            CreateMap<AppUserViewModel, AppUserModel>().ReverseMap();
            CreateMap<CompositionViewModel, CompositionModel>();
            CreateMap<CompositionViewModel, CompositionModel>().ReverseMap();
        }
    }
}
