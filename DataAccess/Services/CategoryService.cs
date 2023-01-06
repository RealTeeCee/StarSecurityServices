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
            var objFromDb = _context.Categories.FirstOrDefault(x => x.Id == obj.Id);            
            if (objFromDb != null)
            {

                objFromDb.Image = obj.Image;
                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.UpdatedAt = obj.UpdatedAt;
                _context.Categories.Update(objFromDb);
            }
        }
    }
}