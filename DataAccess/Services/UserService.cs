﻿using DataAccess.Data;
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

        public void Update(Branch obj)
        {
            throw new NotImplementedException();
        }
    }
}