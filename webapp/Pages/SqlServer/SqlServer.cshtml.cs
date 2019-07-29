using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository.BlobStorage;
using AzureForDevelopersCourse.Repository.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureForDevelopersCourse.Pages
{
    public class SqlServerModel : PageModel
    {
        const string routeName = "/SqlServer/SqlServer";

        [BindProperty]
        public Student Student { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();

        IStudentsRepository _studentsRepository;
        public SqlServerModel(IStudentsRepository studentsRepository) {
            _studentsRepository = studentsRepository;
        }
        public async Task OnGetAsync()
        {
            Students = await _studentsRepository.GetStudents();
        }

       public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            await _studentsRepository.InsertStudent(Student);
            return RedirectToPage(routeName);
        }

        public async Task<IActionResult> OnGetDeleteStudentAsync(int id)
        {
            await _studentsRepository.DeleteStudent(id);
            return RedirectToPage(routeName);
        }
    }
}
