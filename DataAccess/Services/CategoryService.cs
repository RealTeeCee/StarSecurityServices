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
    public class CategoryService : Repository<Category>, ICategory
    {
        private readonly StarSecurityDbContext _context;
        public CategoryService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Category obj)
        {
            throw new NotImplementedException();
        }
    }
}