using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureForDevelopersCourse.Repository.SqlServer {
    public interface IStudentsRepository
    {
        Task<bool> DeleteStudent(int id);
        Task<List<Student>> GetStudents();
        Task<bool> InsertStudent(Student student);
    }
}