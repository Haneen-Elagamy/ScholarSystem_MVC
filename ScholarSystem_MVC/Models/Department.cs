using System.ComponentModel.DataAnnotations;

namespace ScholarSystem_MVC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MgrName { get; set; }
        //Navigation property
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
