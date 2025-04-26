using ScholarSystem_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ScholarSystem_MVC.DbContexts
{
    public class ScholarSystemDbContext:DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=ScholarSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        //}

        public ScholarSystemDbContext():base()
        {
                
        }

        //Ask CLR by inject Dependecy
        public ScholarSystemDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One to Many>>Department to Student
            modelBuilder.Entity<Student>()
                .HasOne(S => S.Department)
                .WithMany(D => D.Students)
                .HasForeignKey(S => S.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many>>Department to Teacher
            modelBuilder.Entity<Teacher>()
                .HasOne(T => T.Department)
                .WithMany(D => D.Teachers)
                .HasForeignKey(T => T.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many>>Department to Course
            modelBuilder.Entity<Course>()
                .HasOne(C => C.Department)
                .WithMany(D => D.Courses)
                .HasForeignKey(C => C.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many>>Course to Teachers
            modelBuilder.Entity<Teacher>()
                .HasOne(T => T.Course)
                .WithMany(C => C.Teachers)
                .HasForeignKey(T => T.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StuCrsRes>()
                .HasOne(SCR => SCR.Student)
                .WithMany(S => S.StuCrsRes)
                .HasForeignKey(SCR => SCR.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StuCrsRes>()
                .HasOne(SCR => SCR.Course)
                .WithMany(C => C.StuCrsRes)
                .HasForeignKey(SCR => SCR.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StuCrsRes>()
                .HasKey(SCR => new { SCR.StudentId, SCR.CourseId });//Composite PK

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StuCrsRes> StuCrsRes { get; set; }

    }
}
