using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        ScholarSystemDbContext _context;
        public StudentRepository(ScholarSystemDbContext context)
        {
            _context = context;
        }
        //crud operations
        public void Add(Student obj)
        {
            _context.Add(obj);
        }

        public void Delete(Student obj)
        {
            _context.Remove(obj);
        }

        public List<Student> GetAll()
        {
            return _context.Students.OrderByDescending(S=>S.Id).ToList();
        }


        public Student GetById(int id)
        {
            return _context.Students.FirstOrDefault(D => D.Id == id);
        }


        public void Update(Student obj)
        {
            _context.Update(obj);
        }
        //best practice
        public void save()
        {
            _context.SaveChanges();
        }

        //Extra Methods
        public List<Student> GetAllWithLoading()
        {
            List<Student> studentList = _context.Students
                .Where(S => !S.IsDeleted) //Exclude soft deleted students >> filter
                .Include(S => S.Department)
                .OrderByDescending(S => S.Id)
                .ToList();
            return studentList;
        }

        public Student GetByIdWithLoading(int id)
        {
            return _context.Students
                .Include(S => S.Department)
                .FirstOrDefault(S => S.Id == id);
        }
    }
}
