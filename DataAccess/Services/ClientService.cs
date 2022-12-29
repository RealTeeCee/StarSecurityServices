using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services.ImplementRepository;
using Models;

namespace DataAccess.Services
{
    public class ClientService : Repository<Client>, IClient
    {
        private readonly StarSecurityDbContext _context;

        public ClientService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Client obj)
        {
            var objFromDb = _context.Clients.FirstOrDefault(x => x.ClientId == obj.ClientId);
            if (objFromDb != null)
            {
                _context.Clients.Update(objFromDb);
            }
        }
    }
}
