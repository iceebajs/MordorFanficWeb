using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models.BaseModels;

namespace MordorFanficWeb.Persistence.AppDbContext
{
    public interface IAppDbContext
    {
        DbSet<T> DbSet<T>() where T: BaseEntity;
        Task SaveAsync();
        void Dispose();
    }
}
