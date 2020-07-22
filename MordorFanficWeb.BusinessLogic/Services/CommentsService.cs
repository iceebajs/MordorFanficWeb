using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.Persistence.AppDbContext;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CommentsService : BaseService, ICommentsService
    {
        public CommentsService(IAppDbContext dbContext) : base(dbContext) { }
    }
}
