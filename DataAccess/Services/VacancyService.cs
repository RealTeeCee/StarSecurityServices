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
    public class VacancyService : Repository<Vacancy>, IVacancy
    {
        private readonly StarSecurityDbContext _context;
        public VacancyService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Vacancy obj)
        {
            throw new NotImplementedException();
        }
    }
}
