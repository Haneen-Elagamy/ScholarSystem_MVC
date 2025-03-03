using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;
using ScholarSystem_MVC.ViewModels;

namespace ScholarSystem_MVC.Controllers
{
    public class DepartmentController : Controller
    {
        ScholarSystemDbContext _context=new ScholarSystemDbContext();
        DepartmentBL departmentBL=new DepartmentBL();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAll()
        {
            List<Department> DepartmentListModel =departmentBL.GetAll();
            return View("ShowAll",DepartmentListModel);
        }

        public IActionResult ShowDetails(int id)
        {
            Department department=departmentBL.GetById(id);
            return View("ShowDetails",department);
        }

        //Add
        //Show Add Form
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        //Handle Form Submission
        [HttpPost]
        public IActionResult SaveAdd(Department DeptFromReq)
        {
            if (ModelState.IsValid)
            {
                departmentBL.AddDept(DeptFromReq);

                // Debugging: Check if it's really saved
                var savedDept = departmentBL.GetById(DeptFromReq.Id);
                if (savedDept == null)
                {
                    return Content("Department was NOT saved! Check DB context.");
                }

                return RedirectToAction(nameof(ShowAll));
            }
            return View("Add", DeptFromReq);//return form with validation errors

        }

        public IActionResult ShowDepartment(int id)
        {
            var department=_context.Departments
                .Include(d=>d.Students) // Include students to avoid lazy loading issues
                .FirstOrDefault(d=>d.Id==id);

            if(department == null)
                return NotFound();

            var studentsAbove25 = department.Students?
                .Where(s => s.Age > 25)
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            var viewModel = new DepartmentViewModel
            {
                Name = department.Name,
                StudentsAbove25 = studentsAbove25,
                DepartmentState = department.Students.Count > 50 ? "Main" : "Branch"
            };

            return View("ShowDepartment",viewModel);
        }



    }
}
