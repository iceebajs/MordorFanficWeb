﻿using AutoMapper;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using MordorFanficWeb.ViewModels.CompositionTagsViewModel;
using MordorFanficWeb.ViewModels.CompositionViewModels;
using MordorFanficWeb.ViewModels.TagsViewModels;

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

            CreateMap<ChapterViewModel, Chapter>();
            CreateMap<ChapterViewModel, Chapter>().ReverseMap();

            CreateMap<TagsViewModel, Tags>();
            CreateMap<TagsViewModel, Tags>().ReverseMap();

            CreateMap<CompositionTagsViewModel, CompositionTags>();
            CreateMap<CompositionTagsViewModel, CompositionTags>().ReverseMap();
        }
    }
}
