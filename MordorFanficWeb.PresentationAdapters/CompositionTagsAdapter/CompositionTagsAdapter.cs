using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.CompositionTagsViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.CompositionTagsAdapter
{
    public class CompositionTagsAdapter : ICompositionTagsAdapter
    {
        private readonly ICompositionTagsService tagsService;
        private readonly IMapper mapper;

        public CompositionTagsAdapter(ICompositionTagsService tagsService, IMapper mapper)
        {
            this.tagsService = tagsService;
            this.mapper = mapper;
        }

        public async Task CreateRangeOfTags(List<CompositionTagsViewModel> tags)
        {
            await tagsService.CreateRangeOfTagsAsync(mapper.Map<List<CompositionTags>>(tags)).ConfigureAwait(false);
        }
    }
}
