using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;

namespace ScholarSystem_MVC.Models
{
    public class StudentBL
    {
        ScholarSystemDbContext Context = new ScholarSystemDbContext();
        //operations on Students
        //GetAll
        public List<Student> GetAll()
        {
            List<Student> studentList = Context.Students
                .Include(S=>S.Department)
                .ToList();
            return studentList;
        }

        public Student GetById(int id)
        {
            return Context.Students
                .Include(S=>S.Department)
                .FirstOrDefault(S => S.Id == id);
        }
    }
}
