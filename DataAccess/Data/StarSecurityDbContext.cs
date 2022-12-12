using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data
{
    public class StarSecurityDbContext : IdentityDbContext
    {
        public StarSecurityDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Service> Services { get; set; }


        public DbSet<Session> Sessions { get; set; }




    }
}
