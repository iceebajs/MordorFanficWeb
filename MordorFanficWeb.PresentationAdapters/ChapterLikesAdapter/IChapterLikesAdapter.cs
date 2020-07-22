using MordorFanficWeb.ViewModels.ChapterLikesViewModels;
using System.Threading.Tasks;

namespace MordorFanficWeb.PresentationAdapters.ChapterLikesAdapter
{
    public interface IChapterLikesAdapter
    {
        Task AddLike(ChapterLikeViewModel like);
    }
}
