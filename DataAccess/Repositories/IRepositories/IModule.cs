using DataAccess.Repositories.IRepositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface IModule : IRepository<Module>
    {
        void Update(Module obj);
    }
}