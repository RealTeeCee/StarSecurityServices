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
    public class UserService : Repository<User>, IUser
    {
        private readonly StarSecurityDbContext _context;
        public UserService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(User model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == model.Id);
            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Phone = model.Phone;
                user.Address = model.Address;
                user.Image = model.Image;
                user.RoleId = model.RoleId;
                user.Status = model.Status;
                user.UpdatedAt = DateTime.Now;
                _context.Users.Update(user);
            }
        }
    }
}
