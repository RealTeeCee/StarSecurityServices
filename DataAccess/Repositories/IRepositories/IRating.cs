using DataAccess.Repositories.IRepositories.GenericRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.IRepositories
{
    public interface IRating : IRepository<Rating>
    {
        void Update(Branch obj);
    }
}
