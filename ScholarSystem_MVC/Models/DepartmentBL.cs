using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;

namespace ScholarSystem_MVC.Models
{
    public class DepartmentBL
    {
        ScholarSystemDbContext Context=new ScholarSystemDbContext();
        //Operations on Departments
        //GetAll
        public List<Department> GetAll()
        {
            List<Department> departmentList=Context.Departments
                .Include(D=> D.Students)
                .Include(D=>D.Teachers)
                .Include(D=>D.Courses)
                .ToList();
            return departmentList;
        }

        //GetById
        public Department GetById(int id)
        {
            return Context.Departments?.FirstOrDefault(D => D.Id == id);
        }


        //Add
        public void AddDept (Department DeptSent)
        {
            //Save to Local
            Context.Departments.Add(DeptSent);
            //Save to DB
            Context.SaveChanges();
        }

    }
}
