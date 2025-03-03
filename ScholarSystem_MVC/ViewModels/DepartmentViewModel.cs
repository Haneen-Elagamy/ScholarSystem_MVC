using Microsoft.AspNetCore.Mvc.Rendering; 

namespace ScholarSystem_MVC.ViewModels
{
    public class DepartmentViewModel
    {
        public String Name { get; set; }
        public List<SelectListItem> StudentsAbove25 { get; set; }
        public string DepartmentState { get; set; }
    }
}
