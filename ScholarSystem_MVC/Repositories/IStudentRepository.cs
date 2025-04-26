using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public interface IStudentRepository
    {
        //crud operations
        public void Add(Student obj);
        public void Update(Student obj);
        public void Delete(Student obj);
        public List<Student> GetAll();
        public Student GetById(int id);

        //best practice
        public void save();

        //Extra Methods
        public List<Student> GetAllWithLoading();
        public Student GetByIdWithLoading(int id);
    }
}
