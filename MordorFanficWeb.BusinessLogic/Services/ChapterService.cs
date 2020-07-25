using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.Persistence.AppDbContext;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using System.Threading.Tasks;
using MordorFanficWeb.Models;
using System.Linq;
using System;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class ChapterService : BaseService, IChapterService
    {
        public ChapterService(IAppDbContext appDbContext) : base(appDbContext) { }

        public async Task UpdateChapterNumeration(ChapterNumerationViewModel[] chapterNumeration)
        {
            var chapters = dbContext.DbSet<Chapter>().Where(x => x.CompositionId == chapterNumeration[0].CompositionId);
            foreach(var chapter in chapters)
            {
                ChapterNumerationViewModel currentNumeration = Array.Find(chapterNumeration, i => i.ChapterId == chapter.ChapterId);
                chapter.ChapterNumber = currentNumeration.CurrentNumber;
            }
            dbContext.DbSet<Chapter>().UpdateRange(chapters);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }
    }
}
