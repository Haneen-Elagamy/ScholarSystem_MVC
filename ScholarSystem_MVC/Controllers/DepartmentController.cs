using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;
using ScholarSystem_MVC.Repositories;
using ScholarSystem_MVC.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace ScholarSystem_MVC.Controllers
{
    public class DepartmentController : Controller
    {
        //ScholarSystemDbContext _context=new ScholarSystemDbContext();
        private readonly ScholarSystemDbContext _context;
        //DepartmentBL departmentBL=new DepartmentBL();
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository,ScholarSystemDbContext context)
        {
            _departmentRepository=departmentRepository;
            _context=context;
            
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAll()
        {
            List<Department> DepartmentListModel = _departmentRepository.GetAllWithLoading();
            return View("ShowAll",DepartmentListModel);
        }

        public IActionResult ShowDetails(int id)
        {
            Department department = _departmentRepository.GetById(id);
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
            if (ModelState.IsValid==true)
            {
                _departmentRepository.Add(DeptFromReq);
                _departmentRepository.save();
                TempData["NotificationAdded"] = "Department was Added Successfully";
                return RedirectToAction(nameof(ShowAll));
            }
            //if (DeptFromReq.Name != null)
            //{
            //    departmentBL.AddDept(DeptFromReq);
            //    return RedirectToAction(nameof(ShowAll));
            //}

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

        //Show Edit form
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Fetch Department from Database
            Department DeptFromReq=_departmentRepository.GetById(id);
            //if(DeptFromReq == null)
            //{
            //    return NotFound();//Handle invalid Id
            //}
            return View(nameof(Edit),DeptFromReq);
        }

        [HttpPost]
        public IActionResult SaveEdit(Department DeptFromReq)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), DeptFromReq);
            }

            // Fetch department from DB
            Department DeptFromDB = _departmentRepository.GetById(DeptFromReq.Id);
            if (DeptFromDB == null)
            {
                return NotFound();
            }

            // Update values
            DeptFromDB.Name = DeptFromReq.Name;
            DeptFromDB.MgrName = DeptFromReq.MgrName;

            // Save changes
            _context.Departments.Update(DeptFromDB);
            _context.SaveChanges();
            TempData["NotificationAdded"] = "Department was Edited Successfully!";
            return RedirectToAction(nameof(ShowAll));
        }

        //[HttpPost]
        //public IActionResult SaveEdit(Department DeptFromReq)
        //{
        //    if (DeptFromReq.Name!=null)
        //    {
        //        // Fetch department from DB
        //        Department DeptFromDB = _departmentRepository.GetById(DeptFromReq.Id);
        //        // Update values
        //        DeptFromDB.Name = DeptFromReq.Name;
        //        DeptFromDB.MgrName = DeptFromReq.MgrName;

        //        // Save changes
        //        _context.Departments.Update(DeptFromDB);
        //        _context.SaveChanges();

        //        return RedirectToAction(nameof(ShowAll));
        //    }

        //    return View(nameof(Edit), DeptFromReq);

            
        //}


    }
}
