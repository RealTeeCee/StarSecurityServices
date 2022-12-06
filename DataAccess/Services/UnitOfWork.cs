using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StarSecurityDbContext _context;
        public IBranch Branch { get; private set; }

        public UnitOfWork(StarSecurityDbContext context)
        {
            _context = context;
            Branch = new BranchService(_context);

        }

        public void ClearTracking()
        {
            throw new NotImplementedException();
        }

        //CheckVirtual
        public async Task<int> Save() //Hàm này sẽ cần virtual
        {
            return await _context.SaveChangesAsync();
        }
    }
}
