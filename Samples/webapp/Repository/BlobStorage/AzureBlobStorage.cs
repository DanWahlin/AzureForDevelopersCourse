using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace AzureForDevelopersCourse.Repository.BlobStorage
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly AzureSettings Settings;
        public AzureBlobStorage(AzureSettings settings)
        {
            Settings = settings;
        }
        
        public async Task UploadAsync(string containerName, string blobName, string filePath)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);

            //Upload
            using (var fileStream = System.IO.File.Open(filePath, FileMode.Open))
            {
                fileStream.Position = 0;
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

        public async Task UploadAsync(string containerName, string blobName, Stream stream)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);

            //Upload
            stream.Position = 0;
            await blockBlob.UploadFromStreamAsync(stream);
        }

        public async Task<MemoryStream> DownloadAsync(string containerName, string blobName)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);

            //Download
            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return stream;
            }
        }

        public async Task DownloadAsync(string containerName, string blobName, string path)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);
            
            //Download
            await blockBlob.DownloadToFileAsync(path, FileMode.Create);
        }

        public async Task DeleteAsync(string containerName, string blobName)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);

            //Delete
            await blockBlob.DeleteAsync();
        }

        public async Task<bool> ExistsAsync(string containerName, string blobName)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(containerName, blobName);

            //Exists
            return await blockBlob.ExistsAsync();
        }

        public async Task<List<AzureBlobItem>> GetBlobsAsync(string containerName)
        {
            return await GetBlobListAsync(containerName);
        }

        public async Task<List<AzureBlobItem>> GetBlobsAsync(string containerName, string rootFolder)
        {
            if (rootFolder == "*") return await GetBlobsAsync(containerName); //All Blobs
            if (rootFolder == "/") rootFolder = "";          //Root Blobs

            var list = await GetBlobListAsync(containerName);
            return list.Where(i => i.Folder == rootFolder).ToList();
        }

        public async Task<List<string>> GetFoldersAsync(string containerName)
        {
            var list = await GetBlobListAsync(containerName);
            return list.Where(i => !string.IsNullOrEmpty(i.Folder))
                       .Select(i => i.Folder)
                       .Distinct()
                       .OrderBy(i => i)
                       .ToList();
        }

        public async Task<List<string>> GetFoldersAsync(string containerName, string rootFolder)
        {
            if (rootFolder == "*" || rootFolder == "") return await GetFoldersAsync(containerName); // All Folders

            var list = await GetBlobListAsync(containerName);
            return list.Where(i => i.Folder.StartsWith(rootFolder))
                       .Select(i => i.Folder)
                       .Distinct()
                       .OrderBy(i => i)
                       .ToList();
        }

        private async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            //Account
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(Settings.StorageAccount, Settings.StorageKey), true);

            //Client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            //await blobContainer.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });

            return blobContainer;
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string containerName, string blobName)
        {
            //Container
            CloudBlobContainer blobContainer = await GetContainerAsync(containerName);

            //Blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            return blockBlob;
        }

        private async Task<List<AzureBlobItem>> GetBlobListAsync(string containerName, bool useFlatListing = true)
        {
            //Container
            CloudBlobContainer blobContainer = await GetContainerAsync(containerName);

            //List
            var list = new List<AzureBlobItem>();
            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment resultSegment =
                    await blobContainer.ListBlobsSegmentedAsync("", useFlatListing, new BlobListingDetails(), null, token, null, null);
                token = resultSegment.ContinuationToken;

                foreach (IListBlobItem item in resultSegment.Results)
                {
                    list.Add(new AzureBlobItem(item));
                }
            } while (token != null);

            return list.OrderBy(i => i.Folder).ThenBy(i => i.Name).ToList();
        }
    }
}