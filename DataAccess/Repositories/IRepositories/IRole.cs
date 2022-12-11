﻿using DataAccess.Repositories.GenericRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.IRepositories
{
    public interface IRole : IRepository<Role>
    {
        void Update(Role obj);
    }
}