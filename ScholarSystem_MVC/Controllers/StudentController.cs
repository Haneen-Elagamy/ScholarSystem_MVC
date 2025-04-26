using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;
using ScholarSystem_MVC.Repositories;
using ScholarSystem_MVC.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

namespace ScholarSystem_MVC.Controllers
{
    public class StudentController : Controller
    {
        //ScholarSystemDbContext _context = new ScholarSystemDbContext();
        //StudentBL studentBL = new StudentBL();
        //DepartmentBL departmentBL = new DepartmentBL();
        private readonly ScholarSystemDbContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public StudentController(ScholarSystemDbContext context, IStudentRepository studentRepository,IDepartmentRepository departmentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
                
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Show All
        public IActionResult ShowAll(int? departmentId, string search = "", int page = 1, int pageSize = 5)
        {

            // Get all students
            var students = _studentRepository.GetAllWithLoading();

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
            ViewBag.Departments = new SelectList(_departmentRepository.GetAllWithLoading(), "Id", "Name");

            return View("ShowAll", StudentListModel);
        }
        #endregion

        #region Show Details
        public IActionResult ShowDetails(int id)
        {
            Student student = _studentRepository.GetByIdWithLoading(id);
            return View("ShowDetails", student);
        } 
        #endregion 

        #region Add LifeCycle
        //Add life cycle
        [HttpGet]
        public IActionResult Add()
        {
            //catch DepartmentList
            List<Department> departmentList = _departmentRepository.GetAllWithLoading();
            //declare viewModel
            StuWithDeptListViewModel SDVM = new StuWithDeptListViewModel
            {
                DeptList = departmentList //pass the list of departments
            };
            return View(nameof(Add), SDVM);
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
                    IsDeleted = StuFromReq.IsDeleted,
                    DepartmentId = StuFromReq.DepartmentId
                };
                _studentRepository.Add(NewStudent);
                _studentRepository.save();
                TempData["NotificationAdded"] = $"Student with name {StuFromReq.Name},and with Id {StuFromReq.Id} was added successfully!";
                return RedirectToAction(nameof(ShowAll));
            }
            //if validation fails 
            StuFromReq.DeptList = _departmentRepository.GetAllWithLoading();
            return View(nameof(Add), StuFromReq);
        } 
        #endregion 

        #region Edit LifeCycle
        //Edit LifeCycle
        public IActionResult Edit(int id)
        {
            //Catch diffrent resources
            Student StuFromDB = _studentRepository.GetById(id);
            List<Department> DepartmentList = _departmentRepository.GetAllWithLoading();
            //declare ViewModel
            StuWithDeptListViewModel SDVM = new StuWithDeptListViewModel
            {
                Id = StuFromDB.Id,
                Name = StuFromDB.Name,
                Age = StuFromDB.Age,
                IsDeleted = StuFromDB.IsDeleted,
                DepartmentId = StuFromDB.DepartmentId,
                DeptList = DepartmentList
            };
            return View(nameof(Edit), SDVM);

        }
        public IActionResult SaveEdit(StuWithDeptListViewModel StuFromReq, int id)
        {
            //Defensive
            if (StuFromReq.Name != null)
            {
                //old object
                Student StuFromDB = _studentRepository.GetById(id);

                if (StuFromDB == null)
                {
                    return NotFound();  // Return 404 if student not found
                }
                //Edit in memory
                StuFromDB.Name = StuFromReq.Name;
                StuFromDB.Age = StuFromReq.Age;
                StuFromDB.IsDeleted = StuFromReq.IsDeleted;
                StuFromDB.DepartmentId = StuFromReq.DepartmentId;

                // Mark entity as modified (if using EF Core)
                _context.Students.Update(StuFromDB);
                //Edit in DB
                _context.SaveChanges();
                //if saved
                TempData["NotificationAdded"]= $"Student with name {StuFromReq.Name} ,and with Id {StuFromReq.Id} was Edited successfully!";
                return RedirectToAction(nameof(ShowAll));
            }
            StuFromReq.DeptList = _departmentRepository.GetAllWithLoading();
            return View(nameof(Edit), StuFromReq);

        }
        #endregion

        #region Delete LifeCycle
        public IActionResult Delete(int id)
        {
            Student StuFromDB = _studentRepository.GetById(id);
            if (StuFromDB == null) return NotFound();
            return View("DeleteWarning", StuFromDB);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Student StuFromDB = _studentRepository.GetById(id);
            if (StuFromDB == null) return NotFound();

            // If using soft delete
            StuFromDB.IsDeleted = true;
            //_context.Students.Update(StuFromDB);
            _studentRepository.Update(StuFromDB);
            //_context.SaveChanges();
            _studentRepository.save();
            TempData["NotificationAdded"] = $"Student with Id {id} was Deleted successfully!";
            return RedirectToAction(nameof(ShowAll));
        }
        #endregion

        [HttpGet]
        public IActionResult StudentCourseDetails(int StudentId,int CourseId)
        {
            //fetch the student degree for a course
            var StudentCourse=_context.StuCrsRes
                .Include(scr=>scr.Student)
                .Include(scr=>scr.Course)
                .FirstOrDefault(scr=>scr.StudentId == StudentId &&scr.CourseId==CourseId);

            if (StudentCourse == null)
            {
                return NotFound(); // Return 404 if not found
            }
            var viewModel = new StuCrsResViewModel
            {
                StudentName = StudentCourse.Student.Name,
                CourseName = StudentCourse.Course.Name,
                Grade = StudentCourse.Grade,
                Color = StudentCourse.Grade >= StudentCourse.Course.MinDegree ? "green" : "red"
            };

            return View(nameof(StudentCourseDetails),viewModel);

        }
    }
}
