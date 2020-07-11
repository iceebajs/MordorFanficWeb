using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models;
using System.Threading.Tasks;

namespace MordorFanficWeb.Persistence.AppDbContext
{
    public class AppDbContext : IdentityDbContext<AppUserModel>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
