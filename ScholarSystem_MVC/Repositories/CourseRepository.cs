using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public class CourseRepository:ICourseRepository
    {
        ScholarSystemDbContext _context;
        public CourseRepository(ScholarSystemDbContext context)
        {
            _context = context;
        }
        //crud operations
        public void Add(Course obj)
        {
            _context.Add(obj);
        }

        public void Delete(Course obj)
        {
           _context.Remove(obj);
        }

        public List<Course> GetAll()
        {
            return _context.Courses.OrderByDescending(c => c.Id).ToList();
        }


        public Course GetById(int id)
        {
            return _context.Courses.FirstOrDefault(D => D.Id == id);
        }


        public void Update(Course obj)
        {
            _context.Update(obj);
        }
        //best practice
        public void save()
        {
            _context.SaveChanges();
        }

        //Extra Methods
        public List<Course> GetAllWithLoading()
        {
            List<Course> courses = _context.Courses
                .Where(C => !C.IsDeleted)
                .Include(C => C.Department)
                .OrderByDescending(C =>C.Id)
                .ToList();
            return courses;
        }

        public Course GetByIdWithLoading(int id)
        {
            return _context.Courses
                .Include(C => C.Department)
                .FirstOrDefault(C => C.Id == id);
        }

        public void DeleteById(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                course.IsDeleted = true;
                _context.SaveChanges();
            }
        }




    }
}
