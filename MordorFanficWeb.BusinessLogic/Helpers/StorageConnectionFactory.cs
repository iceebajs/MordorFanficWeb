using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MordorFanficWeb.BusinessLogic.Helpers
{
    public class StorageConnectionFactory : IStorageConnectionFactory
    {
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private readonly CloudStorageOptions storageOptions;

        public StorageConnectionFactory(CloudStorageOptions storageOptions)
        {
            this.storageOptions = storageOptions;
        }

        public async Task<CloudBlobContainer> GetContainer()
        {
            if (blobContainer != null)
                return blobContainer;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageOptions.ConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(storageOptions.ImagesContainerName);

            await blobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).ConfigureAwait(false);

            return blobContainer;
        }
    }
}
