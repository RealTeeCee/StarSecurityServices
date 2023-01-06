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
    public class ClientDetailService : Repository<ClientDetail>, IClientDetail
    {
        private readonly StarSecurityDbContext _context;

        public ClientDetailService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(ClientDetail obj)
        {
            var objFromDb = _context.ClientDetails.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                _context.ClientDetails.Update(objFromDb);
            }
        }
    }
}
