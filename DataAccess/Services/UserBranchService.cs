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
    public class UserBranchService : Repository<UserBranch>, IUserBranch
    {
        private readonly StarSecurityDbContext _context;
        public UserBranchService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(UserBranch model)
        {
            var userBranch =  _context.UserBranchs.FirstOrDefault(x => x.Id == model.Id);
            if(userBranch != null)
            {
                userBranch.UserId = model.Id;
                userBranch.BranchId = model.BranchId;
                _context.UserBranchs.Update(userBranch);
            }                      
        }
    }
}
