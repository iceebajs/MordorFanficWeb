using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Persistence.AppDbContext;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CompositionRatingsService : BaseService, ICompositionRatingsService
    {
        public CompositionRatingsService(IAppDbContext dbContext) : base(dbContext) { }
    }
}
