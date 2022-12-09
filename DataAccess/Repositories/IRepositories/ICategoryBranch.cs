using DataAccess.Repositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ICategoryBranch : IRepository<CategoriesBranches>
    {
        void Update(Branch obj);
    }
}
