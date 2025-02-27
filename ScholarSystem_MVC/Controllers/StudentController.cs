using Microsoft.AspNetCore.Mvc;
using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAll()
        {
            StudentBL studentBL = new StudentBL();
            List<Student> StudentListModel = studentBL.GetAll();
            return View("ShowAll", StudentListModel);
        }

        public IActionResult ShowDetails(int id)
        {
            StudentBL studentBL=new StudentBL();
            Student student=studentBL.GetById(id);
            return View("ShowDetails",student);
        }
        
    }
}
