using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarSystem_MVC.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required (ErrorMessage=" Course Name is Required")]
        [MaxLength(100 ,ErrorMessage= "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Max Degree is Required")]
        [Range(0, 100, ErrorMessage = " Degree must be between 0 and 100")]
        public double Degree { get; set; }

        [Required(ErrorMessage = " Min Degree is Required")]
        [Range(0, 100, ErrorMessage = "Minimum Degree must be between 0 and 100")]
        [CustomMinDegree(ErrorMessage = "Minimum Degree must not exceed the Degree")]
        public double MinDegree { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("Department")]
        [Required(ErrorMessage = "Department is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        //Navigation property
        public Department Department { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<StuCrsRes> StuCrsRes { get; set; }

    }


    public class CustomMinDegreeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (Course)validationContext.ObjectInstance;

            if (course.MinDegree > course.Degree)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
