using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using MordorFanficWeb.BusinessLogic.Helpers;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly IStorageConnectionFactory storageConnectionFactory;
        public CloudStorageService(IStorageConnectionFactory storageConnectionFactory)
        {
            this.storageConnectionFactory = storageConnectionFactory;
        }

        public async Task DeleteImage(string name)
        {
            Uri uri = new Uri(name);
            string filename = Path.GetFileName(uri.LocalPath);
            var blobContainer = await storageConnectionFactory.GetContainer().ConfigureAwait(false);
            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task DeleteImagesRange(CloudImageViewModel[] names)
        {
            foreach(var name in names)
            {
                Uri uri = new Uri(name.Url);
                string filename = Path.GetFileName(uri.LocalPath);
                var blobContainer = await storageConnectionFactory.GetContainer().ConfigureAwait(false);
                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            }
        }

        public async Task<CloudImageViewModel> UploadAsync(IFormFile file)  
        {
            var blobContainer = await storageConnectionFactory.GetContainer().ConfigureAwait(false);

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
            blob.Properties.ContentType = file.ContentType;
            using (var stream = file.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);
            }
            return new CloudImageViewModel { Url = blob.Uri.AbsoluteUri };
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }
}
