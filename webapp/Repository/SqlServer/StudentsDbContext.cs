using System;
using Microsoft.EntityFrameworkCore;

namespace AzureForDevelopersCourse.Repository.SqlServer {
    public class StudentsDbContext : DbContext
    {
        public StudentsDbContext() { }

        public StudentsDbContext(DbContextOptions<StudentsDbContext> options)
            : base(options)  { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student { Id=1, FirstName="Jane", LastName="Doe", School="Blue Elementary School" });
            modelBuilder.Entity<Student>().HasData(new Student { Id=2, FirstName="John", LastName="Doe", School="Blue Elementary School" });
            modelBuilder.Entity<Student>().HasData(new Student { Id=3, FirstName="Tina", LastName="Smith", School="Yellow Elementary School" });
            modelBuilder.Entity<Student>().HasData(new Student { Id=4, FirstName="Iris", LastName="Johanson", School="Green Elementary School" });
        }

    }
}