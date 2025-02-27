using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem_MVC.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        //Navigation property
        public Department Department { get; set; }
        public List<StuCrsRes> StuCrsRes { get; set; }
    }
}
