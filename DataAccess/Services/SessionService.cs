using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services.ImplementRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class SessionService : Repository<Session>, ISession
    {
        private readonly StarSecurityDbContext _context;
        public SessionService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Session obj)
        {
            throw new NotImplementedException();
        }
    }
}
