using System.Threading.Tasks;

namespace MordorFanficWeb.Persistence.AppDbContext
{
    public interface IAppDbContext
    {
        Task SaveAsync();
        void Dispose();
    }
}
