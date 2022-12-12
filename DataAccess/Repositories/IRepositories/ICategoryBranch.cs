using DataAccess.Repositories.IRepositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ICategoryBranch : IRepository<CategoryBranch>
    {
        void Update(CategoryBranch obj);
    }
}