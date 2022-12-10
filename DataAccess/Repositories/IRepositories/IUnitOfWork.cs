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
        ICategory Category { get; }
        ICategoryBranch CategoryBranch { get; }
        ICategoryTranslation CategoryTranslation { get; }
        ILanguage Language { get; }
        IModule Module { get; }
        IRole Role { get; }
        IService Service { get; }
        IServiceTranslation ServiceTranslation { get; }
        ISession Session { get; }
        ITestimonial Testimonial { get; }
        IUser User { get; }
        IUserDetail UserDetail { get; }
        Task<int> Save();
        void ClearTracking();
    }
}