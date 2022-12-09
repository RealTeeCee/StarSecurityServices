using DataAccess.Repositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ISession : IRepository<Session>
    {
        void Update(Branch obj);
    }
}
