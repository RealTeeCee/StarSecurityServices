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
    public class UserDetailService : Repository<UserDetail>, IUserDetail
    {
        private readonly StarSecurityDbContext _context;
        public UserDetailService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(UserDetail obj)
        {
            throw new NotImplementedException();
        }
    }
}
