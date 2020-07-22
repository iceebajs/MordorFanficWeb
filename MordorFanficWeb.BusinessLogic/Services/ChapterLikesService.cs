using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Persistence.AppDbContext;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class ChapterLikesService : BaseService, IChapterLikesService
    {
        public ChapterLikesService(IAppDbContext dbContext) : base(dbContext) { }
    }
}
