using Microsoft.AspNetCore.Mvc;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;
using ScholarSystem_MVC.Repositories;

namespace ScholarSystem_MVC.Controllers
{
    public class CourseController : Controller
    {

        //ScholarSystemDbContext Context = new ScholarSystemDbContext();
        //CourseBL courseBL = new CourseBL();
        private readonly ScholarSystemDbContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public CourseController(ScholarSystemDbContext context,ICourseRepository courseRepository, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
        }

        #region Show All
        public IActionResult ShowAll()
        {
            List<Course> CoursesList = _courseRepository.GetAllWithLoading();
            return View(nameof(ShowAll), CoursesList);
        } 
        #endregion 

        #region Show Details
        public IActionResult ShowDetails(int id)
        {
            Course course = _courseRepository.GetByIdWithLoading(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(nameof(ShowDetails), course);
        } 
        #endregion

        #region Add

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return View(nameof(Add));
        }

        [HttpPost]
        public IActionResult SaveAdd(Course courseSent)
        {
            if (ModelState.IsValid)
            {
                _courseRepository.Add(courseSent);
                _courseRepository.save();
                TempData["NotificationAdded"] = "Course was Added Successfully!";
                return RedirectToAction(nameof(ShowAll));
            }

            ViewBag.Departments = _departmentRepository.GetAll();
            return View(nameof(Add), courseSent);
        } 
        #endregion

        #region Edit 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course courseFromReq = _courseRepository.GetById(id);
            if (courseFromReq == null)
            {
                return NotFound();
            }
            ViewBag.Departments = _departmentRepository.GetAll();
            return View(nameof(Edit), courseFromReq);
        }

        [HttpPost]
        public IActionResult SaveEdit(Course courseSent)
        {
            if (ModelState.IsValid)
            {
                _courseRepository.Update(courseSent);
                _courseRepository.save();
                TempData["NotificationAdded"] = "Course was Edited Successfully!";
                return RedirectToAction(nameof(ShowAll));
            }
            ViewBag.Departments = _courseRepository.GetAll();
            return View(nameof(Edit), courseSent);

        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Course courseFromReq = _courseRepository.GetByIdWithLoading(id);
            if (courseFromReq == null)
            {
                return NotFound();
            }
            ViewBag.Departments = _departmentRepository.GetAll();
            return View(nameof(Delete), courseFromReq);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _courseRepository.DeleteById(id);
            _courseRepository.save();
            TempData["NotificationAdded"] = "Course was deleted Successfully!";
            return RedirectToAction(nameof(ShowAll));
        }
        #endregion
       
    }
}
