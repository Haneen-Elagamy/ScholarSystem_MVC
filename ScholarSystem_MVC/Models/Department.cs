using System.ComponentModel.DataAnnotations;

namespace ScholarSystem_MVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Department Name is required")]
        [StringLength(100,ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }=string.Empty;
        [Required(ErrorMessage ="Manager Name is required")]
        [StringLength(100, ErrorMessage = "Manager Name can't be longer than 100 characters")]
        [Display(Name="Manager Name")]
        public string MgrName { get; set; }= string.Empty;
        //Navigation property
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
