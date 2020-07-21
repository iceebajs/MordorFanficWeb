using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.TagsViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.TagsAdapter
{
    public class TagsAdapter : ITagsAdapter
    {
        private readonly ITagsService tagsService;
        private readonly IMapper mapper;

        public TagsAdapter(ITagsService tagsService, IMapper mapper)
        {
            this.tagsService = tagsService;
            this.mapper = mapper;
        }

        public async Task CreateRangeOfTags(List<TagsViewModel> tags)
        {
            await tagsService.CreateAsync(mapper.Map<Tags>(tags)).ConfigureAwait(false);
        }

        public async Task<List<TagsViewModel>> GetAllTags()
        {
            return mapper.Map<List<TagsViewModel>>(await tagsService.GetAllAsync<Tags>().ConfigureAwait(false));
        }
    }
}
