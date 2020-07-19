using MordorFanficWeb.BusinessLogic.Base;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CompositionService : BaseService, ICompositionService
    {
        public CompositionService(IAppDbContext dbContext) : base(dbContext) { }
    }
}
