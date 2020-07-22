using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Persistence.AppDbContext;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class ChapterLikes : BaseService, IChapterLikesService
    {
        public ChapterLikes(IAppDbContext dbContext) : base(dbContext) { }
    }
}
