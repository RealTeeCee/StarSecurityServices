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
            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict; //Bat buoc delete subTable item truoc parentTable item (ON DELETE NO ACTION)
            //}
            modelBuilder.Seed();            
        }
        public DbSet<Branch> Branches { get; set; }
                
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserBranch> UserBranchs { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryBranch> CategoryBranches { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        //public DbSet<Project> Projects { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<ClientDetail> ClientDetails { get; set; }
    }
}
