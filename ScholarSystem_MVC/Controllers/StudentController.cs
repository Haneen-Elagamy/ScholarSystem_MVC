using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;
using ScholarSystem_MVC.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

namespace ScholarSystem_MVC.Controllers
{
    public class StudentController : Controller
    {
        ScholarSystemDbContext _context = new ScholarSystemDbContext();
        StudentBL studentBL = new StudentBL();
        DepartmentBL departmentBL = new DepartmentBL();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAll(int? departmentId, string search="",int page=1,int pageSize=5)
        {

            // Get all students
            var students = studentBL.GetAll();

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Apply department filter if departmentId is provided
            if (departmentId.HasValue && departmentId > 0)
            {
                students = students.Where(s => s.DepartmentId == departmentId.Value).ToList();
            }

            // Apply pagination
            IPagedList<Student> StudentListModel = students.ToPagedList(page, pageSize);

            // Pass department list to ViewBag for the dropdown
            ViewBag.Departments = new SelectList(departmentBL.GetAll(), "Id", "Name");

            return View("ShowAll", StudentListModel);
        }

        public IActionResult ShowDetails(int id)
        {
            Student student=studentBL.GetById(id);
            return View("ShowDetails",student);
        }

        //Add life cycle
        [HttpGet]
        public IActionResult Add()
        {
            //catch DepartmentList
            List<Department> departmentList = departmentBL.GetAll();
            //declare viewModel
            StuWithDeptListViewModel SDVM = new StuWithDeptListViewModel
            {
                DeptList= departmentList //pass the list of departments
            };
            return View(nameof(Add),SDVM);
        }
        [HttpPost]
        public IActionResult SaveAdd(StuWithDeptListViewModel StuFromReq)
        {
            
            if (StuFromReq.Name != null)
            {
                Student NewStudent = new Student
                {
                    Name = StuFromReq.Name,
                    Age = StuFromReq.Age,
                    IsDeleted= StuFromReq.IsDeleted,
                    DepartmentId = StuFromReq.DepartmentId
                };
                studentBL.AddStudent(NewStudent);
                return RedirectToAction(nameof(ShowAll));
            }
            //if validation fails 
            StuFromReq.DeptList = departmentBL.GetAll();
            return View(nameof(Add), StuFromReq);
        }

        //Edit LifeCycle
        public IActionResult Edit(int id)
        {
            //Catch diffrent resources
            Student StuFromDB = studentBL.GetById(id);
            List<Department> DepartmentList= departmentBL.GetAll();
            //declare ViewModel
            StuWithDeptListViewModel SDVM = new StuWithDeptListViewModel
            {
               Id=StuFromDB.Id,
               Name=StuFromDB.Name,
               Age=StuFromDB.Age,
               IsDeleted= StuFromDB.IsDeleted,
               DepartmentId=StuFromDB.DepartmentId,
               DeptList = DepartmentList
            };
            return View(nameof(Edit), SDVM);

        }
        public IActionResult SaveEdit(StuWithDeptListViewModel StuFromReq,int id)
        {
            //Defensive
            if(StuFromReq.Name != null)
            {
                //old object
                Student StuFromDB= studentBL.GetById(id);

                if (StuFromDB == null)
                {
                    return NotFound();  // Return 404 if student not found
                }
                //Edit in memory
                StuFromDB.Name = StuFromReq.Name;
                StuFromDB.Age= StuFromReq.Age;
                StuFromDB.IsDeleted= StuFromReq.IsDeleted;
                StuFromDB.DepartmentId= StuFromReq.DepartmentId;

                // Mark entity as modified (if using EF Core)
                _context.Students.Update(StuFromDB);
                //Edit in DB
                _context.SaveChanges();
                //if saved
                return RedirectToAction(nameof(ShowAll));
            }
            StuFromReq.DeptList= departmentBL.GetAll();
            return View(nameof(Edit),StuFromReq);

        }
        public IActionResult Delete(int id)
        {
            Student StuFromDB= studentBL.GetById(id);
            if(StuFromDB == null) return NotFound();
            return View("DeleteWarning",StuFromDB);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Student StuFromDB = studentBL.GetById(id);
            if (StuFromDB == null) return NotFound();

            // If using soft delete
            StuFromDB.IsDeleted = true;
            _context.Students.Update(StuFromDB);

            _context.SaveChanges();

            return RedirectToAction(nameof(ShowAll));
        }
    }
}
