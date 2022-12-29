using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IBranch Branch { get; }
        IVacancy Vacancy { get; }        
        IContact Contact { get; }
        IUserBranch UserBranch { get; }
        ICategory Category { get; }
        ICategoryBranch CategoryBranch { get; }               
        IService Service { get; }        
        ISession Session { get; }
        ITestimonial Testimonial { get; }
        IUser User { get; }
        IUserDetail UserDetail { get; }
        IClient Client { get; }
        Task<int> Save();
        void ClearTracking();
    }
}