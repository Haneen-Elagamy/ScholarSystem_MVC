using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem_MVC.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Student Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Student Age is required")]
        public int Age { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        //Navigation property
        public Department Department { get; set; }
        public List<StuCrsRes> StuCrsRes { get; set; }
    }
}
