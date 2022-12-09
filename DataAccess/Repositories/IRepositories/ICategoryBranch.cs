using DataAccess.Repositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ICategoryBranch : IRepository<CategoryBranch>
    {
        void Update(Branch obj);
    }
}
