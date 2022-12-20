using DataAccess.Repositories.IRepositories.GenericRepositories;
using Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface ITestimonial : IRepository<Testimonial>
    {
        void Update(Testimonial obj);
    }
}