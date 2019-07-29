using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureForDevelopersCourse.Repository.BlobStorage
{
    public interface IAzureBlobStorage
    {
        Task UploadAsync(string containerName, string blobName, string filePath);
        Task UploadAsync(string containerName, string blobName, Stream stream);
        Task<MemoryStream> DownloadAsync(string containerName, string blobName);
        Task DownloadAsync(string containerName, string blobName, string path);
        Task DeleteAsync(string containerName, string blobName);
        Task<bool> ExistsAsync(string containerName, string blobName);
        Task<List<AzureBlobItem>> GetBlobsAsync(string containerName);
        Task<List<AzureBlobItem>> GetBlobsAsync(string containerName, string rootFolder);
        Task<List<string>> GetFoldersAsync(string containerName);
        Task<List<string>> GetFoldersAsync(string containerName, string rootFolder);
    }
}