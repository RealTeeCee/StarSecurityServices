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

        public List<Testimonial> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Testimonial obj)
        {
            var objFromDb = _context.Testimonials.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {

                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.UpdatedAt = obj.UpdatedAt;
                _context.Testimonials.Update(objFromDb);
            }
        }
    }
}
