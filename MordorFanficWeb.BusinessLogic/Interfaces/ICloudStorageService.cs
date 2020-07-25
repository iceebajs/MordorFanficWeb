using Microsoft.AspNetCore.Http;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Interfaces
{
    public interface ICloudStorageService
    {
        Task<CloudImageViewModel> UploadAsync(IFormFile file);
        Task DeleteImage(string name);
        Task DeleteImagesRange(CloudImageViewModel[] names);
    }
}
