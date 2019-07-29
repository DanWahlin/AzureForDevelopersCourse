using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AzureForDevelopersCourse.Model
{
    public class FileUpload
    {
        public string Folder { get; set; }
        public IFormFile File { get; set; }
    }
}