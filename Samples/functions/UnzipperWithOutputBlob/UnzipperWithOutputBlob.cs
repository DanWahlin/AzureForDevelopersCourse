using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

// Based on https://github.com/fboucher/azunzipeverything

namespace AzureForDevelopersCourse.Functions
{
    public static class UnzipperWithOutputBlob
    {
        [FunctionName("UnzipperWithOutputBlob")]
        public static async Task Run(
          [BlobTrigger("zipped-files/{name}", Connection = "unzipperFuncsStorage")] CloudBlockBlob blob, 
          string name, 
          [Blob("unzipped-files", FileAccess.Write)]CloudBlobContainer unzippedBlobContainer, 
          ILogger log)
        {
            log.LogInformation($"Blob trigger function Processed blob: {name}");

            string unzipperFuncsStorage = Environment.GetEnvironmentVariable("unzipperFuncsStorage");
            string destinationFilesContainer = Environment.GetEnvironmentVariable("destinationFilesContainer");

            try {
                if (name.EndsWith(".zip"))
                {
                    await ProcessZip(blob, unzippedBlobContainer, log);
                }
            }
            catch(Exception ex){
                log.LogInformation($"Error! Something went wrong: {ex.Message}");
            }            
        }

        private static async Task ProcessZip(CloudBlockBlob blob, CloudBlobContainer unzippedBlobContainer, ILogger log)
        {
            using (MemoryStream blobMemStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(blobMemStream);
                using (ZipArchive archive = new ZipArchive(blobMemStream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        log.LogInformation($"Now processing {entry.FullName}");

                        if (!entry.FullName.StartsWith("__MACOSX"))
                        {
                            //Replace all NO digits, letters, or "-" by a "-" Azure storage is specific on valid characters
                            string validName = Regex.Replace(entry.Name, @"[^a-zA-Z0-9\-\.]", "-").ToLower();

                            log.LogInformation($"File name being moved to destination blob container: {validName}");

                            if (!String.IsNullOrEmpty(validName))
                            {
                                CloudBlockBlob blockBlob = unzippedBlobContainer.GetBlockBlobReference(validName);
                                using (var fileStream = entry.Open())
                                {
                                    await blockBlob.UploadFromStreamAsync(fileStream);
                                }
                            }
                        }
                        else
                        {
                            log.LogInformation($"Skipping {entry.FullName}");
                        }
                    }
                }
            }
        }
    }
}