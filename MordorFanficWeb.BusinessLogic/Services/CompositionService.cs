using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using MordorFanficWeb.Persistence.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CompositionService : BaseService, ICompositionService
    {
        public CompositionService(IAppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Composition>> GetAllCompositionsOfAccount(int id)
        {
            return await dbContext.DbSet<Composition>().Where(x => x.UserId == id).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Composition> GetCompositionById(int id)
        {
            return await dbContext.DbSet<Composition>()
                .Include(c => c.Chapters)
                .ThenInclude(l => l.ChapterLikes)
                .Include(t => t.CompositionTags)
                .Include(c => c.CompositionComments)
                .Include(r => r.CompositionRatings)
                .FirstOrDefaultAsync(x => x.CompositionId == id).ConfigureAwait(false);
        }
    }
}
