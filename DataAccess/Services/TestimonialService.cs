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
    public class TestimonialService : Repository<Testimonial>, ITestimonial
    {
        private readonly StarSecurityDbContext _context;
        
        public TestimonialService(StarSecurityDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(long id, Testimonial obj)
        {
            try
            {
                var mdl = _context.Testimonials.FirstOrDefault(x => x.Id == id);
                if (mdl == null)
                {
                    return;
                }

                mdl.CopyFromNotNull(obj);
                mdl.UpdatedAt = DateTime.Now;

                int count = _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
