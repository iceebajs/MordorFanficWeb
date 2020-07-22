using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.ViewModels.ChapterLikesViewModels;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.ChapterLikesAdapter
{
    public class ChapterLikesAdapter : IChapterLikesAdapter
    {
        private readonly IMapper mapper;
        private readonly IChapterLikesService likesService;

        public ChapterLikesAdapter(IMapper mapper, IChapterLikesService likesService)
        {
            this.mapper = mapper;
            this.likesService = likesService;
        }

        public async Task AddLike(ChapterLikeViewModel like)
        {
            await likesService.CreateAsync(mapper.Map<ChapterLike>(like)).ConfigureAwait(false);
        }
    }
}
