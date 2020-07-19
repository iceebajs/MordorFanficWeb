using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models.BaseModels;
using MordorFanficWeb.Persistence.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Base
{
    public class BaseService : IBaseService, IDisposable
    {
        protected IAppDbContext dbContext;

        public BaseService(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<T>> GetAllAsync<T>() where T : BaseEntity
        {
            return await dbContext.DbSet<T>().ToListAsync().ConfigureAwait(false); 
        }

        public async Task CreateAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Add(entity);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Remove(entity);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return await dbContext.DbSet<T>().FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            dbContext.DbSet<T>().Update(entity);
            await dbContext.SaveAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
