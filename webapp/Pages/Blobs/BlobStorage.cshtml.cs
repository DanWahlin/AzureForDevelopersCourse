using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository.BlobStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureForDevelopersCourse.Pages
{
    public class BlobStorageModel : PageModel
    {
        const string containerName = "blobs";
        const string routeName = "/Blobs/BlobStorage";

        [BindProperty]
        public FileUpload FileUpload { get; set; }
        public List<AzureBlobItem> Blobs { get; set; } = new List<AzureBlobItem>();

        IAzureBlobStorage BlobStorage;
        public BlobStorageModel(IAzureBlobStorage blobStorage) {
            BlobStorage = blobStorage;
        }
        public async Task OnGetAsync()
        {
            Blobs = await BlobStorage.GetBlobsAsync(containerName);
            Console.WriteLine(Blobs);
        }

       public async Task<IActionResult> OnPostAsync()
        {
            if (FileUpload == null) ModelState.AddModelError("File", "FileUpload is null");
            if (FileUpload.File == null || FileUpload.File.Length == 0) ModelState.AddModelError("File", "No file selected to upload");

            if (!ModelState.IsValid) {
                return Page();
            }

            var blobName = FileUpload.File.FileName;
            blobName = (FileUpload.Folder != null) ? string.Format(@"{0}\{1}", FileUpload.Folder, blobName) : blobName;
            var ms = new MemoryStream();
            ms.Position = 0;
            await FileUpload.File.CopyToAsync(ms);
            await BlobStorage.UploadAsync(containerName, blobName, ms);

            return RedirectToPage(routeName);
        }

        public async Task<IActionResult> OnGetDeleteBlobAsync(string blobName)
        {
            await BlobStorage.DeleteAsync(containerName, blobName);
            return RedirectToPage(routeName);
        }
    }
}
