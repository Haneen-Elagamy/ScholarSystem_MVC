using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public interface ICourseRepository
    {
        //crud operations
        public void Add(Course obj);
        public void Update(Course obj);
        public void Delete(Course obj);
        public List<Course> GetAll();
        public Course GetById(int id);

        //best practice
        public void save();

        //Extra Methods
        public List<Course> GetAllWithLoading();

        public Course GetByIdWithLoading(int id);

        public void DeleteById (int id);

    }
}
