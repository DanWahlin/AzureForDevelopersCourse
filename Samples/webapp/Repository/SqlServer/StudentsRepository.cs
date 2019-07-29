using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AzureForDevelopersCourse.Repository.SqlServer
{

    public class StudentsRepository : IStudentsRepository
    {
        StudentsDbContext dbContext;
        public StudentsRepository(StudentsDbContext dbContext)
        {
            this.dbContext = dbContext;
            // Use for demo purposes to seed database
            // If using EF migrations then comment out this line
            this.dbContext.Database.EnsureCreated();
        }

        public async Task<List<Student>> GetStudents()
        {
            return await dbContext.Students.ToListAsync();
        }

        public async Task<bool> InsertStudent(Student student)
        {
            dbContext.Students.Add(student);
            try
            {
                var recordsAffected = await dbContext.SaveChangesAsync();
                return (recordsAffected > 0) ? true : false;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            try
            {
                var student = await dbContext.Students.FindAsync(id);
                dbContext.Remove(student);
                var recordsAffected = await dbContext.SaveChangesAsync();
            return (recordsAffected > 0) ? true : false;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return false;
        }

    }
}