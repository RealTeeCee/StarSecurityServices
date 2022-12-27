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
        public ICategory Category { get; private set; }

        public ICategoryBranch CategoryBranch { get; private set; }     

        public IService Service { get; private set; }
      
        public ISession Session { get; private set; }

        public ITestimonial Testimonial { get; private set; }

        public IUser User { get; private set; }

        public IUserDetail UserDetail { get; private set; }

        public IVacancy Vacancy { get; private set; }
        

        public IContact Contact { get; private set; }

        public IUserBranch UserBranch { get; private set; }

        public UnitOfWork(StarSecurityDbContext context)
        {
            _context = context;
            Branch = new BranchService(_context);
            Category = new CategoryService(_context);
            CategoryBranch = new CategoryBranchService(_context);              
            Service = new ServicesService(_context);            
            Session = new SessionService(_context);
            Testimonial = new TestimonialService(_context);
            User = new UserService(_context);
            UserDetail = new UserDetailService(_context);            
            Vacancy = new VacancyService(_context);
            Contact = new ContactService(_context);
            UserBranch = new UserBranchService(_context);
        }

        public void ClearTracking()
        {
            _context.ChangeTracker.Clear();
        }

        //CheckVirtual
        public async Task<int> Save() //Hàm này sẽ cần virtual
        {
            return await _context.SaveChangesAsync();
        }
    }
}