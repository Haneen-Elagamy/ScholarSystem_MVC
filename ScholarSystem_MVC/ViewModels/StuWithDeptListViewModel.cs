using ScholarSystem_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem_MVC.ViewModels
{
    public class StuWithDeptListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name="Department")]
        public int DepartmentId { get; set; }
        public List<Department> DeptList { get; set; }
    }
}
