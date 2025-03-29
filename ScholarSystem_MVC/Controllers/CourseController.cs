using Microsoft.AspNetCore.Mvc;
using ScholarSystem_MVC.DbContexts;
using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Controllers
{
    public class CourseController : Controller
    {

        ScholarSystemDbContext Context = new ScholarSystemDbContext();
        CourseBL courseBL = new CourseBL();

        #region Show All
        public IActionResult ShowAll()
        {
            List<Course> CoursesList = courseBL.GetAll();
            return View(nameof(ShowAll), CoursesList);
        } 
        #endregion 

        #region Show Details
        public IActionResult ShowDetails(int id)
        {
            Course course = courseBL.GetById(id);
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
            ViewBag.Departments = courseBL.GetDepartments();
            return View(nameof(Add));
        }

        [HttpPost]
        public IActionResult SaveAdd(Course courseSent)
        {
            if (ModelState.IsValid)
            {
                Context.Courses.Add(courseSent);
                return RedirectToAction(nameof(ShowAll));
            }

            ViewBag.Departments = courseBL.GetDepartments();
            return View(nameof(Add), courseSent);
        } 
        #endregion

        #region Edit 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course courseFromReq = courseBL.GetById(id);
            if (courseFromReq == null)
            {
                return NotFound();
            }
            ViewBag.Departments = courseBL.GetDepartments();
            return View(nameof(Edit), courseFromReq);
        }

        [HttpPost]
        public IActionResult SaveEdit(Course courseSent)
        {
            if (ModelState.IsValid)
            {
                courseBL.Update(courseSent);
                return RedirectToAction(nameof(ShowAll));
            }
            ViewBag.Departments = courseBL.GetDepartments();
            return View(nameof(Edit), courseSent);

        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Course courseFromReq = courseBL.GetById(id);
            if (courseFromReq == null)
            {
                return NotFound();
            }
            ViewBag.Departments = courseBL.GetDepartments();
            return View(nameof(Delete), courseFromReq);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            courseBL.Delete(id);
            return RedirectToAction(nameof(ShowAll));
        }
        #endregion
       
    }
}
