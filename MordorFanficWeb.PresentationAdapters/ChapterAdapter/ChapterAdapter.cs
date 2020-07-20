using AutoMapper;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using MordorFanficWeb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MordorFanficWeb.PresentationAdapters.ChapterAdapter
{
    public class ChapterAdapter : IChapterAdapter
    {
        private readonly IChapterService chapterService;
        private readonly IMapper mapper;

        public ChapterAdapter(IChapterService chapterService, IMapper mapper)
        {
            this.chapterService = chapterService;
            this.mapper = mapper;
        }

        public async Task CreateChapter(ChapterViewModel chapter)
        {
            await chapterService.CreateAsync(mapper.Map<Chapter>(chapter)).ConfigureAwait(false);
        }

        public async Task DeleteChapter(int id)
        {
            var chapter = await chapterService.GetAsync<Chapter>(x => x.ChapterId == id).ConfigureAwait(false);
            await chapterService.DeleteAsync(chapter).ConfigureAwait(false);
        }

        public async Task<List<ChapterViewModel>> GetAllChapters()
        {
            return mapper.Map<List<ChapterViewModel>>(await chapterService.GetAllAsync<Chapter>().ConfigureAwait(false));
        }

        public async Task<ChapterViewModel> GetChapter(int id)
        {
            return mapper.Map<ChapterViewModel>(await chapterService.GetAsync<Chapter>(x => x.ChapterId == id).ConfigureAwait(false));
        }

        public async Task UpdateChapter(ChapterViewModel chapter)
        {
            var updatedChapter = mapper.Map<Chapter>(chapter);
            await chapterService.UpdateAsync(updatedChapter).ConfigureAwait(false);
        }
    }
}
