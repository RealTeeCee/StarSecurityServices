using DataAccess.Repositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface IModule : IRepository<Module>
    {
        void Update(Module obj);
    }
}
