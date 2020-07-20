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
            CreateMap<RegistrationViewModel, AppUser>();
            CreateMap<UsersListViewModel, AppUser>().ReverseMap();
            CreateMap<AppUserViewModel, AppUser>().ReverseMap();
            CreateMap<CompositionViewModel, Composition>();
            CreateMap<CompositionViewModel, Composition>().ReverseMap();
        }
    }
}
