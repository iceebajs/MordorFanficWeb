using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Helpers
{
    public interface IStorageConnectionFactory
    {
        Task<CloudBlobContainer> GetContainer();
    }
}
