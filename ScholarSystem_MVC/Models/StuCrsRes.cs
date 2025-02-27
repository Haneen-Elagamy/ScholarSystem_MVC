using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarSystem_MVC.Models
{
    public class StuCrsRes
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public double Grade { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
