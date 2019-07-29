using System;
using System.ComponentModel.DataAnnotations;

namespace AzureForDevelopersCourse.Repository.SqlServer {
    public class Student {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string School { get; set; }
    }
}