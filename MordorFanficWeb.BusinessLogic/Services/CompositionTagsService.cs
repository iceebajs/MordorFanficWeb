using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.Persistence.AppDbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CompositionTagsService : BaseService, ICompositionTagsService
    {
        public CompositionTagsService(IAppDbContext appDbContext) : base(appDbContext) { }

        public async Task CreateRangeOfTagsAsync(List<CompositionTags> tags)
        {
            await dbContext.DbSet<CompositionTags>().AddRangeAsync(tags).ConfigureAwait(false);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }
    }
}
