using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.Persistence.AppDbContext;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class ChapterService : BaseService, IChapterService
    {
        public ChapterService(IAppDbContext appDbContext) : base(appDbContext) { }
    }
}
