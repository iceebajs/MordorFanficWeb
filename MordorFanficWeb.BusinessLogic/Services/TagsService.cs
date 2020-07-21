using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.Persistence.AppDbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class TagsService : BaseService, ITagsService
    {
        public TagsService(IAppDbContext dbContext) : base(dbContext) { }

        public async Task CreateRangeOfTagsAsync(List<Tags> tags)
        {
            await dbContext.DbSet<Tags>().AddRangeAsync(tags).ConfigureAwait(false);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }
    }
}
