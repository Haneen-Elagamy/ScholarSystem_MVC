using ScholarSystem_MVC.Models;

namespace ScholarSystem_MVC.Repositories
{
    public interface IDepartmentRepository
    {
        //crud operations
        public void Add(Department obj);
        public void Update(Department obj);
        public void Delete(Department obj);
        public List<Department> GetAll();
        public Department GetById(int id);

        //best practice
        public void save();

        //Extra Methods
        public List<Department> GetAllWithLoading();
    }
}
