using DataAccess.Repositories.IRepositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ISession : IRepository<Session>
    {
        void Update(Session obj);
    }
}
