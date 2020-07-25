using System.Threading.Tasks;
using System.Collections.Generic;
using MordorFanficWeb.ViewModels.ChapterViewModels;

namespace MordorFanficWeb.PresentationAdapters.ChapterAdapter
{
    public interface IChapterAdapter
    {
        Task<ChapterViewModel> GetChapter(int id);
        Task DeleteChapter(int id);
        Task UpdateChapter(ChapterViewModel composition);
        Task CreateChapter(ChapterViewModel composition);
        Task<List<ChapterViewModel>> GetAllChapters();
        Task UpdateChapterNumeration(ChapterNumerationViewModel[] chapterNumeration);
    }
}
