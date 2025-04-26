using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        ScholarSystemDbContext _context;
        public DepartmentRepository(ScholarSystemDbContext context)
        {
            _context = context;    
        }
        //crud operations
        public void Add(Department obj)
        {
            _context.Add(obj);
        }

        public void Delete(Department obj)
        {
            _context.Remove(obj);
        }

        public List<Department> GetAll()
        {
            return _context.Departments.OrderByDescending(d=>d.Id).ToList();
        }


        public Department GetById(int id)
        {
            return _context.Departments.FirstOrDefault(D => D.Id == id);
        }


        public void Update(Department obj)
        {
            _context.Update(obj);
        }
        //best practice
        public void save()
        {
            _context.SaveChanges();
        }
        //Extra Methods
        public List<Department> GetAllWithLoading()
        {
            List<Department> departmentList = _context.Departments
                .Include(D => D.Students)
                .Include(D => D.Teachers)
                .Include(D => D.Courses)
                .OrderByDescending(d=>d.Id)
                .ToList();
            return departmentList;
        }
    }
}
