using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class StarSecurityDbContext : DbContext
    {
        public StarSecurityDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Branch> Branches { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        //public DbSet<Language> Languages { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<ServiceTranslation> ServiceTranslations { get; set; }
        //public DbSet<Session> Sessions { get; set; }
        //public DbSet<Testimonial> Testimonials { get; set; }
        //public DbSet<UserDetail> UserDetails { get; set; }
        //public DbSet<User> Users { get; set; }
    }
}
