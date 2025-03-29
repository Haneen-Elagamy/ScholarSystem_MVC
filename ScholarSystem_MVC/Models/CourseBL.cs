using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
namespace ScholarSystem_MVC.Models
{
    public class CourseBL
    {
        ScholarSystemDbContext Context=new ScholarSystemDbContext();


        public List<Course> GetAll()
        {
            List<Course> courses=Context.Courses
                .Where(C=>!C.IsDeleted) 
                .Include(C=>C.Department)
                .ToList();
            return courses;
        }

        public Course GetById(int id)
        {
            return Context.Courses
                .Include(C => C.Department)
                .FirstOrDefault(C => C.Id == id);
        }

        public List<Department> GetDepartments()
        {
            return Context.Departments.ToList();
        }

        //Add new Course

        public void AddCourse(Course CourseSent)
        {
            Context.Courses.Add(CourseSent);
            Context.SaveChanges();
        }
        public void Update(Course course)
        {
            Context.Courses.Update(course);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                course.IsDeleted=true;
                Context.SaveChanges();
            }
        }




    }
}
