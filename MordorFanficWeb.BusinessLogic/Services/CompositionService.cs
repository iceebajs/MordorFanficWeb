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

        public async Task<int> CreateComposition(Composition composition)
        {
            await CreateAsync(composition).ConfigureAwait(false);
            return composition.CompositionId;
        }

        public async Task<List<Composition>> GetLastAdded()
        {
            return await dbContext.DbSet<Composition>()
                .OrderByDescending(i => i.CompositionId)
                .Include(c => c.Chapters)
                .Include(t => t.CompositionTags)
                .Include(c => c.CompositionComments)
                .Include(r => r.CompositionRatings)
                .Take(6)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Composition>> FindInCompositions(string keyword)
        {
            var result = await dbContext.DbSet<Composition>().
                Where(kw => kw.Title.Contains($"{keyword}"))
                .Include(r => r.CompositionRatings)
                .ToListAsync().ConfigureAwait(false);

            if(result.Count == 0)
                result = await dbContext.DbSet<Composition>().
                Where(kw => kw.PreviewContext.Contains($"{keyword}"))
                .Include(r => r.CompositionRatings)
                .ToListAsync().ConfigureAwait(false);

            if (result.Count == 0)
                result = await dbContext.DbSet<Composition>()
                    .Include(c => c.Chapters)
                    .Where(c => c.Chapters.Any() && c.Chapters.Where(kw => kw.ChapterTitle.Contains($"{keyword}")).Count() == c.Chapters.Count)
                    .ToListAsync().ConfigureAwait(false);

            if (result.Count == 0)
                result = await dbContext.DbSet<Composition>()
                    .Include(c => c.Chapters)
                    .Where(c => c.Chapters.Any() && c.Chapters.Where(kw => kw.Context.Contains($"{keyword}")).Count() == c.Chapters.Count)
                    .ToListAsync().ConfigureAwait(false);

            if (result.Count == 0)
                result = await dbContext.DbSet<Composition>()
                    .Include(c => c.CompositionComments)
                    .Where(c => c.CompositionComments.Any() && c.CompositionComments.Where(kw => kw.CommentContext.Contains($"{keyword}")).Count() == c.CompositionComments.Count)
                    .ToListAsync().ConfigureAwait(false);

            return result;
        }
    }
}
