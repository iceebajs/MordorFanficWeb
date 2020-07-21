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
            var account = await GetAsync<AccountUser>(x => x.AccountUserId == id).ConfigureAwait(false);
            List<Composition> compositions = await Task.Run(() => account.Compositions.ToList());
            return compositions;
        }
    }
}
