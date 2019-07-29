using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos.Table;

namespace AzureForDevelopersCourse.Model
{
    public class Customer : TableEntity
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string City { get; set; }
    }
}