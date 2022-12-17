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

        public bool Delete(long id)
        {
            var objFromDb = _context.Testimonials.FirstOrDefault(x => x.Id == id);
            
            if (objFromDb == null) return false;

            _context.Testimonials.Remove(objFromDb);

            int count = _context.SaveChanges();
            return (count > 0);
        }

        public Testimonial? Detail(long id)
        {
            var objFromDb = _context.Testimonials.FirstOrDefault(x => x.Id == id);

            return objFromDb;
        }

        public List<Testimonial> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(long id, Testimonial obj)
        {
            var objFromDb = _context.Testimonials.FirstOrDefault(x => x.Id == id);
            if (objFromDb != null)
            {

                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.UpdatedAt = obj.UpdatedAt;
                _context.Testimonials.Update(objFromDb);
            }
        }

        public bool Create(Testimonial obj)
        {
            if (string.IsNullOrEmpty(obj.Name)
                || string.IsNullOrEmpty(obj.Title)
                || string.IsNullOrEmpty(obj.Description)) return false;

            _context.Testimonials.Add(obj);

            int count = _context.SaveChanges();
            return (count > 0);
        }
    }
}
